using Ase.Messaging.EventHandling;
using Ase.Messaging.Serialization;

namespace AseFramework.EventSourcing.EventStore.jpa
{
    /// <summary>
    /// Default implementation of a tracked domain event entry. This implementation is used by the {@link
    /// JpaEventStorageEngine} to store events. Event payload and metadata are serialized to a byte array.
    /// </summary>
    // @Entity
    // @Table(indexes = @Index(columnList = "aggregateIdentifier,sequenceNumber", unique = true))
    public class DomainEventEntry : AbstractSequencedDomainEventEntry<byte[]>
    {
        
        /// <summary>
        /// Construct a new default domain event entry from a published domain event message to enable storing the event or
        /// sending it to a remote location. The event payload and metadata will be serialized to a byte array.
        /// <p>
        /// The given {@code serializer} will be used to serialize the payload and metadata in the given {@code eventMessage}.
        /// The type of the serialized data will be the same as the given {@code contentType}.
        /// </summary>
        /// <param name="eventMessage">The event message to convert to a serialized event entry</param>
        /// <param name="serializer">The serializer to convert the event</param>
        public DomainEventEntry(IDomainEventMessage<byte[]> eventMessage, ISerializer serializer) 
            : base(eventMessage, serializer, typeof(byte[]))
        {
        }
        
        protected DomainEventEntry() {
        }
    }
}