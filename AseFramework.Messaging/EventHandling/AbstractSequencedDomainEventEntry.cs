using System;
using Ase.Messaging.Serialization;
using NHMA = NHibernate.Mapping.Attributes;

namespace Ase.Messaging.EventHandling
{
    public class AbstractSequencedDomainEventEntry<T> : AbstractDomainEventEntry<T>, IDomainEventData<T>
        where T : class
    {
        [NHMA.Id] [NHMA.Generator] private long globalIndex;


        /// <summary>
        /// Construct a new default domain event entry from a published domain event message to enable storing the event or
        /// sending it to a remote location. The event payload and metadata will be serialized to a byte array.
        /// <p>
        /// The given {@code serializer} will be used to serialize the payload and metadata in the given {@code
        /// eventMessage}. The type of the serialized data will be the same as the given {@code contentType}.
        /// </summary>
        /// <param name="eventMessage">The event message to convert to a serialized event entry</param>
        /// <param name="serializer">The serializer to convert the event</param>
        /// <param name="contentType">The data type of the payload and metadata after serialization</param>
        public AbstractSequencedDomainEventEntry(
            IDomainEventMessage<T> eventMessage,
            ISerializer serializer,
            Type contentType
        ) : base(eventMessage, serializer, contentType)
        {
            
        }
        
        /// <summary>
        /// Default constructor required by JPA
        /// Should be removed?
        /// </summary>
        protected AbstractSequencedDomainEventEntry() {
        }

    }
}