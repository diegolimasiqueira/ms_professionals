using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using MSProfessionals.Application.Commands.TimeZone;
using MSProfessionals.Domain.Entities;
using MSProfessionals.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace MSProfessionals.UnitTests.Commands.TimeZone;

public class GetTimeZonesCommandHandlerTests
{
    private readonly Mock<ITimeZoneRepository> _timeZoneRepositoryMock;
    private readonly GetTimeZonesCommandHandler _handler;

    public GetTimeZonesCommandHandlerTests()
    {
        _timeZoneRepositoryMock = new Mock<ITimeZoneRepository>();
        _handler = new GetTimeZonesCommandHandler(_timeZoneRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnPaginatedList_WhenValidRequest()
    {
        // Arrange
        var request = new GetTimeZonesCommand
        {
            PageNumber = 1,
            PageSize = 10
        };

        var timeZones = new List<MSProfessionals.Domain.Entities.TimeZone>
        {
            new() { Id = Guid.NewGuid(), Name = "America/Sao_Paulo", Description = "Horário de Brasília" },
            new() { Id = Guid.NewGuid(), Name = "Europe/London", Description = "Horário de Londres" }
        };

        _timeZoneRepositoryMock
            .Setup(x => x.CountAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(timeZones.Count);

        _timeZoneRepositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<MSProfessionals.Domain.Entities.TimeZone>)timeZones);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.PageNumber.Should().Be(request.PageNumber);
        result.PageSize.Should().Be(request.PageSize);
        result.TotalItems.Should().Be(timeZones.Count);
        result.Items.Should().HaveCount(timeZones.Count);
        result.Items.Should().BeInAscendingOrder(x => x.Description);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoData()
    {
        // Arrange
        var request = new GetTimeZonesCommand
        {
            PageNumber = 1,
            PageSize = 10
        };

        _timeZoneRepositoryMock
            .Setup(x => x.CountAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        _timeZoneRepositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<MSProfessionals.Domain.Entities.TimeZone>)new List<MSProfessionals.Domain.Entities.TimeZone>());

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
        var request = new GetTimeZonesCommand
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
        var request = new GetTimeZonesCommand
        {
            PageNumber = 1,
            PageSize = pageSize
        };

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(request, CancellationToken.None));
    }
} 