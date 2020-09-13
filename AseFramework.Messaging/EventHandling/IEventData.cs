using Ase.Messaging.Common.Wrapper;
using Ase.Messaging.Serialization;

namespace Ase.Messaging.EventHandling
{
    /// <summary>
    /// Interface describing the properties of serialized Event Messages. Event Storage Engine implementations should have
    /// their storage entries implement this interface.
    /// </summary>
    /// <typeparam name="T">The content type of the serialized data</typeparam>
    public interface IEventData<T>
    {
        /// <summary>
        /// Returns the identifier of the serialized event.
        /// </summary>
        /// <returns>the identifier of the serialized event</returns>
        string GetEventIdentifier();

        /// <summary>
        /// Returns the timestamp at which the event was first created.
        /// </summary>
        /// <returns>the timestamp at which the event was first created</returns>
        InternalDateTimeOffset GetTimestamp();

        /// <summary>
        /// Returns the serialized data of the MetaData of the serialized Event.
        /// </summary>
        /// <returns>the serialized data of the MetaData of the serialized Event</returns>
        ISerializedObject<T> GetMetaData();

        /// <summary>
        /// Returns the serialized data of the Event Message's payload.
        /// </summary>
        /// <returns>the serialized data of the Event Message's payload</returns>
        ISerializedObject<T> GetPayload();
        
    }
}