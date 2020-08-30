using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using Ase.Messaging.Messaging;

namespace AseFramework.Messaging.EventHandling
{
    /// <summary>
    /// Generic implementation of the EventMessage interface.
    /// </summary>
    /// <typeparam name="T">The type of payload contained in this Message</typeparam>
    public class GenericEventMessage<T> : MessageDecorator<T>, IEventMessage<T>
    where T : class
    {
        private readonly Action<DateTime> timestampSupplier;

        /// <summary>
        /// {@link Clock} instance used to set the time on new events. To fix the time while testing set this value to a
        /// constant value.
        /// </summary>
        public static DateTimeOffset clock = DateTimeOffset.UtcNow;

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
                return new GenericEventMessage<TR>(message, clock.ToUnixTimeMilliseconds);
            }
            return new GenericEventMessage<TR>(new GenericMessage<TR>((TR)@event), clock.ToUnixTimeMilliseconds);
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
        : this(new GenericMessage<T>(payload, metaData), clock.ToUnixTimeMilliseconds())
        {
        }

        public GenericEventMessage(String identifier, T payload, IImmutableDictionary<string, object> metaData, DateTimeOffset timestamp) :
        this(new GenericMessage<T>(identifier, payload, metaData), timestamp)
        {
        }

        public GenericEventMessage(IMessage<T>? @delegate, Action<DateTimeOffset> timestampSupplier) : base(@delegate)
        {

            this.timestampSupplier = CachingSupplier.of(timestampSupplier);
        }


        protected GenericEventMessage(IMessage<T>? @delegate, DateTimeOffset timestamp) : this(@delegate, CachingSupplier.of(timestamp))
        {

        }



        public IEventMessage<T> AndMetaData(IImmutableDictionary<string, object> metaData)
        {
            throw new NotImplementedException();
        }

        public IMessage<T> AndMetaData(IIImmutableDictionary<string, object> metaData)
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
    }
}