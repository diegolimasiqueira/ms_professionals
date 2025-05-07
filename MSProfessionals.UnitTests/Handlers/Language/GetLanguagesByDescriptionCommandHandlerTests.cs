using FluentAssertions;
using MediatR;
using Moq;
using MSProfessionals.Application.Commands.Language;
using MSProfessionals.Domain.Entities;
using MSProfessionals.Domain.Interfaces;
using Xunit;

namespace MSProfessionals.UnitTests.Handlers.Language;

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
    public async Task Handle_ShouldReturnPaginatedLanguages()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;
        var totalItems = 25;
        var description = "English";
        var languages = new List<MSProfessionals.Domain.Entities.Language>
        {
            new() { Id = Guid.NewGuid(), Code = "en", Description = "English" },
            new() { Id = Guid.NewGuid(), Code = "en-US", Description = "English (US)" }
        };

        var command = new GetLanguagesByDescriptionCommand 
        { 
            PageNumber = pageNumber, 
            PageSize = pageSize,
            Description = description
        };

        _languageRepositoryMock.Setup(x => x.GetByDescriptionAsync(description, (pageNumber - 1) * pageSize, pageSize, It.IsAny<CancellationToken>()))
            .ReturnsAsync(languages);

        _languageRepositoryMock.Setup(x => x.CountByDescriptionAsync(description, It.IsAny<CancellationToken>()))
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

        var firstLanguage = result.Items.First();
        firstLanguage.Id.Should().Be(languages[0].Id);
        firstLanguage.Code.Should().Be(languages[0].Code);
        firstLanguage.Description.Should().Be(languages[0].Description);

        var secondLanguage = result.Items.Last();
        secondLanguage.Id.Should().Be(languages[1].Id);
        secondLanguage.Code.Should().Be(languages[1].Code);
        secondLanguage.Description.Should().Be(languages[1].Description);
    }

    [Fact]
    public async Task Handle_WhenNoLanguagesExist_ShouldReturnEmptyList()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;
        var description = "NonExistent";
        var command = new GetLanguagesByDescriptionCommand 
        { 
            PageNumber = pageNumber, 
            PageSize = pageSize,
            Description = description
        };

        _languageRepositoryMock.Setup(x => x.GetByDescriptionAsync(description, (pageNumber - 1) * pageSize, pageSize, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<MSProfessionals.Domain.Entities.Language>());

        _languageRepositoryMock.Setup(x => x.CountByDescriptionAsync(description, It.IsAny<CancellationToken>()))
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
        var command = new GetLanguagesByDescriptionCommand 
        { 
            PageNumber = 0, 
            PageSize = 10,
            Description = "English"
        };

        // Act & Assert
        await Assert.ThrowsAsync<System.ComponentModel.DataAnnotations.ValidationException>(() => 
            _handler.Handle(command, CancellationToken.None));
        _languageRepositoryMock.Verify(x => x.GetByDescriptionAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        _languageRepositoryMock.Verify(x => x.CountByDescriptionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenPageSizeIsLessThanOne_ShouldThrowException()
    {
        // Arrange
        var command = new GetLanguagesByDescriptionCommand 
        { 
            PageNumber = 1, 
            PageSize = 0,
            Description = "English"
        };

        // Act & Assert
        await Assert.ThrowsAsync<System.ComponentModel.DataAnnotations.ValidationException>(() => 
            _handler.Handle(command, CancellationToken.None));
        _languageRepositoryMock.Verify(x => x.GetByDescriptionAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        _languageRepositoryMock.Verify(x => x.CountByDescriptionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenDescriptionIsEmpty_ShouldThrowException()
    {
        // Arrange
        var command = new GetLanguagesByDescriptionCommand 
        { 
            PageNumber = 1, 
            PageSize = 10,
            Description = string.Empty
        };

        // Act & Assert
        await Assert.ThrowsAsync<System.ComponentModel.DataAnnotations.ValidationException>(() => 
            _handler.Handle(command, CancellationToken.None));
        _languageRepositoryMock.Verify(x => x.GetByDescriptionAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        _languageRepositoryMock.Verify(x => x.CountByDescriptionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }
} 