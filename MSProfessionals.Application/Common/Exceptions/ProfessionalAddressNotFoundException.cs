using System;

namespace MSProfessionals.Application.Common.Exceptions;

/// <summary>
/// Exception thrown when a professional address is not found
/// </summary>
public class ProfessionalAddressNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the ProfessionalAddressNotFoundException
    /// </summary>
    /// <param name="id">Professional address ID</param>
    public ProfessionalAddressNotFoundException(Guid id)
        : base($"Professional address with ID {id} was not found.")
    {
    }
} 