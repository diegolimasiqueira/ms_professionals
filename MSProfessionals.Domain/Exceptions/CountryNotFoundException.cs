namespace MSProfessionals.Domain.Exceptions
{
    /// <summary>
    /// Exception thrown when a country is not found
    /// </summary>
    public class CountryNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the CountryNotFoundException
        /// </summary>
        public CountryNotFoundException() : base("Country not found")
        {
        }

        /// <summary>
        /// Initializes a new instance of the CountryNotFoundException with a specific message
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        public CountryNotFoundException(string? message) : base(message ?? "Country not found")
        {
        }

        /// <summary>
        /// Initializes a new instance of the CountryNotFoundException with a specific message and inner exception
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        /// <param name="innerException">The exception that is the cause of the current exception</param>
        public CountryNotFoundException(string? message, Exception innerException) : base(message ?? "Country not found", innerException)
        {
        }
    }
} 