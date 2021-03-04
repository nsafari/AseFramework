using System;

namespace Ase.Messaging.EventHandling
{
    
    /// <summary>
    /// Interface for an event message handler that defers handling to one or more other handlers.
    /// </summary>
    /// <typeparam name="TPayload"></typeparam>
    public interface IEventHandlerInvoker<TPayload> 
        where TPayload : class
    {
        
        /// <summary>
        /// Check whether or not this invoker has handlers that can handle the given {@code eventMessage} for a given
        /// {@code segment}.
        /// </summary>
        /// <param name="eventMessage">The message to be processed</param>
        /// <param name="segment">The segment for which the event handler should be invoked</param>
        /// <returns>{@code true} if the invoker has one or more handlers that can handle the given message, {@code false}</returns>
        bool CanHandle(IEventMessage<TPayload> eventMessage, Segment segment);

        /// <summary>
        /// Check whether or not this invoker has handlers that can handle the given {@code payloadType}. 
        /// </summary>
        /// <param name="payloadType">The payloadType of the message to be processed</param>
        /// <returns>{@code true} if the invoker has one or more handlers that can handle the given message, {@code false} otherwise</returns>
        bool CanHandleType(Type payloadType) {
            return true;
        }

        /// <summary>
        /// Handle the given {@code message} for the given {@code segment}.
        /// <p>
        /// Callers are recommended to invoke {@link #canHandle(EventMessage, Segment)} prior to invocation, but aren't
        /// required to do so. Implementations must ensure to take the given segment into account when processing messages.
        /// </summary>
        /// <param name="message">The message to handle</param>
        /// <param name="segment">The segment for which to handle the message</param>
        void Handle(IEventMessage<TPayload> message, Segment segment);

        /// <summary>
        /// Indicates whether the handlers managed by this invoker support a reset.
        /// </summary>
        /// <returns>{@code true} if a reset is supported, otherwise {@code false}</returns>
        bool SupportsReset() {
            return true;
        }

        /// <summary>
        /// Performs any activities that are required to reset the state managed by handlers assigned to this invoker.
        /// </summary>
        void PerformReset() {
        }

    }
}