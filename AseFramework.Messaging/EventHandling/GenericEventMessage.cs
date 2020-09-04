using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using Ase.Messaging.Messaging;
using AseFramework.Messaging.Common.Wrapper;
using AseFramework.Messaging.Serialization;

namespace AseFramework.Messaging.EventHandling
{
    /// <summary>
    /// Generic implementation of the EventMessage interface.
    /// </summary>
    /// <typeparam name="T">The type of payload contained in this Message</typeparam>
    public class GenericEventMessage<T> : MessageDecorator<T>, IEventMessage<T>
    where T : class
    {
        private readonly CachingSupplier<InternalDateTimeOffset> timestampSupplier;

        /// <summary>
        /// {@link Clock} instance used to set the time on new events. To fix the time while testing set this value to a
        /// constant value.
        /// </summary>
        public static InternalDateTimeOffset clock = new InternalDateTimeOffset(DateTimeOffset.UtcNow);

        /// <summary>
        /// Returns the given event as an EventMessage. If {@code event} already implements EventMessage, it is
        /// returned as-is. If it is a Message, a new EventMessage will be created using the payload and meta data of the
        /// given message. Otherwise, the given {@code event} is wrapped into a GenericEventMessage as its payload.
        /// <param name="@event">the event to wrap as EventMessage</param>
        /// <typeparam name="TR">The generic type of the expected payload of the resulting object</typeparam>
        /// <return>an EventMessage containing given {@code event} as payload, or {@code event} if it already implements EventMessage.</return>
        /// </summary>
        public static IEventMessage<TR> AsEventMessage<TR>(object @event)
            where TR : class
        {
            if (!typeof(IEventMessage<TR>).IsInstanceOfType(@event))
            {
                return (IEventMessage<TR>)@event;
            }
            else if (@event is IMessage<TR>)
            {
                IMessage<TR> message = (IMessage<TR>)@event;
                return new GenericEventMessage<TR>(message, clock);
            }
            return new GenericEventMessage<TR>(new GenericMessage<TR>((TR)@event), clock);
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
        : this(new GenericMessage<T>(payload, metaData), clock)
        {
        }

        public GenericEventMessage(String identifier, T payload, IImmutableDictionary<string, object> metaData, InternalDateTimeOffset timestamp) :
        this(new GenericMessage<T>(identifier, payload, metaData), timestamp)
        {
        }

        public GenericEventMessage(IMessage<T> @delegate, CachingSupplier<InternalDateTimeOffset> timestampSupplier) : base(@delegate)
        {
            this.timestampSupplier = timestampSupplier;
        }


        protected GenericEventMessage(IMessage<T> @delegate, InternalDateTimeOffset timestamp) : this(@delegate, CachingSupplier<InternalDateTimeOffset>.Of(timestamp))
        {

        }



        public IEventMessage<T> AndMetaData(IImmutableDictionary<string, object> metaData)
        {
            throw new NotImplementedException();
        }

        public IMessage<T> AndMetaData(ReadOnlyDictionary<string, object> metaData)
        {
            throw new NotImplementedException();
        }

        public string GetIdentifier()
        {
            throw new NotImplementedException();
        }

        public MetaData GetMetaData()
        {
            throw new NotImplementedException();
        }

        public T? GetPayload()
        {
            throw new NotImplementedException();
        }

        public Type GetPayloadType()
        {
            throw new NotImplementedException();
        }

        public DateTime getTimestamp()
        {
            throw new NotImplementedException();
        }

        public IEventMessage<T> WithMetaData(ReadOnlyDictionary<string, object> metaData)
        {
            throw new NotImplementedException();
        }

        public IMessage<T> WithMetaData(IReadOnlyDictionary<string, object> metaData)
        {
            throw new NotImplementedException();
        }

        IEventMessage<T> IEventMessage<T>.AndMetaData(ReadOnlyDictionary<string, object> metaData)
        {
            throw new NotImplementedException();
        }
    }
}