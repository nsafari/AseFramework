using System.Collections.ObjectModel;
using Ase.Messaging.Messaging;

namespace Ase.Messaging.CommandHandling
{
    /// <summary>
    /// Message that represents a result from handling a <see cref="CommandMessage"/>.
    /// </summary>
    /// <typeparam name="R">The type of payload contained in this Message</typeparam>
    public interface ICommandResultMessage<R>: IResultMessage<R>
    {
        
        ICommandResultMessage<R> WithMetaData<T>(ReadOnlyDictionary<string, T> metaData);
        
        ICommandResultMessage<R> AndMetaData<T>(ReadOnlyDictionary<string, T> metaData);

        
    }
}