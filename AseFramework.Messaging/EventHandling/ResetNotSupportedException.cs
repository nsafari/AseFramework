using System;
using Ase.Messaging.Common;

namespace Ase.Messaging.EventHandling
{
    /// <summary>
    /// Exception indicating that a reset is not supported by a component.
    /// </summary>
    public class ResetNotSupportedException: AseNonTransientException
    {
        /// <summary>
        /// saveBatchMeLoConsumptionDataTest
        /// </summary>
        /// <param name="message">a message describing the cause of the exception</param>
        public ResetNotSupportedException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initialize the exception with given {@code message} and {@code cause}.
        /// </summary>
        /// <param name="message">a message describing the cause of the exception</param>
        /// <param name="innerException">the cause of this exception</param>
        public ResetNotSupportedException(string message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}