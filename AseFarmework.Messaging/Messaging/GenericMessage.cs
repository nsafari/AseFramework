using System;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using Ase.Messaging.Common;

namespace Ase.Messaging.Messaging
{
    public class GenericMessage<T> : AbstractMessage<T>
    {
        private readonly MetaData _metaData;
        private readonly Type _payloadType;
        private readonly T _payload;

        public GenericMessage(string identifier) : base(identifier)
        {
        }

        /// <summary>
        /// Constructs a Message for the given {@code payload} using the correlation data of the current Unit of Work, if
        /// present.
        /// </summary>
        /// <param name="payload">The payload for the message</param>
        public GenericMessage(T payload) : this(payload, MetaData.EmptyInstance)
        {
        }

        /// <summary>
        /// Constructs a Message for the given {@code payload} and {@code meta data}. The given {@code metaData} is
        /// merged with the MetaData from the correlation data of the current unit of work, if present.
        /// In case the {@code payload == null}, {@link Void} will be used as the {@code payloadType}.
        /// </summary>
        /// <param name="payload">The payload for the message as a generic {@code T}</param>
        /// <param name="metaData">The meta data {@link Map} for the message</param>
        public GenericMessage(T payload, IImmutableDictionary<string, object> metaData) : this(
            GetDeclaredPayloadType(payload), payload, metaData)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="declaredPayloadType"></param>
        /// <param name="payload"></param>
        /// <param name="metaData"></param>
        private GenericMessage(Type declaredPayloadType, T payload, IImmutableDictionary<string, object> metaData) : this(
            IdentifierFactory.GetInstance().GenerateIdentifier(), declaredPayloadType, payload, metaData)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="payload"></param>
        /// <param name="metaData"></param>
        public GenericMessage(string identifier, T payload, IImmutableDictionary<string, object> metaData) : this(
            identifier, GetDeclaredPayloadType(payload), payload, metaData)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="declaredPayloadType"></param>
        /// <param name="payload"></param>
        /// <param name="metaData"></param>
        public GenericMessage(string identifier, Type declaredPayloadType, T payload,
            IImmutableDictionary<string, object> metaData) : base(identifier)
        {
        }


        /// <summary>
        /// Returns a Message representing the given {@code payloadOrMessage}, either by wrapping it or by returning it
        /// as-is. If the given {@code payloadOrMessage} already implements {@link Message}, it is returned as-is, otherwise
        /// a {@link Message} is returned with the parameter as its payload.
        /// </summary>
        /// <param name="payloadOrMessage">The payload to wrap or message to return</param>
        /// <returns>a Message with the given payload or the message</returns>
        public static IMessage<V> AsMessage<V>(V payloadOrMessage)
        {
            if (payloadOrMessage is IMessage<V> asMessage)
            {
                return asMessage;
            }
            else
            {
                return new GenericMessage<V>(payloadOrMessage);
            }
        }

        /// <summary>
        /// Extract the {@link Class} of the provided {@code payload}. If {@code payload == null} this function returns
        /// {@link Void} as the payload type.
        /// </summary>
        /// <param name="payload">the payload of this {@link Message}</param>
        /// <returns>the declared type of the given {@code payload} or {@link Void} if {@code payload == null}</returns>
        private static Type GetDeclaredPayloadType(T payload)
        {
            return payload != null ? payload.GetType() : typeof(void);
        }


        public override MetaData GetMetaData()
        {
            throw new System.NotImplementedException();
        }

        public override T GetPayload()
        {
            throw new System.NotImplementedException();
        }

        protected override IMessage<T> WithMetaData(MetaData metaData)
        {
            throw new System.NotImplementedException();
        }
    }
}