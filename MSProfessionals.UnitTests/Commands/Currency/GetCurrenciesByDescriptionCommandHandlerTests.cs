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

public class GetCurrenciesByDescriptionCommandHandlerTests
{
    private readonly Mock<ICurrencyRepository> _currencyRepositoryMock;
    private readonly GetCurrenciesByDescriptionCommandHandler _handler;

    public GetCurrenciesByDescriptionCommandHandlerTests()
    {
        _currencyRepositoryMock = new Mock<ICurrencyRepository>();
        _handler = new GetCurrenciesByDescriptionCommandHandler(_currencyRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnPaginatedList_WhenValidRequest()
    {
        // Arrange
        var request = new GetCurrenciesByDescriptionCommand
        {
            Description = "Real",
            PageNumber = 1,
            PageSize = 10
        };

        var currencies = new List<MSProfessionals.Domain.Entities.Currency>
        {
            new() { Id = Guid.NewGuid(), Code = "BRL", Description = "Real Brasileiro" }
        };

        _currencyRepositoryMock
            .Setup(x => x.CountByDescriptionAsync(request.Description, It.IsAny<CancellationToken>()))
            .ReturnsAsync(currencies.Count);

        _currencyRepositoryMock
            .Setup(x => x.GetByDescriptionAsync(request.Description, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<MSProfessionals.Domain.Entities.Currency>)currencies);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.PageNumber.Should().Be(request.PageNumber);
        result.PageSize.Should().Be(request.PageSize);
        result.TotalItems.Should().Be(currencies.Count);
        result.Items.Should().HaveCount(currencies.Count);
        result.Items.Should().AllSatisfy(x => x.Description.Should().Contain(request.Description));
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoData()
    {
        // Arrange
        var request = new GetCurrenciesByDescriptionCommand
        {
            Description = "Real",
            PageNumber = 1,
            PageSize = 10
        };

        _currencyRepositoryMock
            .Setup(x => x.CountByDescriptionAsync(request.Description, It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        _currencyRepositoryMock
            .Setup(x => x.GetByDescriptionAsync(request.Description, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
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
        var request = new GetCurrenciesByDescriptionCommand
        {
            Description = "Real",
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
        var request = new GetCurrenciesByDescriptionCommand
        {
            Description = "Real",
            PageNumber = 1,
            PageSize = pageSize
        };

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrowValidationException_WhenDescriptionIsEmpty()
    {
        // Arrange
        var request = new GetCurrenciesByDescriptionCommand
        {
            Description = string.Empty,
            PageNumber = 1,
            PageSize = 10
        };

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(request, CancellationToken.None));
    }
} 