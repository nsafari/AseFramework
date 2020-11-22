using System;
using Ase.Messaging.Common;

namespace Ase.Messaging.Messaging.Annotation
{
    public class MessageHandlerInvocationException : AseException
    {
        /// <summary>
        /// Initialize the MessageHandlerInvocationException using given {@code message} and {@code cause}.
        /// </summary>
        /// <param name="message">A message describing the cause of the exception</param>
        /// <param name="cause">The exception thrown by the Event Handler</param>
        public MessageHandlerInvocationException(string message, Exception cause) : base(message, cause)
        {
        }
    }
}