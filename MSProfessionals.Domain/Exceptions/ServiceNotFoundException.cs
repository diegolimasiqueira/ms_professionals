namespace MSProfessionals.Domain.Exceptions;

/// <summary>
/// Exception thrown when a service is not found
/// </summary>
public class ServiceNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceNotFoundException"/> class.
    /// </summary>
    /// <param name="message">Error message</param>
    public ServiceNotFoundException(string message) : base(message)
    {
    }
} 