namespace MSProfessionals.Domain.Exceptions;

/// <summary>
/// Exception thrown when a professional tries to add more than 10 services
/// </summary>
public class ProfessionalServiceLimitExceededException : Exception
{
    /// <summary>
    /// Initializes a new instance of the ProfessionalServiceLimitExceededException
    /// </summary>
    /// <param name="message">Exception message</param>
    public ProfessionalServiceLimitExceededException(string message) : base(message)
    {
    }
} 