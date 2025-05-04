using System;

namespace MSProfessionals.Domain.Exceptions;

/// <summary>
/// Exception thrown when a professional tries to add more than 3 professions
/// </summary>
public class ProfessionalProfessionLimitExceededException : Exception
{
    /// <summary>
    /// Initializes a new instance of the ProfessionalProfessionLimitExceededException
    /// </summary>
    /// <param name="message">Exception message</param>
    public ProfessionalProfessionLimitExceededException(string message) : base(message)
    {
    }
} 