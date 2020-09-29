using Ase.Messaging.EventHandling;
using Ase.Messaging.Serialization;

namespace AseFramework.EventSourcing.EventStore.jpa
{
    /// <summary>
    /// Default implementation of an event entry containing a serialized snapshot of an aggregate. This implementation is
    /// used by the {@link JpaEventStorageEngine} to store snapshot events. Event payload and metadata are serialized to a
    /// byte array.
    /// </summary>
    public class SnapshotEventEntry : AbstractSnapshotEventEntry<byte[]>
    {
        /// <summary>
        /// Construct a new default snapshot event entry from an aggregate. The snapshot payload and metadata will be
        /// serialized to a byte array.
        /// <p>
        /// The given {@code serializer} will be used to serialize the payload and metadata in the given {@code
        /// eventMessage}. The type of the serialized data will be the same as the given {@code contentType}.
        /// </summary>
        /// <param name="eventMessage"></param>
        /// <param name="serializer"></param>
        public SnapshotEventEntry(IDomainEventMessage<byte[]> eventMessage, ISerializer serializer)
            : base(eventMessage, serializer, typeof(byte[]))
        {
        }
        
        protected SnapshotEventEntry() {
        }
    }
}