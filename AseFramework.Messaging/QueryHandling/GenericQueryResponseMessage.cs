using System;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using Ase.Messaging.Messaging;

namespace Ase.Messaging.QueryHandling
{
    /// <summary>
    /// QueryResponseMessage implementation that takes all properties as constructor parameters.
    /// </summary>
    /// <typeparam name="R">The type of return value contained in this response</typeparam>
    public class GenericQueryResponseMessage<R> : GenericResultMessage<R>, IQueryResponseMessage<R>
        where R : class
    {
        public GenericQueryResponseMessage(R commandResult) : base(commandResult)
        {
        }

        public GenericQueryResponseMessage(Exception exception) : base(exception)
        {
        }

        public GenericQueryResponseMessage(R commandResult, IImmutableDictionary<string, object> metaData) : base(
            commandResult, metaData)
        {
        }

        public GenericQueryResponseMessage(Exception exception, IImmutableDictionary<string, object> metaData) : base(
            exception, metaData)
        {
        }

        public GenericQueryResponseMessage(IMessage<R> @delegate) : base(@delegate)
        {
        }

        public GenericQueryResponseMessage(IMessage<R> @delegate, Exception exception) : base(@delegate, exception)
        {
        }

        public IQueryResponseMessage<R> WithMetaData(ReadOnlyDictionary<string, object> metaData)
        {
            throw new NotImplementedException();
        }

        public IQueryResponseMessage<R> AndMetaData(ReadOnlyDictionary<string, object> additionalMetaData)
        {
            throw new NotImplementedException();
        }
    }
}