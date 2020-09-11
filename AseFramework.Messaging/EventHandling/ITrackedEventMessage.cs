namespace Ase.Messaging.EventHandling
{
    /// <summary>
    /// Represents an {@link EventMessage} containing a {@link TrackingToken}. The tracking token can be used by  {@link
    /// EventProcessor event processors} to keep track of which events it has processed.
    /// </summary>
    /// <typeparam name="T">The type of payload contained in this Message</typeparam>
    public interface ITrackedEventMessage<T> : IEventMessage<T> 
        where T : class
    {
        /// <summary>
        /// Returns the {@link TrackingToken} of the event message.
        /// </summary>
        /// <returns>the tracking token of the event</returns>
        ITrackingToken TrackingToken();

        /// <summary>
        /// Creates a copy of this message with the given {@code trackingToken} to replace the one in this message.
        /// <p>
        /// This method is useful in case streams are modified (combined, split), and the tokens of the combined stream are
        /// different than the originating stream.
        /// </summary>
        /// <param name="trackingToken">The tracking token to replace</param>
        /// <returns>a new instance of a message with a different tracking token</returns>
        ITrackedEventMessage<T> WithTrackingToken(ITrackingToken trackingToken);

    }
}