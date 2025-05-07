namespace MSProfessionals.Domain.Exceptions;

/// <summary>
/// Exception thrown when a professional service is not found
/// </summary>
public class ProfessionalServiceNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProfessionalServiceNotFoundException"/> class.
    /// </summary>
    /// <param name="message">Error message</param>
    public ProfessionalServiceNotFoundException(string message) : base(message)
    {
    }
} 