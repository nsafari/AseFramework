using System.Collections.Immutable;
using Ase.Messaging.Messaging;

namespace Ase.Messaging.QueryHandling
{
    public interface ISubscriptionQueryUpdateMessage<out U>: IMessage<U> 
        where U : class
    {
        ISubscriptionQueryUpdateMessage<U> WithMetaData(IImmutableDictionary<string, object> metaData);

        ISubscriptionQueryUpdateMessage<U> AndMetaData(IImmutableDictionary<string, object> metaData);

        
    }
}