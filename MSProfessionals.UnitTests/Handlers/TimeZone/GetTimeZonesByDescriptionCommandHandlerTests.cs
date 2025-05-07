using FluentAssertions;
using MediatR;
using Moq;
using MSProfessionals.Application.Commands.TimeZone;
using MSProfessionals.Domain.Entities;
using MSProfessionals.Domain.Interfaces;
using Xunit;

namespace MSProfessionals.UnitTests.Handlers.TimeZone;

public class GetTimeZonesByDescriptionCommandHandlerTests
{
    private readonly Mock<ITimeZoneRepository> _timeZoneRepositoryMock;
    private readonly GetTimeZonesByDescriptionCommandHandler _handler;

    public GetTimeZonesByDescriptionCommandHandlerTests()
    {
        _timeZoneRepositoryMock = new Mock<ITimeZoneRepository>();
        _handler = new GetTimeZonesByDescriptionCommandHandler(_timeZoneRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnPaginatedTimeZones()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;
        var totalItems = 25;
        var description = "Universal";
        var timeZones = new List<MSProfessionals.Domain.Entities.TimeZone>
        {
            new() { Id = Guid.NewGuid(), Name = "UTC", Description = "Coordinated Universal Time" },
            new() { Id = Guid.NewGuid(), Name = "GMT", Description = "Greenwich Mean Time (Universal)" }
        };

        var command = new GetTimeZonesByDescriptionCommand 
        { 
            PageNumber = pageNumber, 
            PageSize = pageSize,
            Description = description
        };

        _timeZoneRepositoryMock.Setup(x => x.GetByDescriptionAsync(description, (pageNumber - 1) * pageSize, pageSize, It.IsAny<CancellationToken>()))
            .ReturnsAsync(timeZones);

        _timeZoneRepositoryMock.Setup(x => x.CountByDescriptionAsync(description, It.IsAny<CancellationToken>()))
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
        var description = "NonExistent";
        var command = new GetTimeZonesByDescriptionCommand 
        { 
            PageNumber = pageNumber, 
            PageSize = pageSize,
            Description = description
        };

        _timeZoneRepositoryMock.Setup(x => x.GetByDescriptionAsync(description, (pageNumber - 1) * pageSize, pageSize, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<MSProfessionals.Domain.Entities.TimeZone>());

        _timeZoneRepositoryMock.Setup(x => x.CountByDescriptionAsync(description, It.IsAny<CancellationToken>()))
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
        var command = new GetTimeZonesByDescriptionCommand 
        { 
            PageNumber = 0, 
            PageSize = 10,
            Description = "Universal"
        };

        // Act & Assert
        await Assert.ThrowsAsync<System.ComponentModel.DataAnnotations.ValidationException>(() => 
            _handler.Handle(command, CancellationToken.None));
        _timeZoneRepositoryMock.Verify(x => x.GetByDescriptionAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        _timeZoneRepositoryMock.Verify(x => x.CountByDescriptionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenPageSizeIsLessThanOne_ShouldThrowException()
    {
        // Arrange
        var command = new GetTimeZonesByDescriptionCommand 
        { 
            PageNumber = 1, 
            PageSize = 0,
            Description = "Universal"
        };

        // Act & Assert
        await Assert.ThrowsAsync<System.ComponentModel.DataAnnotations.ValidationException>(() => 
            _handler.Handle(command, CancellationToken.None));
        _timeZoneRepositoryMock.Verify(x => x.GetByDescriptionAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        _timeZoneRepositoryMock.Verify(x => x.CountByDescriptionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenDescriptionIsEmpty_ShouldThrowException()
    {
        // Arrange
        var command = new GetTimeZonesByDescriptionCommand 
        { 
            PageNumber = 1, 
            PageSize = 10,
            Description = string.Empty
        };

        // Act & Assert
        await Assert.ThrowsAsync<System.ComponentModel.DataAnnotations.ValidationException>(() => 
            _handler.Handle(command, CancellationToken.None));
        _timeZoneRepositoryMock.Verify(x => x.GetByDescriptionAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        _timeZoneRepositoryMock.Verify(x => x.CountByDescriptionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }
} 