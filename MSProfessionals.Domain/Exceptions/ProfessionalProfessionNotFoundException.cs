namespace MSProfessionals.Domain.Exceptions;

/// <summary>
/// Exception thrown when a professional profession is not found
/// </summary>
public class ProfessionalProfessionNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProfessionalProfessionNotFoundException"/> class.
    /// </summary>
    /// <param name="message">Error message</param>
    public ProfessionalProfessionNotFoundException(string message) : base(message)
    {
    }
} 