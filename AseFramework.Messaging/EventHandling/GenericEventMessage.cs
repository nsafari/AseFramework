using System;
using System.Collections.Immutable;
using System.Text;
using Ase.Messaging.Common.Wrapper;
using Ase.Messaging.Messaging;
using Ase.Messaging.Serialization;

namespace Ase.Messaging.EventHandling
{
    /// <summary>
    /// Generic implementation of the EventMessage interface.
    /// </summary>
    /// <typeparam name="T">The type of payload contained in this Message</typeparam>
    public class GenericEventMessage<T> : MessageDecorator<T>, IEventMessage<T>
        where T : class
    {
        protected CachingSupplier<InternalDateTimeOffset> TimestampSupplier;

        /// <summary>
        /// {@link Clock} instance used to set the time on new events. To fix the time while testing set this value to a
        /// constant value.
        /// </summary>
        public static readonly InternalDateTimeOffset Clock = new InternalDateTimeOffset(DateTimeOffset.UtcNow);

        /// <summary>
        /// Returns the given event as an EventMessage. If {@code event} already implements EventMessage, it is
        /// returned as-is. If it is a Message, a new EventMessage will be created using the payload and meta data of the
        /// given message. Otherwise, the given {@code event} is wrapped into a GenericEventMessage as its payload.
        /// <param name="event">the event to wrap as EventMessage</param>
        /// <typeparam name="TR">The generic type of the expected payload of the resulting object</typeparam>
        /// <return>an EventMessage containing given {@code event} as payload, or {@code event} if it already implements EventMessage.</return>
        /// </summary>
        public static IEventMessage<TR> AsEventMessage<TR>(object @event)
            where TR : class
        {
            if (@event is IEventMessage<TR> eventMessage)
            {
                return eventMessage;
            }

            if (@event is IMessage<TR> message)
            {
                return new GenericEventMessage<TR>(message, Clock);
            }

            return new GenericEventMessage<TR>(new GenericMessage<TR>((TR) @event), Clock);
        }

        /// <summary>
        /// Creates a GenericEventMessage with given {@code payload}, and an empty MetaData.
        /// </summary>
        /// <param name="payload">The payload for the message</param>
        public GenericEventMessage(T? payload) : this(payload, MetaData.EmptyInstance)
        {
        }


        /// <summary>
        /// Creates a GenericEventMessage with given {@code payload} and given {@code metaData}.
        /// </summary>
        /// <param name="payload">The payload of the EventMessage</param>
        /// <param name="metaData">The MetaData for the EventMessage</param>
        public GenericEventMessage(T? payload, IImmutableDictionary<string, object> metaData)
            : this(new GenericMessage<T>(payload, metaData), Clock)
        {
        }

        public GenericEventMessage(string identifier, T payload, IImmutableDictionary<string, object> metaData,
            InternalDateTimeOffset timestamp) :
            this(new GenericMessage<T>(identifier, payload, metaData), timestamp)
        {
        }

        public GenericEventMessage(IMessage<T> @delegate, CachingSupplier<InternalDateTimeOffset> timestampSupplier) :
            base(@delegate)
        {
            TimestampSupplier = timestampSupplier;
        }


        protected GenericEventMessage(IMessage<T> @delegate, InternalDateTimeOffset timestamp) : this(@delegate,
            CachingSupplier<InternalDateTimeOffset>.Of(timestamp))
        {
        }

        public InternalDateTimeOffset? GetTimestamp()
        {
            return TimestampSupplier.Get();
        }

        public IEventMessage<T> WithMetaData(IImmutableDictionary<string, object> metaData)
        {
            return GetMetaData().Equals(metaData) ? this : new GenericEventMessage<T>(Delegate().WithMetaData(metaData), TimestampSupplier);
        }

        public IEventMessage<T> AndMetaData(IImmutableDictionary<string, object> metaData)
        {
            if (metaData.Count == 0 || GetMetaData().Equals(metaData))
            {
                return this;
            }

            return new GenericEventMessage<T>(Delegate().AndMetaData(metaData), TimestampSupplier);
        }

        protected override void DescribeTo(StringBuilder stringBuilder)
        {
            base.DescribeTo(stringBuilder);
            stringBuilder.Append(", timestamp='").Append(GetTimestamp()!);
        }
    }
}