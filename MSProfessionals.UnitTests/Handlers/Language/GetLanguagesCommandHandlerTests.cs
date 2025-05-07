using FluentAssertions;
using MediatR;
using Moq;
using MSProfessionals.Application.Commands.Language;
using MSProfessionals.Domain.Entities;
using MSProfessionals.Domain.Interfaces;
using Xunit;

namespace MSProfessionals.UnitTests.Handlers.Language;

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
    public async Task Handle_ShouldReturnPaginatedLanguages()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;
        var totalItems = 25;
        var languages = new List<MSProfessionals.Domain.Entities.Language>
        {
            new() { Id = Guid.NewGuid(), Code = "en", Description = "English" },
            new() { Id = Guid.NewGuid(), Code = "pt", Description = "Portuguese" }
        };

        var command = new GetLanguagesCommand { PageNumber = pageNumber, PageSize = pageSize };

        _languageRepositoryMock.Setup(x => x.GetAllAsync((pageNumber - 1) * pageSize, pageSize, It.IsAny<CancellationToken>()))
            .ReturnsAsync(languages);

        _languageRepositoryMock.Setup(x => x.CountAsync(It.IsAny<CancellationToken>()))
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
        var command = new GetLanguagesCommand { PageNumber = pageNumber, PageSize = pageSize };

        _languageRepositoryMock.Setup(x => x.GetAllAsync((pageNumber - 1) * pageSize, pageSize, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<MSProfessionals.Domain.Entities.Language>());

        _languageRepositoryMock.Setup(x => x.CountAsync(It.IsAny<CancellationToken>()))
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
        var command = new GetLanguagesCommand { PageNumber = 0, PageSize = 10 };

        // Act & Assert
        await Assert.ThrowsAsync<System.ComponentModel.DataAnnotations.ValidationException>(() => 
            _handler.Handle(command, CancellationToken.None));
        _languageRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        _languageRepositoryMock.Verify(x => x.CountAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenPageSizeIsLessThanOne_ShouldThrowException()
    {
        // Arrange
        var command = new GetLanguagesCommand { PageNumber = 1, PageSize = 0 };

        // Act & Assert
        await Assert.ThrowsAsync<System.ComponentModel.DataAnnotations.ValidationException>(() => 
            _handler.Handle(command, CancellationToken.None));
        _languageRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        _languageRepositoryMock.Verify(x => x.CountAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
} 