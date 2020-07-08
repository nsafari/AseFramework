using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Ase.Messaging.QueryHandling;

namespace Ase.Messaging.Messaging.ResponseTypes
{
    /// <summary>
    /// Implementation of a QueryResponseMessage that is aware of the requested response type and performs a just-in-time
    /// conversion to ensure the response is formatted as requested.
    /// <p>
    /// The conversion is generally used to accommodate response types that aren't compatible with serialization, such as
    /// {@link OptionalResponseType}.
    /// </summary>
    /// <typeparam name="R"></typeparam>
    public class ConvertingResponseMessage<R>: IQueryResponseMessage<R>
    {
        public string GetIdentifier()
        {
            throw new NotImplementedException();
        }

        public MetaData GetMetaData()
        {
            throw new NotImplementedException();
        }

        public R GetPayload()
        {
            throw new NotImplementedException();
        }

        public Type GetPayloadType()
        {
            throw new NotImplementedException();
        }

        public IMessage<R> WithMetaData(IReadOnlyDictionary<string, object> metaData)
        {
            throw new NotImplementedException();
        }

        public IMessage<R> AndMetaData(IReadOnlyDictionary<string, object> metaData)
        {
            throw new NotImplementedException();
        }

        public bool IsExceptional()
        {
            throw new NotImplementedException();
        }

        public Exception? OptionalExceptionResult()
        {
            throw new NotImplementedException();
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