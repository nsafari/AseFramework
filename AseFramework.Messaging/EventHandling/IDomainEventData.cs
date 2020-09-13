namespace Ase.Messaging.EventHandling
{
    /// <summary>
    /// Interface describing the properties of serialized Domain Event Messages. Event Store implementations should have
    /// their storage entries implement this interface.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDomainEventData<T> : IEventData<T>
    {
        /// <summary>
        /// Returns the type identifier of the aggregate.
        /// </summary>
        /// <returns>the type identifier of the aggregate.</returns>
        string GetType();

        /// <summary>
        /// Returns the Identifier of the Aggregate to which the Event was applied.
        /// </summary>
        /// <returns>the Identifier of the Aggregate to which the Event was applied</returns>
        string GetAggregateIdentifier();

        /// <summary>
        /// Returns the sequence number of the event in the aggregate.
        /// </summary>
        /// <returns>the sequence number of the event in the aggregate</returns>
        long GetSequenceNumber();
        
    }
}