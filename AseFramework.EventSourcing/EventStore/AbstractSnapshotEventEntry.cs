using System;
using System.Runtime.Serialization;
using Ase.Messaging.Common;
using Ase.Messaging.EventHandling;
using Ase.Messaging.Serialization;
using NHibernate.Event;
using NHMA = NHibernate.Mapping.Attributes;

namespace AseFramework.EventSourcing.EventStore
{
    /// <summary>
    /// Abstract base class of a serialized snapshot event storing the state of an aggregate. If JPA is used these entries
    /// have a primary key that is a combination of the aggregate's identifier, sequence number and type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    // @MappedSuperclass
    // @IdClass(AbstractSnapshotEventEntry.PK.class)
    public class AbstractSnapshotEventEntry<T> : AbstractEventEntry<T>, IDomainEventData<T>
        where T : class
    {
        [NHMA.Id] private string _aggregateIdentifier;
        [NHMA.Id] private long _sequenceNumber;
        [NHMA.Id] private string _type;

        /// <summary>
        /// Construct a new event entry from a published domain event message to enable storing the event or sending it to a
        /// remote location.
        /// <p>
        /// The given {@code serializer} will be used to serialize the payload and metadata in the given {@code
        /// eventMessage}. The type of the serialized data will be the same as the given {@code contentType}.
        /// </summary>
        /// <param name="eventMessage">The event message to convert to a serialized event entry</param>
        /// <param name="serializer">The serializer to convert the event</param>
        /// <param name="contentType">The data type of the payload and metadata after serialization</param>
        public AbstractSnapshotEventEntry(IDomainEventMessage<T> eventMessage, ISerializer serializer, Type contentType)
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
        public AbstractSnapshotEventEntry(
            string type,
            string aggregateIdentifier,
            long sequenceNumber,
            string eventIdentifier,
            object timestamp,
            string payloadType,
            string payloadRevision,
            T payload,
            T metaData
        ) : base(eventIdentifier, timestamp, payloadType, payloadRevision, payload, metaData)
        {
            _type = type;
            _aggregateIdentifier = aggregateIdentifier;
            _sequenceNumber = sequenceNumber;
        }

        protected AbstractSnapshotEventEntry()
        {
        }

        public string Type()
        {
            return _type;
        }

        public string GetAggregateIdentifier()
        {
            return _aggregateIdentifier;
        }

        public long GetSequenceNumber()
        {
            return _sequenceNumber;
        }

        // @SuppressWarnings("UnusedDeclaration")
        public class PK /* : Serializable */
        {
            private string _aggregateIdentifier;
            private long _sequenceNumber;
            private string _type;

            /**
         * Constructor for JPA. Not to be used directly
         */
            public PK()
            {
            }


            public override bool Equals(object o)
            {
                if (this == o)
                {
                    return true;
                }

                if (o == null || GetType() != o.GetType())
                {
                    return false;
                }

                PK pk = (PK) o;
                return /*sequenceNumber == pk.sequenceNumber &&*/
                       Equals(_aggregateIdentifier, pk._aggregateIdentifier) &&
                       Equals(_type, pk._type);
            }

            public override int GetHashCode()
            {
                return System.HashCode.Combine(_aggregateIdentifier, _type/*, sequenceNumber*/);
            }

            public string ToString()
            {
                return "PK{type='" + _type + '\'' + ", aggregateIdentifier='" + _aggregateIdentifier + '\'' +
                       ", sequenceNumber=" + _sequenceNumber + '}';
            }
        }
    }
}