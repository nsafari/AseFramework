using System;
using System.Collections.Generic;

namespace Ase.Messaging.Messaging
{
    /// <summary>
    /// Representation of a Message, containing a Payload and MetaData. Typical examples of Messages are Commands and
    /// Events.
    /// <para></para>
    /// Instead of implementing <code>Message</code> directly, consider implementing
    /// <see cref="org.axonframework.commandhandling.CommandMessage"/> or <see cref="EventMessage"/> instead.
    /// <typeparam name="T">The type of payload contained in this Message</typeparam>
    /// @author nSafari
    /// @since 2.0
    /// </summary>
    public interface IMessage<T>
    {
        /// <summary>
        /// Returns the identifier of this message. Two messages with the same identifiers should be interpreted as
        /// different representations of the same conceptual message. In such case, the meta-data may be different for both
        /// representations. The payload <em>may</em> be identical.
        /// @return the unique identifier of this message
        /// </summary>
        String GetIdentifier();

        /// <summary>
        /// Returns the meta data for this event. This meta data is a collection of key-value pairs, where the key is a
        /// String, and the value is a serializable object.
        /// @return the meta data for this event
        /// </summary>
        MetaData GetMetaData();

        /// <summary>
        /// Returns the payload of this Event. The payload is the application-specific information.
        /// @return the payload of this Event
        /// </summary>
        T GetPayload();

        /// <summary>
        /// Returns the type of the payload.
        /// <para></para>
        /// Is semantically equal to <code>getPayload().getClass()</code>, but allows implementations to optimize by using
        /// lazy loading or deserialization.
        /// @return the type of payload.
        /// </summary>
        Type GetPayloadType();

        /// <summary>
        /// Returns a copy of this Message with the given <code>metaData</code>. The payload remains unchanged.
        /// <para></para>
        /// While the implementation returned may be different than the implementation of <code>this</code>, implementations
        /// must take special care in returning the same type of Message (e.g. EventMessage, DomainEventMessage) to prevent
        /// errors further downstream.
        /// @param metaData The new MetaData for the Message
        /// @return a copy of this message with the given MetaData
        /// </summary>
        IMessage<T> WithMetaData(IReadOnlyDictionary<string, object> metaData);
        
        
        /// <summary>
        /// Returns a copy of this Message with it MetaData merged with the given <code>metaData</code>. The payload
        /// remains unchanged.
        /// </summary>
        /// <param name="metaData">The MetaData to merge with</param>
        /// <returns>a copy of this message with the given MetaData</returns>
        IMessage<T> AndMetaData(IReadOnlyDictionary<string, object> metaData);

    }
}