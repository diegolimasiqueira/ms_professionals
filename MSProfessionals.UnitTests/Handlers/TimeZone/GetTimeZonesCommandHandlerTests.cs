using FluentAssertions;
using MediatR;
using Moq;
using MSProfessionals.Application.Commands.TimeZone;
using MSProfessionals.Domain.Entities;
using MSProfessionals.Domain.Interfaces;
using Xunit;

namespace MSProfessionals.UnitTests.Handlers.TimeZone;

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
    public async Task Handle_ShouldReturnPaginatedTimeZones()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;
        var totalItems = 25;
        var timeZones = new List<MSProfessionals.Domain.Entities.TimeZone>
        {
            new() { Id = Guid.NewGuid(), Name = "UTC", Description = "Coordinated Universal Time" },
            new() { Id = Guid.NewGuid(), Name = "GMT", Description = "Greenwich Mean Time" }
        };

        var command = new GetTimeZonesCommand { PageNumber = pageNumber, PageSize = pageSize };

        _timeZoneRepositoryMock.Setup(x => x.GetAllAsync((pageNumber - 1) * pageSize, pageSize, It.IsAny<CancellationToken>()))
            .ReturnsAsync(timeZones);

        _timeZoneRepositoryMock.Setup(x => x.CountAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(totalItems);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.PageNumber.Should().Be(pageNumber);
        result.PageSize.Should().Be(pageSize);
        result.TotalItems.Should().Be(totalItems);
        result.TotalPages.Should().Be((int)Math.Ceiling(totalItems / (double)pageSize));
        result.Items.Should().HaveCount(2);

        var firstTimeZone = result.Items.First();
        firstTimeZone.Id.Should().Be(timeZones[0].Id);
        firstTimeZone.Code.Should().Be(timeZones[0].Name);
        firstTimeZone.Description.Should().Be(timeZones[0].Description);

        var secondTimeZone = result.Items.Last();
        secondTimeZone.Id.Should().Be(timeZones[1].Id);
        secondTimeZone.Code.Should().Be(timeZones[1].Name);
        secondTimeZone.Description.Should().Be(timeZones[1].Description);
    }

    [Fact]
    public async Task Handle_WhenNoTimeZonesExist_ShouldReturnEmptyList()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;
        var command = new GetTimeZonesCommand { PageNumber = pageNumber, PageSize = pageSize };

        _timeZoneRepositoryMock.Setup(x => x.GetAllAsync((pageNumber - 1) * pageSize, pageSize, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<MSProfessionals.Domain.Entities.TimeZone>());

        _timeZoneRepositoryMock.Setup(x => x.CountAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.PageNumber.Should().Be(pageNumber);
        result.PageSize.Should().Be(pageSize);
        result.TotalItems.Should().Be(0);
        result.TotalPages.Should().Be(0);
        result.Items.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_WhenPageNumberIsLessThanOne_ShouldThrowException()
    {
        // Arrange
        var command = new GetTimeZonesCommand { PageNumber = 0, PageSize = 10 };

        // Act & Assert
        await Assert.ThrowsAsync<System.ComponentModel.DataAnnotations.ValidationException>(() => 
            _handler.Handle(command, CancellationToken.None));
        _timeZoneRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        _timeZoneRepositoryMock.Verify(x => x.CountAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenPageSizeIsLessThanOne_ShouldThrowException()
    {
        // Arrange
        var command = new GetTimeZonesCommand { PageNumber = 1, PageSize = 0 };

        // Act & Assert
        await Assert.ThrowsAsync<System.ComponentModel.DataAnnotations.ValidationException>(() => 
            _handler.Handle(command, CancellationToken.None));
        _timeZoneRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        _timeZoneRepositoryMock.Verify(x => x.CountAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
} 