namespace Ase.Messaging.EventHandling
{
    /// <summary>
    /// Tag interface identifying a token that is used to identify the position of an event in an event stream. Event
    /// processors use this token to keep track of the events they have processed and still need to process.
    /// </summary>
    public interface ITrackingToken
    {
        /// <summary>
        /// Returns a token that represents the lower bound between this and the {@code other} token. Effectively, the
        /// returned token will cause messages not received by both this and the {@code other} token to be redelivered.
        /// </summary>
        /// <param name="other">The token to compare to this one</param>
        /// <returns>The token representing the lower bound of the two</returns>
        ITrackingToken LowerBound(ITrackingToken other);

        /// <summary>
        /// Returns the token that represents the furthest possible position in a stream that either this token or the given
        /// {@code other} represents. Effectively, this means this token will only deliver messages that neither this, nor
        /// the other have been received.
        /// </summary>
        /// <param name="other">The token to compare this token to</param>
        /// <returns>a token that represents the furthest position of this or the other stream</returns>
        ITrackingToken UpperBound(ITrackingToken other);

        /// <summary>
        /// Indicates whether this token covers the {@code other} token completely. That means that this token represents a
        /// position in a stream that has received all of the messages that a stream represented by the {@code other} token
        /// has received.
        /// <p>
        /// Note that this operation is only safe when comparing tokens obtained from messages from the same
        /// {@link org.axonframework.messaging.StreamableMessageSource}.
        /// </summary>
        /// <param name="other">The token to compare to this one</param>
        /// <returns>if this token covers the other, otherwise {@code false}</returns>
        bool Covers(ITrackingToken other);

        /// <summary>
        /// Return the estimated relative position this token represents.
        /// In case no estimation can be given an {@code OptionalLong.empty()} will be returned.
        /// </summary>
        /// <returns>the estimated relative position of this token</returns>
        long? Position()
        {
            return default;
        }
    }
}