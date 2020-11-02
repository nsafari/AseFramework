using System.Collections;
using System.Collections.Generic;
using Ase.Messaging.CommandHandling;
using Ase.Messaging.Messaging.UnitOfWork;

namespace Ase.Messaging.Messaging
{
    /// <summary>
    /// Mechanism that takes care of interceptor and handler execution.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    /// <typeparam name="TPayload"></typeparam>
    public class DefaultInterceptorChain<TMessage, TPayload> : IInterceptorChain
        where TMessage : IMessage<TPayload> where TPayload : class
    {
        private readonly IMessageHandler<TMessage, TPayload> _handler;
        private readonly IEnumerable<IMessageHandlerInterceptor<TMessage, TPayload>> _chain;
        private readonly IUnitOfWork<TMessage, TPayload> _unitOfWork;

        /// <summary>
        /// Initialize the default interceptor chain to dispatch the given {@code message}, through the
        /// {@code chain}, to the {@code handler}.
        /// </summary>
        /// <param name="unitOfWork">The UnitOfWork the message is executed in</param>
        /// <param name="interceptors">The interceptors composing the chain</param>
        /// <param name="handler">The handler for the message</param>
        public DefaultInterceptorChain(IUnitOfWork<TMessage, TPayload> unitOfWork,
            IEnumerable<IMessageHandlerInterceptor<TMessage, TPayload>> interceptors,
            IMessageHandler<TMessage, TPayload> handler)
        {
            _handler = handler;
            _chain = interceptors;
            _unitOfWork = unitOfWork;
        }

        
        public object Proceed()
        {
            using var enumerator = _chain.GetEnumerator();
            return enumerator.MoveNext() ? 
                enumerator.Current.Handle(_unitOfWork, this) : _handler.Handle(_unitOfWork.GetMessage());
        }
    }
}