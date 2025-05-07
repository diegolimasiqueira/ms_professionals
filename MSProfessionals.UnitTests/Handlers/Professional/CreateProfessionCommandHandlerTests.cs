using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using MSProfessionals.Application.Commands.Professional;
using MSProfessionals.Domain.Entities;
using MSProfessionals.Domain.Exceptions;
using MSProfessionals.Domain.Interfaces;
using Npgsql;
using Xunit;
using Microsoft.EntityFrameworkCore;

public class CreateProfessionCommandHandlerTests
{
    private readonly Mock<IProfessionalRepository> _professionalRepositoryMock = new();
    private readonly Mock<IProfessionRepository> _professionRepositoryMock = new();
    private readonly Mock<IProfessionalProfessionRepository> _professionalProfessionRepositoryMock = new();
    private readonly CreateProfessionCommandHandler _handler;

    public CreateProfessionCommandHandlerTests()
    {
        _handler = new CreateProfessionCommandHandler(
            _professionalRepositoryMock.Object,
            _professionRepositoryMock.Object,
            _professionalProfessionRepositoryMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldCreateProfession_WhenValidRequest()
    {
        // Arrange
        var professionalId = Guid.NewGuid();
        var professionId = Guid.NewGuid();
        var command = new CreateProfessionCommand { ProfessionalId = professionalId, ProfessionId = professionId };
        _professionalRepositoryMock.Setup(r => r.GetByIdAsync(professionalId, It.IsAny<CancellationToken>())).ReturnsAsync(new Professional());
        _professionRepositoryMock.Setup(r => r.GetByIdAsync(professionId, It.IsAny<CancellationToken>())).ReturnsAsync(new Profession());
        _professionalProfessionRepositoryMock.Setup(r => r.GetByProfessionalIdAsync(professionalId, It.IsAny<CancellationToken>())).ReturnsAsync(new List<ProfessionalProfession>());
        _professionalProfessionRepositoryMock.Setup(r => r.AddAsync(It.IsAny<ProfessionalProfession>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _professionalProfessionRepositoryMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(professionalId, result.ProfessionalId);
        Assert.Equal(professionId, result.ProfessionId);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenProfessionalNotFound()
    {
        // Arrange
        var professionalId = Guid.NewGuid();
        var professionId = Guid.NewGuid();
        var command = new CreateProfessionCommand { ProfessionalId = professionalId, ProfessionId = professionId };
        _professionalRepositoryMock.Setup(r => r.GetByIdAsync(professionalId, It.IsAny<CancellationToken>())).ReturnsAsync((Professional)null!);

        // Act & Assert
        await Assert.ThrowsAsync<ProfessionalNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenProfessionNotFound()
    {
        // Arrange
        var professionalId = Guid.NewGuid();
        var professionId = Guid.NewGuid();
        var command = new CreateProfessionCommand { ProfessionalId = professionalId, ProfessionId = professionId };
        _professionalRepositoryMock.Setup(r => r.GetByIdAsync(professionalId, It.IsAny<CancellationToken>())).ReturnsAsync(new Professional());
        _professionRepositoryMock.Setup(r => r.GetByIdAsync(professionId, It.IsAny<CancellationToken>())).ReturnsAsync((Profession)null!);

        // Act & Assert
        await Assert.ThrowsAsync<ProfessionNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenProfessionalHasThreeProfessions()
    {
        // Arrange
        var professionalId = Guid.NewGuid();
        var professionId = Guid.NewGuid();
        var command = new CreateProfessionCommand { ProfessionalId = professionalId, ProfessionId = professionId };
        _professionalRepositoryMock.Setup(r => r.GetByIdAsync(professionalId, CancellationToken.None)).ReturnsAsync(new Professional());
        _professionRepositoryMock.Setup(r => r.GetByIdAsync(professionId, CancellationToken.None)).ReturnsAsync(new Profession());
        _professionalProfessionRepositoryMock.Setup(r => r.GetByProfessionalIdAsync(professionalId, CancellationToken.None)).ReturnsAsync(new List<ProfessionalProfession> { new(professionalId, Guid.NewGuid()), new(professionalId, Guid.NewGuid()), new(professionalId, Guid.NewGuid()) });

        // Act & Assert
        await Assert.ThrowsAsync<ProfessionalProfessionLimitExceededException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenUniqueConstraintViolation()
    {
        // Arrange
        var professionalId = Guid.NewGuid();
        var professionId = Guid.NewGuid();
        var command = new CreateProfessionCommand { ProfessionalId = professionalId, ProfessionId = professionId };
        _professionalRepositoryMock.Setup(r => r.GetByIdAsync(professionalId, It.IsAny<CancellationToken>())).ReturnsAsync(new Professional());
        _professionRepositoryMock.Setup(r => r.GetByIdAsync(professionId, It.IsAny<CancellationToken>())).ReturnsAsync(new Profession());
        _professionalProfessionRepositoryMock.Setup(r => r.GetByProfessionalIdAsync(professionalId, It.IsAny<CancellationToken>())).ReturnsAsync(new List<ProfessionalProfession>());
        _professionalProfessionRepositoryMock.Setup(r => r.AddAsync(It.IsAny<ProfessionalProfession>(), It.IsAny<CancellationToken>())).ThrowsAsync(new DbUpdateException("", new PostgresException("", "", "", "23505")));

        // Act & Assert
        await Assert.ThrowsAsync<UniqueConstraintViolationException>(() => _handler.Handle(command, CancellationToken.None));
    }
} 