using System;
using System.Collections.ObjectModel;
using Ase.Messaging.Messaging;

namespace Ase.Messaging.CommandHandling
{
    /// <summary>
    /// Message that represents a result from handling a <see cref="CommandMessage"/>.
    /// </summary>
    /// <typeparam name="R">The type of payload contained in this Message</typeparam>
    public interface ICommandResultMessage<R> : IResultMessage<R>
        where R : class
    {
        ICommandResultMessage<R> WithMetaData(ReadOnlyDictionary<string, object> metaData);

        ICommandResultMessage<R> AndMetaData(ReadOnlyDictionary<string, object> metaData);
    }
}