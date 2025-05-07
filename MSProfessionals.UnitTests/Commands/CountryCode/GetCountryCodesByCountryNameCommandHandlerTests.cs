using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using MSProfessionals.Application.Commands.CountryCode;
using MSProfessionals.Domain.Entities;
using MSProfessionals.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace MSProfessionals.UnitTests.Commands.CountryCode;

public class GetCountryCodesByCountryNameCommandHandlerTests
{
    private readonly Mock<ICountryCodeRepository> _countryCodeRepositoryMock;
    private readonly GetCountryCodesByCountryNameCommandHandler _handler;

    public GetCountryCodesByCountryNameCommandHandlerTests()
    {
        _countryCodeRepositoryMock = new Mock<ICountryCodeRepository>();
        _handler = new GetCountryCodesByCountryNameCommandHandler(_countryCodeRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnPaginatedList_WhenValidRequest()
    {
        // Arrange
        var request = new GetCountryCodesByCountryNameCommand
        {
            CountryName = "Brasil",
            PageNumber = 1,
            PageSize = 10
        };

        var countryCodes = new List<MSProfessionals.Domain.Entities.CountryCode>
        {
            new(Guid.NewGuid(), "BR", "Brasil")
        };

        _countryCodeRepositoryMock
            .Setup(x => x.CountByCountryNameAsync(request.CountryName, It.IsAny<CancellationToken>()))
            .ReturnsAsync(countryCodes.Count);

        _countryCodeRepositoryMock
            .Setup(x => x.GetByCountryNameAsync(request.CountryName, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(countryCodes);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.PageNumber.Should().Be(request.PageNumber);
        result.PageSize.Should().Be(request.PageSize);
        result.TotalItems.Should().Be(countryCodes.Count);
        result.Items.Should().HaveCount(countryCodes.Count);
        result.Items.Should().AllSatisfy(x => x.CountryName.Should().Contain(request.CountryName));
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoData()
    {
        // Arrange
        var request = new GetCountryCodesByCountryNameCommand
        {
            CountryName = "Brasil",
            PageNumber = 1,
            PageSize = 10
        };

        _countryCodeRepositoryMock
            .Setup(x => x.CountByCountryNameAsync(request.CountryName, It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        _countryCodeRepositoryMock
            .Setup(x => x.GetByCountryNameAsync(request.CountryName, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<MSProfessionals.Domain.Entities.CountryCode>)new List<MSProfessionals.Domain.Entities.CountryCode>());

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
        var request = new GetCountryCodesByCountryNameCommand
        {
            CountryName = "Brasil",
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
        var request = new GetCountryCodesByCountryNameCommand
        {
            CountryName = "Brasil",
            PageNumber = 1,
            PageSize = pageSize
        };

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrowValidationException_WhenCountryNameIsEmpty()
    {
        // Arrange
        var request = new GetCountryCodesByCountryNameCommand
        {
            CountryName = string.Empty,
            PageNumber = 1,
            PageSize = 10
        };

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(request, CancellationToken.None));
    }
} 