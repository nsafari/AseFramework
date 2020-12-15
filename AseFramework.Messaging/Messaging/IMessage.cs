using System;
using System.Collections.Generic;
using Ase.Messaging.Serialization;

namespace Ase.Messaging.Messaging
{
    /// <summary>
    /// Representation of a Message, containing a Payload and MetaData. Typical examples of Messages are Commands and
    /// Events.
    /// <para></para>
    /// Instead of implementing <code>Message</code> directly, consider implementing
    /// <see cref="org.axonframework.commandhandling.CommandMessage"/> or <see cref="EventMessage"/> instead.
    /// <typeparam name="T">The type of payload contained in this Message</typeparam>
    /// </summary>
    public interface IMessage<out T>
        where T : class
    {
        /// <summary>
        /// Returns the identifier of this message. Two messages with the same identifiers should be interpreted as
        /// different representations of the same conceptual message. In such case, the meta-data may be different for both
        /// representations. The payload <em>may</em> be identical.
        /// <returns>the unique identifier of this message</returns>
        /// </summary>
        string GetIdentifier();

        /// <summary>
        /// Returns the meta data for this event. This meta data is a collection of key-value pairs, where the key is a
        /// String, and the value is a serializable object.
        /// <returns>the meta data for this event</returns>
        /// </summary>
        MetaData GetMetaData();

        /// <summary>
        /// Returns the payload of this Event. The payload is the application-specific information.
        /// <returns>the payload of this Event</returns>
        /// </summary>
        T? GetPayload();

        /// <summary>
        /// Returns the type of the payload.
        /// <para></para>
        /// Is semantically equal to <code>getPayload().getClass()</code>, but allows implementations to optimize by using
        /// lazy loading or deserialization.
        /// <returns>the type of payload.</returns>
        /// </summary>
        Type GetPayloadType();

        /// <summary>
        /// Returns a copy of this Message with the given <code>metaData</code>. The payload remains unchanged.
        /// <para></para>
        /// While the implementation returned may be different than the implementation of <code>this</code>, implementations
        /// must take special care in returning the same type of Message (e.g. EventMessage, DomainEventMessage) to prevent
        /// errors further downstream.
        /// <param name="metaData">The new MetaData for the Message</param>
        /// <returns>a copy of this message with the given MetaData</returns>
        /// </summary>
        IMessage<T> WithMetaData(IReadOnlyDictionary<string, object> metaData);


        /// <summary>
        /// Returns a copy of this Message with it MetaData merged with the given <code>metaData</code>. The payload
        /// remains unchanged.
        /// </summary>
        /// <param name="metaData">The MetaData to merge with</param>
        /// <returns>a copy of this message with the given MetaData</returns>
        IMessage<T> AndMetaData(IReadOnlyDictionary<string, object> metaData);


        /// <summary>
        /// Serialize the payload of this message to the {@code expectedRepresentation} using given {@code serializer}. This
        /// method <em>should</em> return the same SerializedObject instance when invoked multiple times using the same
        /// serializer.
        /// </summary>
        /// <param name="serializer">The serializer to serialize payload with</param>
        /// <param name="expectedRepresentation">The type of data to serialize to</param>
        /// <typeparam name="R">The type of the serialized data</typeparam>
        /// <returns>a SerializedObject containing the serialized representation of the message's payload</returns>
        ISerializedObject<R> SerializePayload<R>(ISerializer serializer, Type expectedRepresentation)
        {
            return serializer.Serialize<R>(GetPayload()!, expectedRepresentation);
        }

        /// <summary>
        /// Serialize the meta data of this message to the {@code expectedRepresentation} using given {@code serializer}.
        /// This method <em>should</em> return the same SerializedObject instance when invoked multiple times using the same
        /// serializer.
        /// </summary>
        /// <param name="serializer">The serializer to serialize meta data with</param>
        /// <param name="expectedRepresentation">The type of data to serialize to</param>
        /// <typeparam name="R">The type of the serialized data</typeparam>
        /// <returns>a SerializedObject containing the serialized representation of the message's meta data</returns>
        ISerializedObject<R> SerializeMetaData<R>(ISerializer serializer, Type expectedRepresentation) {
            return serializer.Serialize<R>(GetMetaData(), expectedRepresentation);
        }

    }
}