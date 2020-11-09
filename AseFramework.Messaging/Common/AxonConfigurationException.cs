using System;

namespace Ase.Messaging.Common
{
    /// <summary>
    /// Exception indicating that a configuration error has been made in the Axon configuration. This problem prevents the
    /// application from operating properly.
    /// </summary>
    public class AxonConfigurationException: AxonNonTransientException
    {
        /// <summary>
        /// Initializes the exception using the given {@code message}.
        /// </summary>
        /// <param name="message">The message describing the exception</param>
        public AxonConfigurationException(string message): base(message) {
        }
        
        /// <summary>
        /// Initializes the exception using the given {@code message} and {@code cause}.
        /// </summary>
        /// <param name="message">The message describing the exception</param>
        /// <param name="cause">The underlying cause of the exception</param>
        public AxonConfigurationException(string message, Exception? cause): base(message, cause) {
            
        }

    }
}