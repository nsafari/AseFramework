using System;

namespace Ase.Messaging.Messaging
{
    /// <summary>
    /// Interface for a component that processes Messages.
    /// </summary>
    /// <typeparam name="TMessage">The message type this handler can process</typeparam>
    /// <typeparam name="TPayload">The payload type of the message</typeparam>
    public interface IMessageHandler<in TMessage, TPayload>
        where TMessage : IMessage<TPayload> where TPayload : class
    {
        /// <summary>
        /// Handles the given {@code message}.
        /// </summary>
        /// <param name="message">The message to be processed.</param>
        /// <returns>The result of the message processing</returns>
        object? Handle(TMessage message);

        /// <summary>
        /// Indicates whether this handler can handle the given message
        /// </summary>
        /// <param name="message">The message to verify</param>
        /// <returns>{@code true} if this handler can handle the message, otherwise {@code false}</returns>
        bool CanHandle(TMessage message)
        {
            return true;
        }
        
        /// <summary>
        /// Returns the instance type that this handler delegates to.
        /// </summary>
        /// <returns>Returns the instance type that this handler delegates to</returns>
        Type GetTargetType() {
            return GetType();
        }
        
        /// <summary>
        /// Indicates whether this handler can handle messages of given type
        /// </summary>
        /// <param name="payloadType">The payloadType to verify</param>
        /// <returns>{@code true} if this handler can handle the payloadType, otherwise {@code false}</returns>
        bool CanHandleType(Type payloadType) {
            return true;
        }
    }
    
    public class MessageHandler<TMessage, TPayload>: IMessageHandler<TMessage, TPayload> 
        where TMessage : IMessage<TPayload> where TPayload : class
    {
        private readonly Func<TMessage, object> _handler;


        public MessageHandler(Func<TMessage, object> handler)
        {
            _handler = handler;
        }

        public object? Handle(TMessage message)
        {
            return _handler(message);
        }
    }
}