namespace MSProfessionals.Domain.Exceptions
{
    /// <summary>
    /// Exception thrown when a country is not found
    /// </summary>
    public class CountryNotFoundException : Exception
    {


        /// <summary>
        /// Initializes a new instance of the CountryNotFoundException with a specific message
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        public CountryNotFoundException(string? message) : base(message ?? "Country not found")
        {
        }

    }
} 