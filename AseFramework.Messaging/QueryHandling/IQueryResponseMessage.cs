using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using Ase.Messaging.Messaging;

namespace Ase.Messaging.QueryHandling
{
    /// <summary>
    /// Message that contains the results of a Query. Results are represented as a Collection of result objects. When
    /// a query resulted in a single result object, that object is contained as the sole element of the collection.
    /// </summary>
    /// <typeparam name="T">The type of object resulting from the query</typeparam>
    public interface IQueryResponseMessage<T> : IResultMessage<T>
        where T : class
    {
        /// <summary>
        /// Returns a copy of this QueryResponseMessage with the given {@code metaData}. The payload remains unchanged.
        /// </summary>
        /// <param name="metaData">The new MetaData for the QueryResponseMessage</param>
        /// <returns>a copy of this message with the given MetaData</returns>
        IQueryResponseMessage<T> WithMetaData(IReadOnlyDictionary<string, object> metaData);

        /// <summary>
        /// Returns a copy of this QueryResponseMessage with its MetaData merged with given {@code metaData}. The payload
        /// remains unchanged.
        /// </summary>
        /// <param name="additionalMetaData">The MetaData to merge into the QueryResponseMessage</param>
        /// <returns>a copy of this message with the given additional MetaData</returns>
        IQueryResponseMessage<T> AndMetaData(IReadOnlyDictionary<string, object> additionalMetaData);
    }
}