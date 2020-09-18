using System;
using Ase.Messaging.Common;
using Ase.Messaging.Common.Wrapper;
using Ase.Messaging.Serialization;
using NHMA = NHibernate.Mapping.Attributes;

namespace Ase.Messaging.EventHandling
{
    /// <summary>
    /// Abstract base class of a serialized event. Fields in this class contain JPA annotations that direct JPA event storage
    /// engines how to store event entries.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AbstractEventEntry<T> : IEventData<T>
    {
        [NHMA.ColumnAttribute(NotNull = true, Unique = true)]
        private string _eventIdentifier;

        [NHMA.ColumnAttribute(NotNull = true)] private string? _timeStamp;

        [NHMA.ColumnAttribute(NotNull = true)] private string _payloadType;

        [NHMA.ColumnAttribute(NotNull = false)]
        private string _payloadRevision;

        [NHMA.ColumnAttribute(Length = 10000, NotNull = true, SqlType = "DbType.String")]
        private T _payload;

        [NHMA.ColumnAttribute(Length = 10000, NotNull = true, SqlType = "DbType.String")]
        private T _metaData;

        /// <summary>
        /// Construct a new event entry from a published event message to enable storing the event or sending it to a remote
        /// location.
        /// <p>
        /// The given {@code serializer} will be used to serialize the payload and metadata in the given {@code eventMessage}.
        /// The type of the serialized data will be the same as the given {@code contentType}.
        /// </summary>
        /// <param name="eventMessage">The event message to convert to a serialized event entry</param>
        /// <param name="serializer">The serializer to convert the event</param>
        /// <param name="contentType">The data type of the payload and metadata after serialization</param>
        public AbstractEventEntry(
            IEventMessage<object> eventMessage,
            ISerializer serializer,
            Type contentType)
        {
            ISerializedObject<T> payload = eventMessage.SerializePayload<T>(serializer, contentType);
            ISerializedObject<T> metaData = eventMessage.SerializeMetaData<T>(serializer, contentType);
            _eventIdentifier = eventMessage.GetIdentifier();
            _payloadType = payload.Type()!.GetName();
            _payloadRevision = payload.Type()!.GetRevision();
            _payload = payload.GetData();
            _metaData = metaData.GetData();
            _timeStamp = DateTimeUtils.FormatInstant(eventMessage.GetTimestamp()!);
        }

        /// <summary>
        /// Reconstruct an event entry from a stored object.
        /// </summary>
        /// <param name="eventIdentifier">The identifier of the event</param>
        /// <param name="timestamp">The time at which the event was originally created</param>
        /// <param name="payloadType">The fully qualified class name or alias of the event payload</param>
        /// <param name="payloadRevision">The revision of the event payload</param>
        /// <param name="payload">The serialized payload</param>
        /// <param name="metaData">The serialized metadata</param>
        public AbstractEventEntry(
            string eventIdentifier,
            object timestamp,
            string payloadType,
            string payloadRevision,
            T payload,
            T metaData
        )
        {
            _eventIdentifier = eventIdentifier;
            if (timestamp is InternalDateTimeOffset offset)
            {
                _timeStamp = DateTimeUtils.FormatInstant(offset);
            }
            else
            {
                _timeStamp = timestamp.ToString();
            }

            _payloadType = payloadType;
            _payloadRevision = payloadRevision;
            _payload = payload;
            _metaData = metaData;
        }

        /// <summary>
        /// Default constructor required by JPA
        /// Should be removed?
        /// </summary>
        protected AbstractEventEntry()
        {
        }

        public string GetEventIdentifier()
        {
            return _eventIdentifier;
        }

        public InternalDateTimeOffset GetTimestamp()
        {
            return DateTimeUtils.ParseInstant(_timeStamp);
        }

        public ISerializedObject<T> GetMetaData()
        {
            return new SerializedMetaData<T>(_metaData, _metaData!.GetType());
        }

        public ISerializedObject<T> GetPayload()
        {
            return new SimpleSerializedObject<T>(_payload, _payload!.GetType(),
                new SimpleSerializedType(_payloadType, _payloadRevision));
        }
    }
}