namespace MSProfessionals.Domain.Exceptions;

/// <summary>
/// Exception thrown when a professional is not found
/// </summary>
public class ProfessionalNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the ProfessionalNotFoundException
    /// </summary>
    /// <param name="message">Error message</param>
    public ProfessionalNotFoundException(string message) : base(message)
    {
    }
} 