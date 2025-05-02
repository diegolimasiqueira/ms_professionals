namespace MSProfessionals.Domain.Exceptions;

public class ProfessionalNotFoundException : Exception
{
    public ProfessionalNotFoundException(string message) : base(message)
    {
    }
} 