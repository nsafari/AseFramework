using System.Collections.Immutable;
using Ase.Messaging.Messaging.ResponseTypes;

namespace Ase.Messaging.QueryHandling
{
    /// <summary>
    /// Message type that carries a Subscription Query: a request for information. Besides a payload, Subscription Query
    /// Messages also carry the expected response type and update type. The response type is the type of result expected by
    /// the caller. The update type is type of incremental updates.
    /// <p>
    /// Handlers should only answer a query if they can respond with the appropriate response type and update type.
    /// </summary>
    /// <typeparam name="Q">the type of payload</typeparam>
    /// <typeparam name="I">the type of initial response</typeparam>
    /// <typeparam name="U">the type of incremental responses</typeparam>
    public interface ISubscriptionQueryMessage<Q, I, U> : IQueryMessage<Q, I>
        where Q : class where I : class where U : class
    {
        /// <summary>
        /// Returns the type of incremental responses.
        /// </summary>
        /// <returns>the type of incremental responses</returns>
        IResponseType<U> GetUpdateResponseType();

        /// <summary>
        /// Returns a copy of this SubscriptionQueryMessage with the given {@code metaData}. The payload remains unchanged.
        /// </summary>
        /// <param name="metaData">The new MetaData for the SubscriptionQueryMessage</param>
        /// <returns>a copy of this message with the given MetaData</returns>
        new ISubscriptionQueryMessage<Q, I, U> WithMetaData(IImmutableDictionary<string, object> metaData);

        /// <summary>
        /// Returns a copy of this SubscriptionQueryMessage with its MetaData merged with given {@code metaData}. The payload
        /// remains unchanged.
        /// </summary>
        /// <param name="additionalMetaData">The MetaData to merge into the SubscriptionQueryMessage</param>
        /// <returns>a copy of this message with the given additional MetaData</returns>
        new ISubscriptionQueryMessage<Q, I, U> AndMetaData(IImmutableDictionary<string, object> additionalMetaData);
    }
}