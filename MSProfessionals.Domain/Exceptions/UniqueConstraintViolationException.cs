namespace MSProfessionals.Domain.Exceptions;

public class UniqueConstraintViolationException : Exception
{
    public string FieldName { get; }
    public string FieldValue { get; }

    public UniqueConstraintViolationException(string fieldName, string fieldValue)
        : base($"The {fieldName} '{fieldValue}' is already in use.")
    {
        FieldName = fieldName;
        FieldValue = fieldValue;
    }
} 