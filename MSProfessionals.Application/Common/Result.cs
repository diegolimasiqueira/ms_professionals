namespace MSProfessionals.Application.Common;

/// <summary>
/// Represents the result of an operation
/// </summary>
/// <typeparam name="T">Type of the result value</typeparam>
public class Result<T>
{
    /// <summary>
    /// Gets a value indicating whether the operation was successful
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Gets the value of the result
    /// </summary>
    public T? Value { get; }

    /// <summary>
    /// Gets the error message
    /// </summary>
    public string? Error { get; }

    /// <summary>
    /// Initializes a new instance of the Result class
    /// </summary>
    /// <param name="isSuccess">Whether the operation was successful</param>
    /// <param name="value">The result value</param>
    /// <param name="error">The error message</param>
    protected Result(bool isSuccess, T? value, string? error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    /// <summary>
    /// Creates a successful result
    /// </summary>
    /// <param name="value">The result value</param>
    /// <returns>A successful result</returns>
    public static Result<T> Success(T value)
    {
        return new Result<T>(true, value, null);
    }

    /// <summary>
    /// Creates a failed result
    /// </summary>
    /// <param name="error">The error message</param>
    /// <returns>A failed result</returns>
    public static Result<T> Failure(string error)
    {
        return new Result<T>(false, default, error);
    }
} 