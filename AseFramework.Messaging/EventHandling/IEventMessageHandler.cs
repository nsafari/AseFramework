using Ase.Messaging.Messaging;

namespace Ase.Messaging.EventHandling
{
    /// <summary>
    /// Interface to be implemented by classes that can handle events.
    ///
    /// @see EventBus
    /// @see DomainEventMessage
    /// @see EventHandler
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    /// <typeparam name="TPayload"></typeparam>
    public interface IEventMessageHandler<in TMessage, TPayload>: IMessageHandler<TMessage, IEventMessage<TPayload>> 
        where TMessage : IMessage<IEventMessage<TPayload>> where TPayload : class
    {
        
        /// <summary>
        /// Process the given event. The implementation may decide to process or skip the given event. It is highly
        /// unrecommended to throw any exception during the event handling process. 
        /// </summary>
        /// <param name="event">the event to handle</param>
        /// <returns>the result of the event handler invocation. Is generally ignored</returns>
        object Handle(IEventMessage<TPayload> @event);

        /// <summary>
        /// Performs any activities that are required to reset the state managed by handlers assigned to this invoker.
        /// </summary>
        void PrepareReset()
        {
            
        }

        /// <summary>
        /// Indicates whether the handlers managed by this invoker support a reset.
        /// </summary>
        /// <returns>{@code true} if a reset is supported, otherwise {@code false}</returns>
        bool SupportsReset()
        {
            return true;
        }


    }
}