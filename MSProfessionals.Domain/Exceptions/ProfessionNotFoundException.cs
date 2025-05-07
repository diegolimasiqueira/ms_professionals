namespace MSProfessionals.Domain.Exceptions
{
    /// <summary>
    /// Exception thrown when a profession is not found
    /// </summary>
    public class ProfessionNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the ProfessionNotFoundException
        /// </summary>
        public ProfessionNotFoundException() : base("Profession not found")
        {
        }

        /// <summary>
        /// Initializes a new instance of the ProfessionNotFoundException with a specific message
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        public ProfessionNotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ProfessionNotFoundException with a specific message and inner exception
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        /// <param name="innerException">The exception that is the cause of the current exception</param>
        public ProfessionNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
} 