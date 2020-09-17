using System;
using Ase.Messaging.Common;
using Ase.Messaging.Serialization;
using NHMA = NHibernate.Mapping.Attributes;
    
namespace Ase.Messaging.EventHandling
{
    public abstract class AbstractEventEntry<T> : IEventData<T>
    {

        [NHMA.Column(NotNull = true, Unique = true)]
        private string _eventIdentifier;
        
        [NHMA.Column(NotNull = true)]
        private string _timeStamp;
        
        [NHMA.Column(NotNull = true)]
        private string _payloadType;
        
        [NHMA.Column(NotNull = false)]
        private string _payloadRevision;
        
        [NHMA.Column(Length = 10000, NotNull = true, SqlType = "DbType.String")]
        private T _payload;
        
        [NHMA.Column(Length = 10000, NotNull = true, SqlType = "DbType.String")]
        private T _metaData;

        public AbstractEventEntry(
            IEventMessage<object> eventMessage, 
            ISerializer serializer, 
            Type contentType) {
            ISerializedObject<T> payload = eventMessage.SerializePayload<T>(serializer, contentType);
            ISerializedObject<T> metaData = eventMessage.SerializeMetaData<T>(serializer, contentType);
            _eventIdentifier = eventMessage.GetIdentifier();
            _payloadType = payload.Type().GetName();
            _payloadRevision = payload.Type().GetRevision();
            _payload = payload.GetData();
            _metaData = metaData.GetData();
            _timeStamp = DateTimeUtils.FormatInstant(eventMessage.GetTimestamp()!);
        }

        public AbstractEventEntry(string eventIdentifier, Object timestamp, string payloadType, string payloadRevision,
            T payload, T metaData) {
            this._eventIdentifier = eventIdentifier;
            if (timestamp instanceof TemporalAccessor) {
                this._timeStamp = formatInstant((TemporalAccessor) timestamp);
            } else {
                this._timeStamp = timestamp.tostring();
            }
            this._payloadType = payloadType;
            this._payloadRevision = payloadRevision;
            this._payload = payload;
            this._metaData = metaData;
        }

        
        protected AbstractEventEntry() {
        }

        
    }
}