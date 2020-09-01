using System;
using System.Collections.ObjectModel;
using Ase.Messaging.Messaging;

namespace AseFramework.Messaging.EventHandling
{
    /// <summary>
    /// Represents a Message wrapping an Event, which is represented by its payload. An Event is a representation of an
    /// occurrence of an event (i.e. anything that happened any might be of importance to any other component) in the
    /// application. It contains the data relevant for components that need to act based on that event.
    /// </summary>
    /// <typeparam name="T">The type of payload contained in this Message</typeparam>
    public interface IEventMessage<T> : IMessage<T>
    where T : class
    {

        /// <summary>
        /// Returns the identifier of this event. The identifier is used to define the uniqueness of an event. Two events
        /// may contain similar (or equal) payload and timestamp, if the EventIdentifiers are different, they both represent
        /// a different occurrence of an Event. If two messages have the same identifier, they both represent the same
        /// unique occurrence of an event, even though the resulting view may be different. You may not assume two messages
        /// are equal (i.e. interchangeable) if their identifier is equal.
        /// <p/>
        /// For example, an AddressChangeEvent may occur twice for the same Event, because someone moved back to the
        /// previous address. In that case, the Event payload is equal for both EventMessage instances, but the Event
        /// Identifier is different for both.
        /// </summary>
        /// <returns>the identifier of this event.</returns>
        new string GetIdentifier();

        /// <summary>
        /// Returns the timestamp of this event. The timestamp is set to the date and time the event was reported.
        /// </summary>
        /// <returns>the timestamp of this event.</returns>
        DateTime getTimestamp();

        /// <summary>
        /// Returns a copy of this EventMessage with the given {@code metaData}. The payload, {@link #getTimestamp()
        /// Timestamp} and {@link #getIdentifier() Identifier} remain unchanged.
        /// </summary>
        /// <param name="metaData">The new MetaData for the Message</param>
        /// <returns>a copy of this message with the given MetaData</returns>
        IEventMessage<T> WithMetaData(ReadOnlyDictionary<string, object> metaData);

        /// <summary>
        /// Returns a copy of this EventMessage with it MetaData merged with the given {@code metaData}. The payload,
        /// {@link #getTimestamp() Timestamp} and {@link #getIdentifier() Identifier} remain unchanged.
        /// </summary>
        /// <param name="metaData">metaData The MetaData to merge with</param>
        /// <returns>a copy of this message with the given MetaData</returns>
        IEventMessage<T> AndMetaData(ReadOnlyDictionary<string, object> metaData);

    }
}