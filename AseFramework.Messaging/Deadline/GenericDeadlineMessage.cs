using System;
using System.Collections.Immutable;
using Ase.Messaging.Common.Wrapper;
using Ase.Messaging.EventHandling;
using Ase.Messaging.Messaging;
using Ase.Messaging.Serialization;

namespace Ase.Messaging.Deadline
{
    /// <summary>
    /// Generic implementation of the {@link DeadlineMessage}.
    /// </summary>
    /// <typeparam name="T">The type of payload contained in this Message</typeparam>
    public class GenericDeadlineMessage<T> : GenericEventMessage<T>, IDeadlineMessage<T>
        where T : class
    {
        private readonly string _deadlineName;


        /// <summary>
        /// Returns the given {@code deadlineName} and {@code messageOrPayload} as a DeadlineMessage. If the
        /// {@code messageOrPayload} parameter is of type {@link Message}, a new DeadlineMessage will be created using the
        /// payload and meta data of the given deadline.
        /// Otherwise, the given {@code messageOrPayload} is wrapped into a GenericDeadlineMessage as its payload.
        /// </summary>
        /// <param name="deadlineName">A {@link String} denoting the deadline's name</param>
        /// <param name="messageOrPayload">A {@link Message} or payload to wrap as a DeadlineMessage</param>
        /// <typeparam name="T">The generic type of the expected payload of the resulting object</typeparam>
        /// <returns>a DeadlineMessage using the {@code deadlineName} as its deadline name and containing the given
        /// {@code messageOrPayload} as the payload</returns>
        [Obsolete(
            message:
            "Use {@link #asDeadlineMessage(String, Object, Instant)} instead, as it sets the timestamp of the")]
        public static IDeadlineMessage<T> AsDeadlineMessage(string deadlineName, object messageOrPayload)
        {
            return AsDeadlineMessage(deadlineName, messageOrPayload, Clock);
        }

        /// <summary>
        /// Returns the given {@code deadlineName} and {@code messageOrPayload} as a DeadlineMessage which expires at the
        /// given {@code expiryTime}. If the {@code messageOrPayload} parameter is of type {@link Message}, a new
        /// {@code DeadlineMessage} instance will be created using the payload and meta data of the given message.
        /// Otherwise, the given {@code messageOrPayload} is wrapped into a {@code GenericDeadlineMessage} as its payload.
        /// </summary>
        /// <param name="deadlineName">A {@link String} denoting the deadline's name</param>
        /// <param name="messageOrPayload">A {@link Message} or payload to wrap as a DeadlineMessage</param>
        /// <param name="expiryTime">The timestamp at which the deadline expires</param>
        /// <typeparam name="T">The generic type of the expected payload of the resulting object</typeparam>
        /// <returns>a DeadlineMessage using the {@code deadlineName} as its deadline name and containing the given
        /// {@code messageOrPayload} as the payload</returns>
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

        /// <summary>
        /// Instantiate a GenericDeadlineMessage with the given {@code deadlineName}, a {@code null} payload and en empty
        /// {@link MetaData}.
        /// </summary>
        /// <param name="deadlineName">A {@link String} denoting the deadline's name</param>
        public GenericDeadlineMessage(string deadlineName) : this(deadlineName, null)
        {
        }

        /// <summary>
        /// Instantiate a GenericDeadlineMessage with the given {@code deadlineName}, a {@code payload} of type {@code T}
        /// and en empty {@link MetaData}.
        /// </summary>
        /// <param name="deadlineName">A {@link String} denoting the deadline's name</param>
        /// <param name="payload">The payload of type {@code T} for the DeadlineMessage</param>
        public GenericDeadlineMessage(string deadlineName, T? payload) : this(deadlineName, payload,
            MetaData.EmptyInstance)
        {
        }

        /// <summary>
        /// Instantiate a GenericDeadlineMessage with the given {@code deadlineName}, a {@code payload} of type {@code T}
        /// and the given {@code metaData}.
        /// </summary>
        /// <param name="deadlineName">A {@link String} denoting the deadline's name</param>
        /// <param name="payload">The payload of the Message</param>
        /// <param name="metaData">The MetaData of the Message</param>
        public GenericDeadlineMessage(string deadlineName, T? payload, IImmutableDictionary<string, object> metaData) :
            base(payload, metaData)
        {
            _deadlineName = deadlineName;
        }

        /// <summary>
        /// Constructor to reconstructs a DeadlineMessage using existing data.
        /// </summary>
        /// <param name="deadlineName">A {@link String} denoting the deadline's name</param>
        /// <param name="identifier">The identifier of type {@link String} for the Message</param>
        /// <param name="payload">The payload of type {@code T} for the Message</param>
        /// <param name="metaData">The {@link MetaData} of the Message</param>
        /// <param name="timestamp">An {@link Instant} timestamp of the Message creation</param>
        public GenericDeadlineMessage(string deadlineName, string identifier, T payload,
            IImmutableDictionary<string, object> metaData, InternalDateTimeOffset timestamp) : base(identifier, payload,
            metaData, timestamp)
        {
            _deadlineName = deadlineName;
        }

        /// <summary>
        /// Constructor to reconstruct a DeadlineMessage using existing data. The timestamp of the event is supplied lazily
        /// to prevent unnecessary deserialization of the timestamp.
        /// </summary>
        /// <param name="deadlineName">A {@link String} denoting the deadline's name</param>
        /// <param name="delegate">The identifier of type {@link String} for the Message</param>
        /// <param name="timestampSupplier">{@link Supplier} for the timestamp of the Message creation</param>
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
        
        protected override string DescribeType() {
            return "GenericDeadlineMessage";
        }
    }
}