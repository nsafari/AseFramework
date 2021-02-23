using System;
using Ase.Messaging.Common;

namespace Ase.Messaging.EventHandling
{
    /// <summary>
    /// Exception indicating that a reset is not supported by a component.
    /// </summary>
    public class ResetNotSupportedException: AseNonTransientException
    {
        public ResetNotSupportedException(string message) : base(message)
        {
        }

        public ResetNotSupportedException(string message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}