namespace Ase.Messaging.EventHandling
{
    /// <summary>
    /// Generic implementation of a serialized domain event entry. This implementation can be used by Event Storage Engine
    /// implementations to reconstruct an event or snapshot from the underlying storage for example.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericDomainEventEntry<T> : AbstractDomainEventEntry<T>
    {
        
        /// <summary>
        /// Reconstruct an event entry from a stored object. 
        /// </summary>
        /// <param name="type">The type of aggregate that published this event</param>
        /// <param name="aggregateIdentifier">The identifier of the aggregate that published this event</param>
        /// <param name="sequenceNumber">The sequence number of the event in the aggregate</param>
        /// <param name="eventIdentifier">The identifier of the event</param>
        /// <param name="timestamp">The time at which the event was originally created</param>
        /// <param name="payloadType">The fully qualified class name or alias of the event payload</param>
        /// <param name="payloadRevision">The revision of the event payload</param>
        /// <param name="payload">The serialized payload</param>
        /// <param name="metaData">The serialized metadata</param>
        public GenericDomainEventEntry(
            string type,
            string aggregateIdentifier,
            long sequenceNumber,
            string eventIdentifier,
            object timestamp,
            string payloadType,
            string payloadRevision,
            T payload,
            T metaData)
            : base(
                type, aggregateIdentifier, sequenceNumber,
                eventIdentifier, timestamp, payloadType,
                payloadRevision, payload, metaData
            )
        {
        }
    }
}