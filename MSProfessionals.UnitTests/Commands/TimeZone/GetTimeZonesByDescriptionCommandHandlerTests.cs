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
    public async Task Handle_ShouldReturnPaginatedList_WhenValidRequest()
    {
        // Arrange
        var request = new GetTimeZonesByDescriptionCommand
        {
            Description = "Brasília",
            PageNumber = 1,
            PageSize = 10
        };

        var timeZones = new List<MSProfessionals.Domain.Entities.TimeZone>
        {
            new() { Id = Guid.NewGuid(), Name = "America/Sao_Paulo", Description = "Horário de Brasília" }
        };

        _timeZoneRepositoryMock
            .Setup(x => x.CountByDescriptionAsync(request.Description, It.IsAny<CancellationToken>()))
            .ReturnsAsync(timeZones.Count);

        _timeZoneRepositoryMock
            .Setup(x => x.GetByDescriptionAsync(request.Description, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<MSProfessionals.Domain.Entities.TimeZone>)timeZones);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.PageNumber.Should().Be(request.PageNumber);
        result.PageSize.Should().Be(request.PageSize);
        result.TotalItems.Should().Be(timeZones.Count);
        result.Items.Should().HaveCount(timeZones.Count);
        result.Items.Should().AllSatisfy(x => x.Description.Should().Contain(request.Description));
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoData()
    {
        // Arrange
        var request = new GetTimeZonesByDescriptionCommand
        {
            Description = "Brasília",
            PageNumber = 1,
            PageSize = 10
        };

        _timeZoneRepositoryMock
            .Setup(x => x.CountByDescriptionAsync(request.Description, It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        _timeZoneRepositoryMock
            .Setup(x => x.GetByDescriptionAsync(request.Description, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
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
        var request = new GetTimeZonesByDescriptionCommand
        {
            Description = "Brasília",
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
        var request = new GetTimeZonesByDescriptionCommand
        {
            Description = "Brasília",
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
        var request = new GetTimeZonesByDescriptionCommand
        {
            Description = string.Empty,
            PageNumber = 1,
            PageSize = 10
        };

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(request, CancellationToken.None));
    }
} 