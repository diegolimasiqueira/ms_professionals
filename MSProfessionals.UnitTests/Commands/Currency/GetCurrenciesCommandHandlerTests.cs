using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using MSProfessionals.Application.Commands.Currency;
using MSProfessionals.Domain.Entities;
using MSProfessionals.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace MSProfessionals.UnitTests.Commands.Currency;

public class GetCurrenciesCommandHandlerTests
{
    private readonly Mock<ICurrencyRepository> _currencyRepositoryMock;
    private readonly GetCurrenciesCommandHandler _handler;

    public GetCurrenciesCommandHandlerTests()
    {
        _currencyRepositoryMock = new Mock<ICurrencyRepository>();
        _handler = new GetCurrenciesCommandHandler(_currencyRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnPaginatedList_WhenValidRequest()
    {
        // Arrange
        var request = new GetCurrenciesCommand
        {
            PageNumber = 1,
            PageSize = 10
        };

        var currencies = new List<MSProfessionals.Domain.Entities.Currency>
        {
            new() { Id = Guid.NewGuid(), Code = "BRL", Description = "Real Brasileiro" },
            new() { Id = Guid.NewGuid(), Code = "USD", Description = "Dólar Americano" },
            new() { Id = Guid.NewGuid(), Code = "EUR", Description = "Euro" }
        };

        _currencyRepositoryMock
            .Setup(x => x.CountAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(currencies.Count);

        _currencyRepositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(currencies);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.PageNumber.Should().Be(request.PageNumber);
        result.PageSize.Should().Be(request.PageSize);
        result.TotalItems.Should().Be(currencies.Count);
        result.Items.Should().HaveCount(currencies.Count);
        result.Items.Should().BeInAscendingOrder(x => x.Description);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoData()
    {
        // Arrange
        var request = new GetCurrenciesCommand
        {
            PageNumber = 1,
            PageSize = 10
        };

        _currencyRepositoryMock
            .Setup(x => x.CountAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        _currencyRepositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<MSProfessionals.Domain.Entities.Currency>)new List<MSProfessionals.Domain.Entities.Currency>());

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.PageNumber.Should().Be(request.PageNumber);
        result.PageSize.Should().Be(request.PageSize);
        result.TotalItems.Should().Be(0);
        result.Items.Should().BeEmpty();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task Handle_ShouldThrowValidationException_WhenInvalidPageNumber(int pageNumber)
    {
        // Arrange
        var request = new GetCurrenciesCommand
        {
            PageNumber = pageNumber,
            PageSize = 10
        };

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(request, CancellationToken.None));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(101)]
    public async Task Handle_ShouldThrowValidationException_WhenInvalidPageSize(int pageSize)
    {
        // Arrange
        var request = new GetCurrenciesCommand
        {
            PageNumber = 1,
            PageSize = pageSize
        };

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(request, CancellationToken.None));
    }
} 