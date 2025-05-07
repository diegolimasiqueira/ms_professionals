using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using MSProfessionals.Application.Commands.Profession;
using MSProfessionals.Domain.Entities;
using MSProfessionals.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace MSProfessionals.UnitTests.Commands.Profession;

public class GetProfessionsByDescriptionCommandHandlerTests
{
    private readonly Mock<IProfessionRepository> _professionRepositoryMock;
    private readonly GetProfessionsCommandHandler _handler;

    public GetProfessionsByDescriptionCommandHandlerTests()
    {
        _professionRepositoryMock = new Mock<IProfessionRepository>();
        _handler = new GetProfessionsCommandHandler(_professionRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnPaginatedList_WhenValidRequest()
    {
        // Arrange
        var request = new GetProfessionsCommand
        {
            Name = "Médico",
            PageNumber = 1,
            PageSize = 10
        };

        var professions = new List<MSProfessionals.Domain.Entities.Profession>
        {
            new() { Id = Guid.NewGuid(), Name = "Médico" },
            new() { Id = Guid.NewGuid(), Name = "Médico Especialista" }
        };

        _professionRepositoryMock
            .Setup(x => x.CountAsync(request.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(professions.Count);

        _professionRepositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), request.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<MSProfessionals.Domain.Entities.Profession>)professions);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.PageNumber.Should().Be(request.PageNumber);
        result.PageSize.Should().Be(request.PageSize);
        result.TotalCount.Should().Be(professions.Count);
        result.Items.Should().HaveCount(professions.Count);
        result.Items.Should().BeInAscendingOrder(x => x.Name);
        result.Items.Should().AllSatisfy(x => x.Name.Should().Contain(request.Name));
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoData()
    {
        // Arrange
        var request = new GetProfessionsCommand
        {
            Name = "Médico",
            PageNumber = 1,
            PageSize = 10
        };

        _professionRepositoryMock
            .Setup(x => x.CountAsync(request.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        _professionRepositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), request.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<MSProfessionals.Domain.Entities.Profession>)new List<MSProfessionals.Domain.Entities.Profession>());

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.PageNumber.Should().Be(request.PageNumber);
        result.PageSize.Should().Be(request.PageSize);
        result.TotalCount.Should().Be(0);
        result.Items.Should().BeEmpty();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task Handle_ShouldThrowValidationException_WhenInvalidPageNumber(int pageNumber)
    {
        // Arrange
        var request = new GetProfessionsCommand
        {
            Name = "Médico",
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
        var request = new GetProfessionsCommand
        {
            Name = "Médico",
            PageNumber = 1,
            PageSize = pageSize
        };

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(request, CancellationToken.None));
    }
} 