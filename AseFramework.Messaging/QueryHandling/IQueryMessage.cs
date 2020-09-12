using System.Collections.Immutable;
using Ase.Messaging.Messaging;
using Ase.Messaging.Messaging.ResponseTypes;

namespace Ase.Messaging.QueryHandling
{
    /// <summary>
    /// Message type that carries a Query: a request for information. Besides a payload, Query Messages also carry the
    /// expected response type. This is the type of result expected by the caller.
    /// <p>
    /// Handlers should only answer a query if they can respond with the appropriate response type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TR"></typeparam>
    public interface IQueryMessage<out T, TR> : IMessage<T> 
        where T : class where TR : class
    {
        /// <summary>
        /// Returns the name identifying the query to be executed.
        /// </summary>
        /// <returns>the name identifying the query to be executed.</returns>
        string GetQueryName();

        /// <summary>
        /// The type of response expected by the sender of the query
        /// </summary>
        /// <returns>the type of response expected by the sender of the query</returns>
        IResponseType<TR> GetResponseType();
        
        /// <summary>
        /// Returns a copy of this QueryMessage with the given {@code metaData}. The payload remains unchanged.
        /// </summary>
        /// <param name="metaData">The new MetaData for the QueryMessage</param>
        /// <returns>a copy of this message with the given MetaData</returns>
        IQueryMessage<T, TR> WithMetaData(IImmutableDictionary<string, object> metaData);

        /// <summary>
        /// Returns a copy of this QueryMessage with its MetaData merged with given {@code metaData}. The payload
        /// remains unchanged.
        /// </summary>
        /// <param name="additionalMetaData">The MetaData to merge into the QueryMessage</param>
        /// <returns>a copy of this message with the given additional MetaData</returns>
        IQueryMessage<T, TR> AndMetaData(IImmutableDictionary<string, object> additionalMetaData);

    }
}