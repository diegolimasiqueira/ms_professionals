namespace MSProfessionals.Domain.Exceptions;

/// <summary>
/// Exception thrown when attempting to add a duplicate service to a professional profession
/// </summary>
public class DuplicateServiceException : Exception
{
    /// <summary>
    /// Initializes a new instance of the DuplicateServiceException
    /// </summary>
    public DuplicateServiceException() : base("Service is already associated with this professional profession")
    {
    }

    /// <summary>
    /// Initializes a new instance of the DuplicateServiceException with a specific message
    /// </summary>
    /// <param name="message">The exception message</param>
    public DuplicateServiceException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the DuplicateServiceException with a specific message and inner exception
    /// </summary>
    /// <param name="message">The exception message</param>
    /// <param name="innerException">The inner exception</param>
    public DuplicateServiceException(string message, Exception innerException) : base(message, innerException)
    {
    }
} 