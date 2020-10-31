using Ase.Messaging.Messaging.UnitOfWork;

namespace Ase.Messaging.Messaging
{
    /// <summary>
    /// Workflow interface that allows for customized message handler invocation chains. A MessageHandlerInterceptor can add
    /// customized behavior to message handler invocations, both before and after the invocation.
    /// </summary>
    /// <typeparam name="TMessage">The message type this interceptor can process</typeparam>
    /// <typeparam name="TPayload">The payload type of the message</typeparam>
    public interface IMessageHandlerInterceptor<in TMessage, TPayload>
        where TMessage : IMessage<TPayload> where TPayload : class
    {
        /// <summary>
        /// Invoked before a Message is handled by a designated {@link org.axonframework.messaging.MessageHandler}.
        /// <p/>
        /// The interceptor is responsible for the continuation of the handling process by invoking the {@link
        /// InterceptorChain#proceed()} method on the given {@code interceptorChain}.
        /// <p/>
        /// The given {@code unitOfWork} contains contextual information. Any information gathered by interceptors
        /// may be attached to the unitOfWork.
        /// <p/>
        /// Interceptors are highly recommended not to change the type of the message handling result, as the dispatching
        /// component might expect a result of a specific type.
        /// </summary>
        /// <param name="unitOfWork">The UnitOfWork that is processing the message</param>
        /// <param name="interceptorChain">The interceptor chain that allows this interceptor to proceed the dispatch process</param>
        /// <returns>the result of the message handler. May have been modified by interceptors.</returns>
        object Handle(IUnitOfWork<TMessage, TPayload> unitOfWork, IInterceptorChain interceptorChain);
    }
}