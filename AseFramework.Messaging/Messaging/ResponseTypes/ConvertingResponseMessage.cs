using System;
using System.Collections.Generic;
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
    public class ConvertingResponseMessage<R, T> : IQueryResponseMessage<R>
        where R : class
        where T : class

    {
        private readonly IResponseType<R> _expectedResponseType;
        private readonly IQueryResponseMessage<T> _responseMessage;

        /// <summary>
        /// Initialize a response message, using {@code expectedResponseType} to convert the payload from the {@code
        /// responseMessage}, if necessary.
        /// </summary>
        /// <param name="expectedResponseType">an instance describing the expected response type</param>
        /// <param name="responseMessage">the message containing the actual response from the handler</param>
        public ConvertingResponseMessage(IResponseType<R> expectedResponseType,
            IQueryResponseMessage<T> responseMessage)
        {
            _expectedResponseType = expectedResponseType;
            _responseMessage = responseMessage;
        }


        // @Override
        // public <S> SerializedObject<S> serializePayload(Serializer serializer, Class<S> expectedRepresentation) {
        //     return responseMessage.serializePayload(serializer, expectedRepresentation);
        // }

        // @Override
        // public <T> SerializedObject<T> serializeExceptionResult(Serializer serializer, Class<T> expectedRepresentation) {
        //     return responseMessage.serializeExceptionResult(serializer, expectedRepresentation);
        // }

        // @Override
        // public <R1> SerializedObject<R1> serializeMetaData(Serializer serializer, Class<R1> expectedRepresentation) {
        //     return responseMessage.serializeMetaData(serializer, expectedRepresentation);
        // }

        public string GetIdentifier()
        {
            return _responseMessage.GetIdentifier();
        }

        public MetaData GetMetaData()
        {
            return _responseMessage.GetMetaData();
        }

        public R? GetPayload()
        {
            return _expectedResponseType.Convert(_responseMessage.GetPayload());
        }

        public Type GetPayloadType()
        {
            return _expectedResponseType.ResponseMessagePayloadType();
        }

        public bool IsExceptional()
        {
            return _responseMessage.IsExceptional();
        }

        public Exception? OptionalExceptionResult()
        {
            return _responseMessage.OptionalExceptionResult();
        }

        public IQueryResponseMessage<R> WithMetaData(IReadOnlyDictionary<string, object> metaData)
        {
            return new ConvertingResponseMessage<R, T>(_expectedResponseType, _responseMessage.WithMetaData(metaData));
        }

        public IQueryResponseMessage<R> AndMetaData(IReadOnlyDictionary<string, object> additionalMetaData)
        {
            return new ConvertingResponseMessage<R, T>(_expectedResponseType,
                _responseMessage.AndMetaData(additionalMetaData));
        }

        IMessage<R> IMessage<R>.WithMetaData(IReadOnlyDictionary<string, object> metaData)
        {
            return (IMessage<R>) _responseMessage.WithMetaData(metaData);
        }

        IMessage<R> IMessage<R>.AndMetaData(IReadOnlyDictionary<string, object> metaData)
        {
            return (IMessage<R>) _responseMessage.AndMetaData(metaData);
        }
    }
}