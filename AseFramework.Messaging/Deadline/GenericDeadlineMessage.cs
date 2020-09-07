using System;
using System.Collections.Immutable;
using Ase.Messaging.Common.Wrapper;
using Ase.Messaging.EventHandling;
using Ase.Messaging.Messaging;
using Ase.Messaging.Serialization;

namespace Ase.Messaging.Deadline
{
    public class GenericDeadlineMessage<T> : GenericEventMessage<T>, IDeadlineMessage<T>
        where T : class
    {
        private readonly string _deadlineName;


        [Obsolete]
        public static IDeadlineMessage<T> AsDeadlineMessage(string deadlineName, object messageOrPayload)
        {
            return AsDeadlineMessage(deadlineName, messageOrPayload, Clock);
        }

        public static IDeadlineMessage<T> AsDeadlineMessage(string deadlineName,
            object messageOrPayload,
            InternalDateTimeOffset expiryTime)
        {
            return messageOrPayload is IMessage<T> message
                ? new GenericDeadlineMessage<T>(deadlineName, message,
                    CachingSupplier<InternalDateTimeOffset>.Of(() => expiryTime))
                : new GenericDeadlineMessage<T>(deadlineName, new GenericMessage<T>((T) messageOrPayload),
                    CachingSupplier<InternalDateTimeOffset>.Of(() => expiryTime));
        }

        public GenericDeadlineMessage(string deadlineName) : this(deadlineName, null)
        {
        }

        public GenericDeadlineMessage(string deadlineName, T? payload) : this(deadlineName, payload,
            MetaData.EmptyInstance)
        {
        }

        public GenericDeadlineMessage(string deadlineName, T? payload, IImmutableDictionary<string, object> metaData) :
            base(payload, metaData)
        {
            _deadlineName = deadlineName;
        }

        public GenericDeadlineMessage(string deadlineName, IMessage<T> @delegate,
            CachingSupplier<InternalDateTimeOffset> timestampSupplier) : base(@delegate, timestampSupplier)
        {
            _deadlineName = deadlineName;
        }

        public string GetDeadlineName()
        {
            return _deadlineName;
        }

        public new IDeadlineMessage<T> WithMetaData(IImmutableDictionary<string, object> metaData)
        {
            return new GenericDeadlineMessage<T>(_deadlineName, Delegate().WithMetaData(metaData),
                CachingSupplier<InternalDateTimeOffset>.Of(() => GetTimestamp()!));
        }

        public new IDeadlineMessage<T> AndMetaData(IImmutableDictionary<string, object> additionalMetaData)
        {
            return new GenericDeadlineMessage<T>(
                _deadlineName, Delegate().AndMetaData(additionalMetaData),
                CachingSupplier<InternalDateTimeOffset>.Of(() => GetTimestamp()!)
            );
        }
    }
}