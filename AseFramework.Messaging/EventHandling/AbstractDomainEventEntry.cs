using System;
using Ase.Messaging.Serialization;
using NHMA = NHibernate.Mapping.Attributes;

namespace Ase.Messaging.EventHandling
{
    /// <summary>
    /// Abstract base class of a serialized domain event. Fields in this class contain JPA annotations that direct JPA event
    /// storage engines how to store event entries.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AbstractDomainEventEntry<T> : AbstractEventEntry<T>, IDomainEventData<T>
        where T : class
    {
        [NHMA.ColumnAttribute] private string _type;

        [NHMA.ColumnAttribute(NotNull = false)]
        private readonly string _aggregateIdentifier;

        [NHMA.ColumnAttribute(NotNull = false)]
        private readonly long _sequenceNumber;

        /// <summary>
        /// Construct a new event entry from a published domain event message to enable storing the event or sending it to a
        /// remote location.
        /// <p>
        /// The given {@code serializer} will be used to serialize the payload and metadata in the given {@code eventMessage}.
        /// The type of the serialized data will be the same as the given {@code contentType}.
        /// </summary>
        /// <param name="eventMessage">The event message to convert to a serialized event entry</param>
        /// <param name="serializer">The serializer to convert the event</param>
        /// <param name="contentType">The data type of the payload and metadata after serialization</param>
        public AbstractDomainEventEntry(IDomainEventMessage<T> eventMessage, ISerializer serializer, Type contentType)
            : base(eventMessage, serializer, contentType)
        {
            _type = eventMessage.Type();
            _aggregateIdentifier = eventMessage.GetAggregateIdentifier();
            _sequenceNumber = eventMessage.GetSequenceNumber();
        }

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
        public AbstractDomainEventEntry(
            string type, string aggregateIdentifier, long sequenceNumber,
            string eventIdentifier, Object timestamp, string payloadType,
            string payloadRevision, T payload, T metaData
            ) : base(eventIdentifier, timestamp, payloadType, payloadRevision, payload, metaData)
        {
            _type = type;
            _aggregateIdentifier = aggregateIdentifier;
            _sequenceNumber = sequenceNumber;
        }
        
        
        /// <summary>
        /// Default constructor required by JPA
        /// Should be removed?
        /// </summary>
        protected AbstractDomainEventEntry() {
            
        }
        
        public string Type() {
            return _type;
        }
        
        public string GetAggregateIdentifier() {
            return _aggregateIdentifier;
        }
        
        public long GetSequenceNumber() {
            return _sequenceNumber;
        }

    }
}