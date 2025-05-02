using System;

namespace MSProfessionals.Domain.Exceptions;

/// <summary>
/// Exception thrown when a resource is not found
/// </summary>
public class NotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the NotFoundException
    /// </summary>
    /// <param name="message">Error message</param>
    public NotFoundException(string message) : base(message)
    {
    }
} 