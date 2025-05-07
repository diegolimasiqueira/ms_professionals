using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using MSProfessionals.Application.Commands.ProfessionalAddress;
using MSProfessionals.Domain.Entities;
using MSProfessionals.Domain.Exceptions;
using MSProfessionals.Domain.Interfaces;

public class UpdateProfessionalAddressCommandHandlerTests
{
    private readonly Mock<IAddressRepository> _addressRepository = new();
    private readonly Mock<ICountryCodeRepository> _countryCodeRepository = new();
    private readonly UpdateProfessionalAddressCommandHandler _handler;

    public UpdateProfessionalAddressCommandHandlerTests()
    {
        _handler = new UpdateProfessionalAddressCommandHandler(
            _addressRepository.Object,
            _countryCodeRepository.Object);
    }

    [Fact]
    public async Task Handle_ShouldUpdateAddress_WhenDataIsValid()
    {
        var command = new UpdateProfessionalAddressCommand
        {
            Id = Guid.NewGuid(),
            StreetAddress = "Rua Teste",
            City = "Cidade",
            State = "Estado",
            PostalCode = "12345-678",
            CountryId = Guid.NewGuid(),
            IsDefault = false
        };
        var address = new ProfessionalAddress(Guid.NewGuid(), "Rua", "Cidade", "Estado", "12345-678", null, null, false, command.CountryId) { Id = command.Id };
        var country = new CountryCode(command.CountryId, "BR", "Brasil");
        _addressRepository.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(address);
        _countryCodeRepository.Setup(r => r.GetByIdAsync(command.CountryId, It.IsAny<CancellationToken>())).ReturnsAsync(country);
        _addressRepository.Setup(r => r.UpdateAsync(address)).Returns(Task.CompletedTask);
        var result = await _handler.Handle(command, CancellationToken.None);
        Assert.Equal(command.StreetAddress, result.StreetAddress);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenAddressNotFound()
    {
        var command = new UpdateProfessionalAddressCommand {
            Id = Guid.NewGuid(),
            CountryId = Guid.NewGuid(),
            StreetAddress = "Rua Teste",
            City = "Cidade",
            State = "Estado",
            PostalCode = "12345-678",
            IsDefault = false
        };
        _addressRepository.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync((ProfessionalAddress)null!);
        await Assert.ThrowsAsync<ProfessionalNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenCountryNotFound()
    {
        var command = new UpdateProfessionalAddressCommand {
            Id = Guid.NewGuid(),
            CountryId = Guid.NewGuid(),
            StreetAddress = "Rua Teste",
            City = "Cidade",
            State = "Estado",
            PostalCode = "12345-678",
            IsDefault = false
        };
        var address = new ProfessionalAddress(Guid.NewGuid(), "Rua", "Cidade", "Estado", "12345-678", null, null, false, command.CountryId) { Id = command.Id };
        _addressRepository.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(address);
        _countryCodeRepository.Setup(r => r.GetByIdAsync(command.CountryId, It.IsAny<CancellationToken>())).ReturnsAsync((CountryCode)null!);
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldUnsetExistingDefault_WhenIsDefaultTrue()
    {
        var command = new UpdateProfessionalAddressCommand
        {
            Id = Guid.NewGuid(),
            StreetAddress = "Rua Teste",
            City = "Cidade",
            State = "Estado",
            PostalCode = "12345-678",
            CountryId = Guid.NewGuid(),
            IsDefault = true
        };
        var address = new ProfessionalAddress(Guid.NewGuid(), "Rua", "Cidade", "Estado", "12345-678", null, null, false, command.CountryId) { Id = command.Id };
        var country = new CountryCode(command.CountryId, "BR", "Brasil");
        var existingDefault = new ProfessionalAddress(Guid.NewGuid(), "Rua", "Cidade", "Estado", "12345-678", null, null, true, command.CountryId);
        _addressRepository.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(address);
        _countryCodeRepository.Setup(r => r.GetByIdAsync(command.CountryId, It.IsAny<CancellationToken>())).ReturnsAsync(country);
        _addressRepository.Setup(r => r.GetDefaultByProfessionalIdAsync(address.ProfessionalId)).ReturnsAsync(existingDefault);
        _addressRepository.Setup(r => r.UpdateAsync(existingDefault)).Returns(Task.CompletedTask);
        _addressRepository.Setup(r => r.UpdateAsync(address)).Returns(Task.CompletedTask);
        var result = await _handler.Handle(command, CancellationToken.None);
        Assert.Equal(command.StreetAddress, result.StreetAddress);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenValidationFails()
    {
        var command = new UpdateProfessionalAddressCommand();
        await Assert.ThrowsAsync<System.ComponentModel.DataAnnotations.ValidationException>(() => _handler.Handle(command, CancellationToken.None));
    }
} 