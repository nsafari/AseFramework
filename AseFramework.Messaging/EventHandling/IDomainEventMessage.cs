using System.Collections.Immutable;

namespace Ase.Messaging.EventHandling
{
    /// <summary>
    /// Represents a Message that wraps a DomainEvent and an Event representing an important change in the Domain. In
    /// contrast to a regular EventMessage, a DomainEventMessages contains the identifier of the Aggregate that reported it.
    /// The DomainEventMessage's sequence number allows messages to be placed in their order of generation.
    /// </summary>
    /// <typeparam name="T">The type of payload contained in this Message</typeparam>
    public interface IDomainEventMessage<out T> : IEventMessage<T>
        where T : class
    {
        
        /// <summary>
        /// Returns the sequence number that allows DomainEvents originating from the same Aggregate to be placed in the
        /// order of generation.
        /// </summary>
        /// <returns>the sequence number of this Event</returns>
        long GetSequenceNumber();

        /// <summary>
        /// Returns the identifier of the Aggregate that generated this DomainEvent. Note that the value returned does not
        /// necessarily have to be the same instance that was provided at creation time. It is possible that (due to
        /// serialization, for example) the value returned here has a different structure.
        /// </summary>
        /// <returns>the identifier of the Aggregate that generated this DomainEvent</returns>
        string GetAggregateIdentifier();

        /// <summary>
        /// Returns the type of the Aggregate that generated this DomainEvent. By default this is equal to the simple class
        /// name of the aggregate root.
        /// </summary>
        /// <returns>the type of the Aggregate that generated this DomainEvent</returns>
        string Type();

        /// <summary>
        /// Returns a copy of this DomainEventMessage with the given {@code metaData}. The payload, {@link #getTimestamp()
        /// Timestamp} and {@link #getIdentifier() EventIdentifier}, as well as the {@link #getAggregateIdentifier()
        /// Aggregate Identifier} and {@link #getSequenceNumber() Sequence Number} remain unchanged.
        /// </summary>
        /// <param name="metaData">The new MetaData for the Message</param>
        /// <returns>a copy of this message with the given MetaData</returns>
        new IDomainEventMessage<T> WithMetaData(IImmutableDictionary<string, object> metaData);

        /// <summary>
        /// Returns a copy of this DomainEventMessage with its MetaData merged with the given {@code metaData}. The
        /// payload, {@link #getTimestamp() Timestamp} and {@link #getIdentifier() EventIdentifier}, as well as the
        /// {@link #getAggregateIdentifier() Aggregate Identifier} and {@link #getSequenceNumber() Sequence Number} remain
        /// unchanged.
        /// </summary>
        /// <param name="metaData">The MetaData to merge with</param>
        /// <returns>a copy of this message with the given MetaData</returns>
        new IDomainEventMessage<T> AndMetaData(IImmutableDictionary<string, object> metaData);

        
    }
}