using System;

namespace Ase.Messaging.Common
{
    /// <summary>
    /// Base exception of all Axon Framework related exceptions.
    /// </summary>
    public abstract class AseException : SystemException
    {
        /// <summary>
        /// Initializes the exception using the given {@code message}.
        /// </summary>
        /// <param name="message">The message describing the exception</param>
        protected AseException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes the exception using the given {@code message} and {@code cause}.
        /// </summary>
        /// <param name="message">The message describing the exception</param>
        /// <param name="innerException">The underlying cause of the exception</param>
        protected AseException(string message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}