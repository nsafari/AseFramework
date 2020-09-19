using System;
using Ase.Messaging.Serialization;
using NHMA = NHibernate.Mapping.Attributes;

namespace Ase.Messaging.EventHandling
{
    public class AbstractDomainEventEntry<T> : AbstractEventEntry<T>, IDomainEventData<T>
        where T : class
    {
        [NHMA.ColumnAttribute] private string _type;

        [NHMA.ColumnAttribute(NotNull = false)]
        private readonly string _aggregateIdentifier;

        [NHMA.ColumnAttribute(NotNull = false)]
        private readonly long _sequenceNumber;

        public AbstractDomainEventEntry(IDomainEventMessage<T> eventMessage, ISerializer serializer, Type contentType)
            : base(eventMessage, serializer, contentType)
        {
            _type = eventMessage.Type();
            _aggregateIdentifier = eventMessage.GetAggregateIdentifier();
            _sequenceNumber = eventMessage.GetSequenceNumber();
        }

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