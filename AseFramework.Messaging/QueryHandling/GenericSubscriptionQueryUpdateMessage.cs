using System;
using System.Collections.Immutable;
using System.Reflection;
using Ase.Messaging.Messaging;

namespace Ase.Messaging.QueryHandling
{
    /// <summary>
    /// Generic {@link SubscriptionQueryUpdateMessage} which holds incremental update of an subscription query.
    /// </summary>
    /// <typeparam name="U">type of incremental update</typeparam>
    public class GenericSubscriptionQueryUpdateMessage<U> : MessageDecorator<U>, ISubscriptionQueryUpdateMessage<U>
        where U : class
    {
        /// <summary>
        /// Creates {@link GenericSubscriptionQueryUpdateMessage} from provided {@code payload} which represents incremental
        /// update. The provided {@code payload} may not be {@code null}.
        /// </summary>
        /// <param name="payload">incremental update</param>
        /// <typeparam name="T">type of the {@link GenericSubscriptionQueryUpdateMessage}</typeparam>
        /// <returns></returns>
        public ISubscriptionQueryUpdateMessage<T> AsUpdateMessage<T>(object payload) where T : class
        {
            return payload switch
            {
                ISubscriptionQueryUpdateMessage<T> subscribeMessage => subscribeMessage,
                IMessage<T> message => new GenericSubscriptionQueryUpdateMessage<T>(message),
                _ => new GenericSubscriptionQueryUpdateMessage<T>((T) payload)
            };
        }

        /// <summary>
        /// Initializes {@link GenericSubscriptionQueryUpdateMessage} with incremental update.
        /// </summary>
        /// <param name="payload">payload of the message which represent incremental update</param>
        public GenericSubscriptionQueryUpdateMessage(U payload)
            : this(new GenericMessage<U>(payload, MetaData.EmptyInstance))
        {
        }

        /// <summary>
        /// Initializes {@link GenericSubscriptionQueryUpdateMessage} with incremental update of provided {@code
        /// declaredType} and {@code metaData}.
        /// </summary>
        /// <param name="declaredType">the type of the update</param>
        /// <param name="payload">the payload of the update</param>
        public GenericSubscriptionQueryUpdateMessage(Type declaredType, U payload)
            : this(declaredType, payload, MetaData.EmptyInstance)
        {
        }

        /// <summary>
        /// Initializes {@link GenericSubscriptionQueryUpdateMessage} with incremental update of provided {@code
        /// declaredType} and {@code metaData}.
        /// </summary>
        /// <param name="declaredType">the type of the update</param>
        /// <param name="payload">the payload of the update</param>
        /// <param name="metaData">the metadata of the update</param>
        public GenericSubscriptionQueryUpdateMessage(
            Type declaredType,
            U payload,
            IImmutableDictionary<string, object> metaData
        ) : base(new GenericMessage<U>(declaredType, payload, metaData))
        {
        }

        /// <summary>
        /// Initializes a new decorator with given {@code delegate} message. The decorator delegates to the delegate for
        /// the message's payload, metadata and identifier.
        /// </summary>
        /// <param name="delegate">the message delegate</param>
        protected GenericSubscriptionQueryUpdateMessage(IMessage<U> @delegate)
            : base(@delegate)
        {
        }
        
        public ISubscriptionQueryUpdateMessage<U> WithMetaData(IImmutableDictionary<string, object> metaData) {
            return new GenericSubscriptionQueryUpdateMessage<U>(Delegate().WithMetaData(metaData));
        }
        
        public ISubscriptionQueryUpdateMessage<U> AndMetaData(IImmutableDictionary<string, object> metaData) {
            return new GenericSubscriptionQueryUpdateMessage<U>(Delegate().AndMetaData(metaData));
        }
        
        protected override string DescribeType() {
            return "GenericSubscriptionQueryUpdateMessage";
        }
    }
}