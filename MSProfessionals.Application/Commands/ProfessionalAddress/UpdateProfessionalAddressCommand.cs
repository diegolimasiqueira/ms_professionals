using System;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace MSProfessionals.Application.Commands.ProfessionalAddress;

/// <summary>
/// Command para atualizar um endereço
/// </summary>
public class UpdateProfessionalAddressCommand : IRequest<UpdateProfessionalAddressCommandResponse>
{
    /// <summary>
    /// ID do endereço
    /// </summary>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// Endereço completo
    /// </summary>
    [Required]
    [MaxLength(255)]
    public required string StreetAddress { get; set; }

    /// <summary>
    /// Cidade
    /// </summary>
    [Required]
    [MaxLength(30)]
    public required string City { get; set; }

    /// <summary>
    /// Estado
    /// </summary>
    [Required]
    [MaxLength(50)]
    public required string State { get; set; }

    /// <summary>
    /// CEP
    /// </summary>
    [Required]
    [MaxLength(20)]
    public required string PostalCode { get; set; }

    /// <summary>
    /// Latitude
    /// </summary>
    public double? Latitude { get; set; }

    /// <summary>
    /// Longitude
    /// </summary>
    public double? Longitude { get; set; }

    /// <summary>
    /// Indica se é o endereço padrão
    /// </summary>
    public bool IsDefault { get; set; }

    /// <summary>
    /// ID do país
    /// </summary>
    [Required]
    public Guid CountryId { get; set; }
} 