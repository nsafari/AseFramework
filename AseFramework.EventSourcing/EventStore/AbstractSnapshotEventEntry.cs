using System;
using System.Runtime.Serialization;
using Ase.Messaging.Common;
using Ase.Messaging.EventHandling;
using Ase.Messaging.Serialization;
using NHibernate.Event;
using NHMA = NHibernate.Mapping.Attributes;

namespace AseFramework.EventSourcing.EventStore
{
    // @MappedSuperclass
    // @IdClass(AbstractSnapshotEventEntry.PK.class)
    public class AbstractSnapshotEventEntry<T> : AbstractEventEntry<T>, IDomainEventData<T>
        where T : class
    {
        [NHMA.Id] private string _aggregateIdentifier;
        [NHMA.Id] private long _sequenceNumber;
        [NHMA.Id] private string _type;

        public AbstractSnapshotEventEntry(IDomainEventMessage<T> eventMessage, ISerializer serializer, Type contentType)
            : base(eventMessage, serializer, contentType)
        {
            _type = eventMessage.Type();
            _aggregateIdentifier = eventMessage.GetAggregateIdentifier();
            _sequenceNumber = eventMessage.GetSequenceNumber();
        }

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