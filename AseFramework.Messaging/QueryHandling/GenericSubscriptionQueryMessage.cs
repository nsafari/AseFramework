using System.Collections.Immutable;
using Ase.Messaging.Messaging;
using Ase.Messaging.Messaging.ResponseTypes;

namespace Ase.Messaging.QueryHandling
{
    /// <summary>
    /// Generic implementation of the {@link SubscriptionQueryMessage}. Unless explicitly provided, it assumes the {@code
    /// queryName} of the message is the fully qualified class name of the message's payload.
    /// </summary>
    /// <typeparam name="Q">The type of payload expressing the query in this message</typeparam>
    /// <typeparam name="I">The type of initial response expected from this query</typeparam>
    /// <typeparam name="U">The type of incremental updates expected from this query</typeparam>
    public class GenericSubscriptionQueryMessage<Q, I, U> : GenericQueryMessage<Q, I>, ISubscriptionQueryMessage<Q, I, U>
        where Q : class where I : class where U : class
    {
        private readonly IResponseType<U> _updateResponseType;

        /// <summary>
        /// Initializes the message with the given {@code payload}, expected {@code responseType} and expected {@code
        /// updateResponseType}. The query name is set to the fully qualified class name of the {@code payload}.
        /// </summary>
        /// <param name="payload">The payload expressing the query</param>
        /// <param name="responseType">The expected response type</param>
        /// <param name="updateResponseType">The expected type of incremental updates</param>
        public GenericSubscriptionQueryMessage(
            Q payload,
            IResponseType<I> responseType,
            IResponseType<U> updateResponseType
        ) : this(payload, payload.GetType().ToString(), responseType, updateResponseType)
        {
        }

        /// <summary>
        /// Initializes the message with the given {@code payload}, {@code queryName}, expected {@code responseType} and
        /// expected {@code updateResponseType}.
        /// </summary>
        /// <param name="payload">The payload expressing the query</param>
        /// <param name="queryName">The name identifying the query to execute</param>
        /// <param name="responseType">The expected response type</param>
        /// <param name="updateResponseType">The expected type of incremental updates</param>
        public GenericSubscriptionQueryMessage(
            Q payload,
            string queryName,
            IResponseType<I> responseType,
            IResponseType<U> updateResponseType
        ) : base(payload, queryName, responseType)
        {
            _updateResponseType = updateResponseType;
        }

        /// <summary>
        /// Initializes the message, using given {@code delegate} as the carrier of payload and metadata and given {@code
        /// queryName}, expected {@code responseType} and expected {@code updateResponseType}.
        /// </summary>
        /// <param name="delegate">The message containing the payload and meta data for this message</param>
        /// <param name="queryName">The name identifying the query to execute</param>
        /// <param name="responseType">The expected response type</param>
        /// <param name="updateResponseType">The expected type of incremental updates</param>
        public GenericSubscriptionQueryMessage(
            IMessage<Q> @delegate,
            string queryName,
            IResponseType<I> responseType,
            IResponseType<U> updateResponseType
        ) : base(@delegate, queryName, responseType)
        {
            _updateResponseType = updateResponseType;
        }

        public IResponseType<U> GetUpdateResponseType()
        {
            return _updateResponseType;
        }

        public new GenericSubscriptionQueryMessage<Q, I, U> WithMetaData(IImmutableDictionary<string, object> metaData)
        {
            return new GenericSubscriptionQueryMessage<Q, I, U>(
                Delegate().WithMetaData(metaData),
                GetQueryName(),
                GetResponseType(),
                _updateResponseType
            );
        }

        public new GenericSubscriptionQueryMessage<Q, I, U> AndMetaData(IImmutableDictionary<string, object> metaData)
        {
            return new GenericSubscriptionQueryMessage<Q, I, U>(
                Delegate().AndMetaData(metaData),
                GetQueryName(),
                GetResponseType(),
                _updateResponseType);
        }
    }
}