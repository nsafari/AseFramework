using Ase.Messaging.Messaging.UnitOfWork;

namespace Ase.Messaging.Messaging
{
    public interface IMessageHandlerInterceptor<T, R>
        where T : IMessage<R> where R: class
    {
        
        object Handle(IUnitOfWork<T, R> unitOfWork, IInterceptorChain interceptorChain);
    }
}