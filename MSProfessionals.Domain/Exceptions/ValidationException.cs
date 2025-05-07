namespace MSProfessionals.Domain.Exceptions;

/// <summary>
/// Exception thrown when validation fails
/// </summary>
public class ValidationException : Exception
{
    /// <summary>
    /// Initializes a new instance of the ValidationException
    /// </summary>
    public ValidationException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the ValidationException
    /// </summary>
    /// <param name="message">Exception message</param>
    public ValidationException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the ValidationException
    /// </summary>
    /// <param name="message">Exception message</param>
    /// <param name="innerException">Inner exception</param>
    public ValidationException(string message, Exception innerException) : base(message, innerException)
    {
    }
} 