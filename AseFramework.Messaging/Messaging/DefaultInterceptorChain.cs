using System.Collections;
using System.Collections.Generic;
using Ase.Messaging.Messaging.UnitOfWork;

namespace Ase.Messaging.Messaging
{
    public class DefaultInterceptorChain<TMessage, TPayload> : IInterceptorChain
        where TMessage : IMessage<TPayload> where TPayload : class
    {
        private readonly IMessageHandler<TMessage, TPayload> _handler;
        private readonly IEnumerator<IMessageHandlerInterceptor<TMessage, TPayload>> _chain;
        private readonly IUnitOfWork<TMessage, TPayload> _unitOfWork;

        public DefaultInterceptorChain(IUnitOfWork<TMessage, TPayload> unitOfWork,
            IEnumerator<IMessageHandlerInterceptor<TMessage, TPayload>> interceptors,
            IMessageHandler<TMessage, TPayload> handler)
        {
            _handler = handler;
            _chain = interceptors;
            _unitOfWork = unitOfWork;
        }

        public object Proceed()
        {
        }
    }
}