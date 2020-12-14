namespace Ase.Messaging.EventHandling
{
    /// <summary>
    /// Interface marking a token that wraps another token. As certain implementations may depend on specific token types,
    /// Tokens that wrap another must provide a means to retrieve the original token.
    /// </summary>
    public interface IWrappedToken : ITrackingToken
    {
        /// <summary>
        /// Extracts a raw token describing the current processing position of the given {@code token}. If the given token
        /// is a wrapped token, it will be unwrapped until the raw token (as received from the event stream) is reached.
        /// <p>
        /// The returned token represents the minimal position described by the given token (which may express a range)
        /// </summary>
        /// <param name="token">The token to unwrap</param>
        /// <returns>the raw lower bound token described by given token</returns>
        static ITrackingToken UnwrapLowerBound(ITrackingToken? token)
        {
            return token is IWrappedToken wrappedToken ? wrappedToken.LowerBound() : token;
        }

        /// <summary>
        /// Extracts a raw token describing the current processing position of the given {@code token}. If the given token
        /// is a wrapped token, it will be unwrapped until the raw token (as received from the event stream) is reached.
        /// <p>
        /// The returned token represents the furthest position described by the given token (which may express a range)
        /// </summary>
        /// <param name="token">The token to unwrap</param>
        /// <returns>the raw upper bound token described by given token</returns>
        static ITrackingToken UnwrapUpperBound(ITrackingToken? token)
        {
            return token is IWrappedToken wrappedToken ? ((IWrappedToken) token).UpperBound() : token;
        }

        /// <summary>
        /// Unwrap the given {@code token} until a token of given {@code tokenType} is exposed. Returns an empty optional
        /// if the given {@code token} is not a WrappedToken instance, or if it does not wrap a token of expected
        /// {@code tokenType}.
        /// </summary>
        /// <param name="token">The token to unwrap</param>
        /// <typeparam name="R">The generic type of the token to reveal</typeparam>
        /// <returns>an optional with the unwrapped token, if found</returns>
        static R? Unwrap<R>(ITrackingToken? token)
            where R : class, ITrackingToken
        {
            return token switch
            {
                IWrappedToken wrappedToken => wrappedToken.Unwrap<R>(),
                R trackingToken => trackingToken,
                _ => null
            };
        }

        /// <summary>
        /// Advance this token to the given {@code newToken}.
        /// </summary>
        /// <param name="newToken">The token representing the position to advance to</param>
        /// <returns>a token representing the new position</returns>
        ITrackingToken AdvancedTo(ITrackingToken newToken);

        /// <summary>
        /// Returns the token representing the current position in the stream.
        /// </summary>
        /// <returns>the token representing the current position in the stream</returns>
        ITrackingToken LowerBound();

        /// <summary>
        /// Returns the token representing the furthest position in the stream described by this token.
        /// This is usually a position that has been (partially) processed before.
        /// </summary>
        /// <returns>the token representing the furthest position reached in the stream</returns>
        ITrackingToken UpperBound();

        /// <summary>
        /// Retrieve a token of given {@code tokenType} if it is wrapped by this token.
        /// </summary>
        /// <typeparam name="R">The type of token to unwrap to</typeparam>
        /// <returns>The generic type of the token to unwrap to</returns>
        R? Unwrap<R>()
            where R : class, ITrackingToken;
    }
}