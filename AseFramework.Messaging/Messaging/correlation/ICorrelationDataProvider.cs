using System.Collections.Generic;

namespace Ase.Messaging.Messaging.correlation
{
    /// <summary>
    /// Object defining the data from a Message that should be attached as correlation data to messages generated as
    /// result of the processing of that message.
    /// </summary>
    public interface ICorrelationDataProvider
    {
        delegate IDictionary<string, R> CorrelationDataFor<R>(IMessage<R> message)
            where R : class;
    }
}