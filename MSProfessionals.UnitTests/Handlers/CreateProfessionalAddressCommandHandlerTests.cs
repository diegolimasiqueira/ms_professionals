using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using MSProfessionals.Application.Commands.ProfessionalAddress;
using MSProfessionals.Domain.Entities;
using MSProfessionals.Domain.Exceptions;
using MSProfessionals.Domain.Interfaces;

public class CreateProfessionalAddressCommandHandlerTests
{
    private readonly Mock<IAddressRepository> _addressRepository = new();
    private readonly Mock<IProfessionalRepository> _professionalRepository = new();
    private readonly Mock<ICountryCodeRepository> _countryCodeRepository = new();
    private readonly CreateProfessionalAddressCommandHandler _handler;

    public CreateProfessionalAddressCommandHandlerTests()
    {
        _handler = new CreateProfessionalAddressCommandHandler(
            _addressRepository.Object,
            _professionalRepository.Object,
            _countryCodeRepository.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateAddress_WhenDataIsValid()
    {
        var command = new CreateProfessionalAddressCommand
        {
            ProfessionalId = Guid.NewGuid(),
            StreetAddress = "Rua Teste",
            City = "Cidade",
            State = "Estado",
            PostalCode = "12345-678",
            CountryId = Guid.NewGuid(),
            IsDefault = false
        };
        var professional = new Professional { Id = command.ProfessionalId, Name = "Nome", DocumentId = "Doc", Email = "Email", PhoneNumber = "Phone", CurrencyId = Guid.NewGuid(), PhoneCountryCodeId = Guid.NewGuid(), PreferredLanguageId = Guid.NewGuid(), TimezoneId = Guid.NewGuid() };
        var country = new CountryCode(command.CountryId, "BR", "Brasil");
        _professionalRepository.Setup(r => r.GetByIdWithoutRelationsAsync(command.ProfessionalId, CancellationToken.None)).ReturnsAsync(professional);
        _countryCodeRepository.Setup(r => r.GetByIdAsync(command.CountryId, It.IsAny<CancellationToken>())).ReturnsAsync(country);
        _addressRepository.Setup(r => r.AddAsync(It.IsAny<ProfessionalAddress>())).Returns(Task.CompletedTask);
        var result = await _handler.Handle(command, CancellationToken.None);
        Assert.Equal(command.StreetAddress, result.StreetAddress);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenProfessionalNotFound()
    {
        var command = new CreateProfessionalAddressCommand {
            ProfessionalId = Guid.NewGuid(),
            CountryId = Guid.NewGuid(),
            StreetAddress = "Rua Teste",
            City = "Cidade",
            State = "Estado",
            PostalCode = "12345-678",
            IsDefault = false
        };
        _professionalRepository.Setup(r => r.GetByIdWithoutRelationsAsync(command.ProfessionalId, CancellationToken.None)).ReturnsAsync((Professional)null!);
        await Assert.ThrowsAsync<ProfessionalNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenCountryNotFound()
    {
        var command = new CreateProfessionalAddressCommand {
            ProfessionalId = Guid.NewGuid(),
            CountryId = Guid.NewGuid(),
            StreetAddress = "Rua Teste",
            City = "Cidade",
            State = "Estado",
            PostalCode = "12345-678",
            IsDefault = false
        };
        var professional = new Professional { Id = command.ProfessionalId, Name = "Nome", DocumentId = "Doc", Email = "Email", PhoneNumber = "Phone", CurrencyId = Guid.NewGuid(), PhoneCountryCodeId = Guid.NewGuid(), PreferredLanguageId = Guid.NewGuid(), TimezoneId = Guid.NewGuid() };
        _professionalRepository.Setup(r => r.GetByIdWithoutRelationsAsync(command.ProfessionalId, CancellationToken.None)).ReturnsAsync(professional);
        _countryCodeRepository.Setup(r => r.GetByIdAsync(command.CountryId, It.IsAny<CancellationToken>())).ReturnsAsync((CountryCode)null!);
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldUnsetExistingDefault_WhenIsDefaultTrue()
    {
        var command = new CreateProfessionalAddressCommand
        {
            ProfessionalId = Guid.NewGuid(),
            StreetAddress = "Rua Teste",
            City = "Cidade",
            State = "Estado",
            PostalCode = "12345-678",
            CountryId = Guid.NewGuid(),
            IsDefault = true
        };
        var professional = new Professional { Id = command.ProfessionalId, Name = "Nome", DocumentId = "Doc", Email = "Email", PhoneNumber = "Phone", CurrencyId = Guid.NewGuid(), PhoneCountryCodeId = Guid.NewGuid(), PreferredLanguageId = Guid.NewGuid(), TimezoneId = Guid.NewGuid() };
        var country = new CountryCode(command.CountryId, "BR", "Brasil");
        var existingDefault = new ProfessionalAddress(command.ProfessionalId, "Rua", "Cidade", "Estado", "12345-678", null, null, true, command.CountryId);
        _professionalRepository.Setup(r => r.GetByIdWithoutRelationsAsync(command.ProfessionalId, CancellationToken.None)).ReturnsAsync(professional);
        _countryCodeRepository.Setup(r => r.GetByIdAsync(command.CountryId, It.IsAny<CancellationToken>())).ReturnsAsync(country);
        _addressRepository.Setup(r => r.GetDefaultByProfessionalIdAsync(command.ProfessionalId)).ReturnsAsync(existingDefault);
        _addressRepository.Setup(r => r.UpdateAsync(existingDefault)).Returns(Task.CompletedTask);
        _addressRepository.Setup(r => r.AddAsync(It.IsAny<ProfessionalAddress>())).Returns(Task.CompletedTask);
        var result = await _handler.Handle(command, CancellationToken.None);
        Assert.Equal(command.StreetAddress, result.StreetAddress);
    }
} 