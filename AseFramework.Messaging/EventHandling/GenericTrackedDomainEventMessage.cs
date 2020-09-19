using System.Collections.Immutable;
using System.Text;
using Ase.Messaging.Common.Wrapper;
using Ase.Messaging.Messaging;
using Ase.Messaging.Serialization;

namespace Ase.Messaging.EventHandling
{

    /// <summary>
    /// Generic implementation of a {@link DomainEventMessage} that is also a {@link TrackedEventMessage}.
    /// </summary>
    /// <typeparam name="T">The type of payload contained in this Message</typeparam>
    public class GenericTrackedDomainEventMessage<T> : GenericDomainEventMessage<T>
        where T : class
    {
        private readonly ITrackingToken _trackingToken;

        /// <summary>
        /// Initialize a DomainEventMessage originating from an aggregate.
        /// </summary>
        /// <param name="trackingToken">Tracking token of the event</param>
        /// <param name="delegate">Delegate domain event containing other event data</param>
        public GenericTrackedDomainEventMessage(
            ITrackingToken trackingToken,
            IDomainEventMessage<T> @delegate
        ) : this(
            trackingToken,
            @delegate.Type(),
            @delegate.GetAggregateIdentifier(),
            @delegate.GetSequenceNumber(),
            @delegate,
            @delegate.GetTimestamp()
        )
        {
        }

        /// <summary>
        /// Initialize a DomainEventMessage originating from an Aggregate using existing data. The timestamp of the event is
        /// supplied lazily to prevent unnecessary deserialization of the timestamp.
        /// </summary>
        /// <param name="trackingToken">Tracking token of the event</param>
        /// <param name="type">The domain type</param>
        /// <param name="aggregateIdentifier">The identifier of the aggregate generating this message</param>
        /// <param name="sequenceNumber">The message's sequence number</param>
        /// <param name="delegate">The delegate message providing the payload, metadata and identifier of the event</param>
        /// <param name="timestamp">The event's timestamp supplier</param>
        public GenericTrackedDomainEventMessage(
            ITrackingToken trackingToken,
            string type,
            string aggregateIdentifier,
            long sequenceNumber,
            IMessage<T> @delegate,
            CachingSupplier<InternalDateTimeOffset> timestamp
        ) : base(type, aggregateIdentifier, sequenceNumber, @delegate, timestamp)
        {
            _trackingToken = trackingToken;
        }

        /// <summary>
        /// Initialize a DomainEventMessage originating from an aggregate.
        /// </summary>
        /// <param name="trackingToken">Tracking token of the event</param>
        /// <param name="type">The domain type</param>
        /// <param name="aggregateIdentifier">The identifier of the aggregate generating this message</param>
        /// <param name="sequenceNumber">The message's sequence number</param>
        /// <param name="delegate">The delegate message providing the payload, metadata and identifier of the event</param>
        /// <param name="timestamp">The event's timestamp</param>
        protected GenericTrackedDomainEventMessage(
            ITrackingToken trackingToken,
            string type,
            string aggregateIdentifier,
            long sequenceNumber,
            IMessage<T> @delegate,
            InternalDateTimeOffset? timestamp
        ) : base(type, aggregateIdentifier, sequenceNumber, @delegate, timestamp)
        {
            _trackingToken = trackingToken;
        }

        public ITrackingToken TrackingToken()
        {
            return _trackingToken;
        }

        public new GenericTrackedDomainEventMessage<T> WithMetaData(IImmutableDictionary<string, object> metaData)
        {
            return new GenericTrackedDomainEventMessage<T>(
                _trackingToken,
                Type(),
                GetAggregateIdentifier(),
                GetSequenceNumber(),
                Delegate().WithMetaData(metaData),
                GetTimestamp()
            );
        }

        public new GenericTrackedDomainEventMessage<T> AndMetaData(IImmutableDictionary<string, object> metaData)
        {
            return new GenericTrackedDomainEventMessage<T>(
                _trackingToken,
                Type(),
                GetAggregateIdentifier(),
                GetSequenceNumber(),
                Delegate().AndMetaData(metaData),
                GetTimestamp()
            );
        }

        protected override void DescribeTo(StringBuilder stringBuilder)
        {
            base.DescribeTo(stringBuilder);
            stringBuilder.Append(", trackingToken={")
                .Append(TrackingToken())
                .Append('}');
        }

        public GenericTrackedDomainEventMessage<T> WithTrackingToken(ITrackingToken trackingToken)
        {
            return new GenericTrackedDomainEventMessage<T>(trackingToken, this);
        }

        protected override string DescribeType()
        {
            return "GenericTrackedDomainEventMessage";
        }
    }
}