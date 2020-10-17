using System;
using Ase.Messaging.Common;

namespace AseFramework.Modeling.Command
{
    /// <summary>
    /// Exception indicating that the an entity for an aggregate could not be found.
    /// </summary>
    public class AggregateEntityNotFoundException: AxonNonTransientException
    {
        /// <summary>
        /// Initialize a AggregateEntityNotFoundException with given {@code message}.
        /// </summary>
        /// <param name="message">The message describing the cause of the exception</param>
        public AggregateEntityNotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initialize a AggregateEntityNotFoundException with given {@code message} and {@code cause}.
        /// </summary>
        /// <param name="message">The message describing the cause of the exception</param>
        /// <param name="innerException">The underlying cause of the exception</param>
        public AggregateEntityNotFoundException(string message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}