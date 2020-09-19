using System;
using System.Collections.Immutable;
using System.Text;
using Ase.Messaging.Common.Wrapper;
using Ase.Messaging.Messaging;
using Ase.Messaging.Serialization;

namespace Ase.Messaging.EventHandling
{
    /// <summary>
    /// Generic implementation of a {@link DomainEventMessage}.
    /// </summary>
    /// <typeparam name="T">The type of payload contained in this Message</typeparam>
    public class GenericDomainEventMessage<T> : GenericEventMessage<T>, IDomainEventMessage<T>
        where T : class
    {
        private readonly string _type;
        private readonly string _aggregateIdentifier;
        private readonly long _sequenceNumber;

        /// <summary>
        /// Initialize a DomainEventMessage originating from an Aggregate with the given {@code aggregateIdentifier},
        /// with given {@code sequenceNumber} and {@code payload}. The MetaData of the message is empty.
        /// </summary>
        /// <param name="type">The domain type</param>
        /// <param name="aggregateIdentifier">The identifier of the aggregate generating this message</param>
        /// <param name="sequenceNumber">The message's sequence number</param>
        /// <param name="payload">The application-specific payload of the message</param>
        public GenericDomainEventMessage(string type, string aggregateIdentifier, long sequenceNumber, T payload) :
            this(type, aggregateIdentifier, sequenceNumber, payload, MetaData.EmptyInstance)
        {
        }

        /// <summary>
        /// Initialize a DomainEventMessage originating from an Aggregate with the given {@code aggregateIdentifier},
        /// with given {@code sequenceNumber} and {@code payload} and {@code metaData}.
        /// </summary>
        /// <param name="type">The domain type</param>
        /// <param name="aggregateIdentifier">The identifier of the aggregate generating this message</param>
        /// <param name="sequenceNumber">The message's sequence number</param>
        /// <param name="payload">The application-specific payload of the message</param>
        /// <param name="metaData">The MetaData to attach to the message</param>
        public GenericDomainEventMessage(string type, string aggregateIdentifier, long sequenceNumber, T payload,
            IImmutableDictionary<string, object> metaData) :
            this(type,
                aggregateIdentifier,
                sequenceNumber,
                new GenericMessage<T>(payload, metaData),
                Clock)
        {
        }

        /// <summary>
        /// Initialize a DomainEventMessage originating from an Aggregate using existing data.
        /// </summary>
        /// <param name="type">The domain type</param>
        /// <param name="aggregateIdentifier">The identifier of the aggregate generating this message</param>
        /// <param name="sequenceNumber">The message's sequence number</param>
        /// <param name="payload">The application-specific payload of the message</param>
        /// <param name="metaData">The MetaData to attach to the message</param>
        /// <param name="messageIdentifier">The message identifier</param>
        /// <param name="timestamp">The event's timestamp</param>
        public GenericDomainEventMessage(string type, string aggregateIdentifier, long sequenceNumber, T payload,
            IImmutableDictionary<string, object> metaData, string messageIdentifier,
            InternalDateTimeOffset timestamp)
            : this(type, aggregateIdentifier, sequenceNumber,
                new GenericMessage<T>(messageIdentifier, payload, metaData),
                timestamp)
        {
        }

        /// <summary>
        /// Initialize a DomainEventMessage originating from an Aggregate using existing data. The timestamp of the event is
        /// supplied lazily to prevent unnecessary deserialization of the timestamp.
        /// </summary>
        /// <param name="type">The domain type</param>
        /// <param name="aggregateIdentifier">The identifier of the aggregate generating this message</param>
        /// <param name="sequenceNumber">The message's sequence number</param>
        /// <param name="delegate">The delegate message providing the payload, metadata and identifier of the event</param>
        /// <param name="timestamp">The event's timestamp supplier</param>
        public GenericDomainEventMessage(string type, string aggregateIdentifier, long sequenceNumber,
            IMessage<T> @delegate,
            CachingSupplier<InternalDateTimeOffset> timestamp) : base(@delegate, timestamp)
        {
            _type = type;
            _aggregateIdentifier = aggregateIdentifier;
            _sequenceNumber = sequenceNumber;
        }

        /// <summary>
        /// Initialize a DomainEventMessage originating from an Aggregate with the given {@code aggregateIdentifier},
        /// with given {@code sequenceNumber} and {@code payload}, {@code metaData} and {@code timestamp}.
        /// </summary>
        /// <param name="type">the aggregate type</param>
        /// <param name="aggregateIdentifier">the aggregate identifier</param>
        /// <param name="sequenceNumber">the sequence number of the event</param>
        /// <param name="delegate">The delegate message providing the payload, metadata and identifier of the event</param>
        /// <param name="timestamp">the timestamp of the event</param>
        public GenericDomainEventMessage(string type, string aggregateIdentifier, long sequenceNumber,
            IMessage<T> @delegate, InternalDateTimeOffset? timestamp) : base(@delegate, timestamp)
        {
            _type = type;
            _aggregateIdentifier = aggregateIdentifier;
            _sequenceNumber = sequenceNumber;
        }


        public new string Type()
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

        public new IDomainEventMessage<T> WithMetaData(IImmutableDictionary<string, object> metaData)
        {
            if (GetMetaData().Equals(metaData))
            {
                return this;
            }

            return new GenericDomainEventMessage<T>(
                _type,
                _aggregateIdentifier,
                _sequenceNumber,
                Delegate().WithMetaData(metaData),
                GetTimestamp()!
            );
        }

        public new IDomainEventMessage<T> AndMetaData(IImmutableDictionary<string, object> metaData)
        {
            if (metaData.Count == 0 || GetMetaData().Equals(metaData))
            {
                return this;
            }

            return new GenericDomainEventMessage<T>(_type, _aggregateIdentifier, _sequenceNumber,
                Delegate().AndMetaData(metaData), GetTimestamp()!);
        }

        protected override void DescribeTo(StringBuilder stringBuilder)
        {
            base.DescribeTo(stringBuilder);
            stringBuilder.Append('\'').Append(", aggregateType='")
                .Append(Type()).Append('\'')
                .Append(", aggregateIdentifier='")
                .Append(GetAggregateIdentifier()).Append('\'')
                .Append(", sequenceNumber=")
                .Append(GetSequenceNumber());
        }

        protected override string DescribeType()
        {
            return "GenericDomainEventMessage";
        }
    }
}