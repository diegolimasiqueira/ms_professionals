using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using MSProfessionals.Application.Commands.Language;
using MSProfessionals.Domain.Entities;
using MSProfessionals.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace MSProfessionals.UnitTests.Commands.Language;

public class GetLanguagesByDescriptionCommandHandlerTests
{
    private readonly Mock<ILanguageRepository> _languageRepositoryMock;
    private readonly GetLanguagesByDescriptionCommandHandler _handler;

    public GetLanguagesByDescriptionCommandHandlerTests()
    {
        _languageRepositoryMock = new Mock<ILanguageRepository>();
        _handler = new GetLanguagesByDescriptionCommandHandler(_languageRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnPaginatedList_WhenValidRequest()
    {
        // Arrange
        var request = new GetLanguagesByDescriptionCommand
        {
            Description = "Português",
            PageNumber = 1,
            PageSize = 10
        };

        var languages = new List<MSProfessionals.Domain.Entities.Language>
        {
            new() { Id = Guid.NewGuid(), Code = "pt-BR", Description = "Português do Brasil" }
        };

        _languageRepositoryMock
            .Setup(x => x.CountByDescriptionAsync(request.Description, It.IsAny<CancellationToken>()))
            .ReturnsAsync(languages.Count);

        _languageRepositoryMock
            .Setup(x => x.GetByDescriptionAsync(request.Description, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<MSProfessionals.Domain.Entities.Language>)languages);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.PageNumber.Should().Be(request.PageNumber);
        result.PageSize.Should().Be(request.PageSize);
        result.TotalItems.Should().Be(languages.Count);
        result.Items.Should().HaveCount(languages.Count);
        result.Items.Should().AllSatisfy(x => x.Description.Should().Contain(request.Description));
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoData()
    {
        // Arrange
        var request = new GetLanguagesByDescriptionCommand
        {
            Description = "Português",
            PageNumber = 1,
            PageSize = 10
        };

        _languageRepositoryMock
            .Setup(x => x.CountByDescriptionAsync(request.Description, It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        _languageRepositoryMock
            .Setup(x => x.GetByDescriptionAsync(request.Description, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<MSProfessionals.Domain.Entities.Language>)new List<MSProfessionals.Domain.Entities.Language>());

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
        var request = new GetLanguagesByDescriptionCommand
        {
            Description = "Português",
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
        var request = new GetLanguagesByDescriptionCommand
        {
            Description = "Português",
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
        var request = new GetLanguagesByDescriptionCommand
        {
            Description = string.Empty,
            PageNumber = 1,
            PageSize = 10
        };

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(request, CancellationToken.None));
    }
} 