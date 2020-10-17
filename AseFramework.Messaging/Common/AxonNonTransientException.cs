using System;

namespace Ase.Messaging.Common
{
    /// <summary>
    /// Exception indicating an error has been cause that cannot be resolved without intervention. Retrying the operation
    /// that threw the exception will most likely result in the same exception being thrown.
    /// <p/>
    /// Examples of such errors are programming errors and version conflicts.
    /// </summary>
    public class AxonNonTransientException: AseException
    {
        
        /// <summary>
        /// Indicates whether the given {@code throwable} is a AxonNonTransientException exception or indicates to be
        /// caused by one.
        /// </summary>
        /// <param name="throwable">The throwable to inspect</param>
        /// <returns>{@code true} if the given instance or one of it's causes is an instance of
        /// AxonNonTransientException, otherwise {@code false}</returns>
        public static bool IsCauseOf(Exception? throwable) {
            return throwable != null
                   && (throwable is AxonNonTransientException || IsCauseOf(throwable.InnerException));
        }
        
        /// <summary>
        /// Initializes the exception using the given {@code message}.
        /// </summary>
        /// <param name="message">The message describing the exception</param>
        public AxonNonTransientException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes the exception using the given {@code message} and {@code cause}.
        /// </summary>
        /// <param name="message">The message describing the exception</param>
        /// <param name="innerException">The underlying cause of the exception</param>
        public AxonNonTransientException(string message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}