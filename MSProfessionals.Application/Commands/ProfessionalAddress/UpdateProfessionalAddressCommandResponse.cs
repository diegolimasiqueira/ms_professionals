using System;

namespace MSProfessionals.Application.Commands.ProfessionalAddress;

/// <summary>
/// Response para o comando de atualização de endereço
/// </summary>
public class UpdateProfessionalAddressCommandResponse
{
    /// <summary>
    /// ID do endereço
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// ID do profissional
    /// </summary>
    public Guid ProfessionalId { get; set; }

    /// <summary>
    /// Endereço completo
    /// </summary>
    public string StreetAddress { get; set; } = string.Empty;

    /// <summary>
    /// Cidade
    /// </summary>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// Estado
    /// </summary>
    public string State { get; set; } = string.Empty;

    /// <summary>
    /// CEP
    /// </summary>
    public string PostalCode { get; set; } = string.Empty;

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
    public Guid CountryId { get; set; }

    /// <summary>
    /// Data de criação
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Data de atualização
    /// </summary>
    public DateTime UpdatedAt { get; set; }
} 