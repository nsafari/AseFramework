namespace Ase.Messaging.EventHandling
{
    public interface IWrappedToken : ITrackingToken
    {
        static ITrackingToken UnwrapLowerBound(ITrackingToken token)
        {
            return token is IWrappedToken wrappedToken ? wrappedToken.LowerBound() : token;
        }

        static ITrackingToken UnwrapUpperBound(ITrackingToken token)
        {
            return token is IWrappedToken wrappedToken ? ((IWrappedToken) token).UpperBound() : token;
        }

        static R? Unwrap<R>(ITrackingToken token)
            where R : class, ITrackingToken
        {
            return token switch
            {
                IWrappedToken wrappedToken => wrappedToken.Unwrap<R>(),
                R trackingToken => trackingToken,
                _ => null
            };
        }

        ITrackingToken AdvancedTo(ITrackingToken newToken);

        ITrackingToken LowerBound();

        ITrackingToken UpperBound();

        R? Unwrap<R>()
            where R : class, ITrackingToken;
    }
}