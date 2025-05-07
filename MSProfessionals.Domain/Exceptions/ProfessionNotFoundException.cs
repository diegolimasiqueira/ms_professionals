namespace MSProfessionals.Domain.Exceptions
{
    /// <summary>
    /// Exception thrown when a profession is not found
    /// </summary>
    public class ProfessionNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the ProfessionNotFoundException with a specific message
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        public ProfessionNotFoundException(string message) : base(message)
        {
        }
    }
} 