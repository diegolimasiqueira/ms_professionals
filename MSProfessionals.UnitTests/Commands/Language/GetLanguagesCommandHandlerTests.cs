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

public class GetLanguagesCommandHandlerTests
{
    private readonly Mock<ILanguageRepository> _languageRepositoryMock;
    private readonly GetLanguagesCommandHandler _handler;

    public GetLanguagesCommandHandlerTests()
    {
        _languageRepositoryMock = new Mock<ILanguageRepository>();
        _handler = new GetLanguagesCommandHandler(_languageRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnPaginatedList_WhenValidRequest()
    {
        // Arrange
        var request = new GetLanguagesCommand
        {
            PageNumber = 1,
            PageSize = 10
        };

        var languages = new List<MSProfessionals.Domain.Entities.Language>
        {
            new() { Id = Guid.NewGuid(), Code = "pt-BR", Description = "Português do Brasil" },
            new() { Id = Guid.NewGuid(), Code = "en-US", Description = "English" }
        };

        _languageRepositoryMock
            .Setup(x => x.CountAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(languages.Count);

        _languageRepositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<MSProfessionals.Domain.Entities.Language>)languages);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.PageNumber.Should().Be(request.PageNumber);
        result.PageSize.Should().Be(request.PageSize);
        result.TotalItems.Should().Be(languages.Count);
        result.Items.Should().HaveCount(languages.Count);
        result.Items.Should().BeInAscendingOrder(x => x.Description);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoData()
    {
        // Arrange
        var request = new GetLanguagesCommand
        {
            PageNumber = 1,
            PageSize = 10
        };

        _languageRepositoryMock
            .Setup(x => x.CountAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        _languageRepositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
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
        var request = new GetLanguagesCommand
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
        var request = new GetLanguagesCommand
        {
            PageNumber = 1,
            PageSize = pageSize
        };

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(request, CancellationToken.None));
    }
} 