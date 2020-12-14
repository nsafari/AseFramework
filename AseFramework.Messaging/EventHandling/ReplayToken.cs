using Ase.Messaging.Messaging;

namespace Ase.Messaging.EventHandling
{
    public class ReplayToken :  IWrappedToken
    {
        private readonly ITrackingToken _tokenAtReset;
        private readonly ITrackingToken? _currentToken;
        private readonly bool _lastMessageWasReplay;

        public static ITrackingToken CreateReplayToken(ITrackingToken tokenAtReset, ITrackingToken? startPosition)
        {
            while (true)
            {
                if (tokenAtReset is ReplayToken token)
                {
                    tokenAtReset = token._tokenAtReset;
                    continue;
                }

                return startPosition != null && startPosition.Covers(IWrappedToken.UnwrapLowerBound(tokenAtReset)) ? 
                    startPosition : new ReplayToken(tokenAtReset, startPosition);
            }
        }

        public static ITrackingToken CreateReplayToken(ITrackingToken tokenAtReset)
        {
            return CreateReplayToken(tokenAtReset, null);
        }

        public static bool IsReplay(IMessage<object> message)
        {
            return message is ITrackedEventMessage<object>
                   && IsReplay(((ITrackedEventMessage<object>) message).TrackingToken());
        }

        public static bool IsReplay(ITrackingToken trackingToken)
        {
            return IWrappedToken.Unwrap<ReplayToken>(trackingToken)?.IsReplay() ?? false;
        }

        public static long? GetTokenAtReset(ITrackingToken trackingToken)
        {
            return IWrappedToken.Unwrap<ReplayToken>(trackingToken)?.GetTokenAtReset().Position();
        }

        public ReplayToken(ITrackingToken tokenAtReset) : this(tokenAtReset, null)
        {
        }

        public ReplayToken(ITrackingToken tokenAtReset, ITrackingToken? newRedeliveryToken) : this(tokenAtReset,
            newRedeliveryToken, true)
        {
        }

        private ReplayToken(ITrackingToken tokenAtReset,
            ITrackingToken? newRedeliveryToken,
            bool lastMessageWasReplay)
        {
            _tokenAtReset = tokenAtReset;
            _currentToken = newRedeliveryToken;
            _lastMessageWasReplay = lastMessageWasReplay;
        }

        public ITrackingToken GetTokenAtReset()
        {
            return _tokenAtReset;
        }

        public ITrackingToken? GetCurrentToken()
        {
            return _currentToken;
        }

        public ITrackingToken AdvancedTo(ITrackingToken newToken)
        {
            if ((newToken.Covers(IWrappedToken.UnwrapUpperBound(_tokenAtReset))
                 && !_tokenAtReset.Covers(IWrappedToken.UnwrapLowerBound(newToken))))
            {
                // we're done replaying
                // if the token at reset was a wrapped token itself, we'll need to use that one to maintain progress.
                return _tokenAtReset is IWrappedToken wrappedToken ? wrappedToken.AdvancedTo(newToken) : newToken;
            }

            if (_tokenAtReset.Covers(IWrappedToken.UnwrapLowerBound(newToken)))
            {
                // we're still well behind
                return new ReplayToken(_tokenAtReset, newToken, true);
            }

            // we're getting an event that we didn't have before, but we haven't finished replaying either
            if (_tokenAtReset is IWrappedToken token)
            {
                return new ReplayToken(token.UpperBound(newToken),
                    token.AdvancedTo(newToken), false);
            }

            return new ReplayToken(_tokenAtReset.UpperBound(newToken), newToken, false);
        }

        public ITrackingToken LowerBound(ITrackingToken other)
        {
            if (other is ReplayToken token)
            {
                return new ReplayToken(this, token._currentToken);
            }

            return new ReplayToken(this, other);
        }

        public ITrackingToken UpperBound(ITrackingToken other)
        {
            return AdvancedTo(other);
        }

        public bool Covers(ITrackingToken? other)
        {
            if (other is ReplayToken token)
            {
                return _currentToken != null && _currentToken.Covers(token._currentToken);
            }

            return _currentToken != null && _currentToken.Covers(other);
        }

        private bool IsReplay()
        {
            return _lastMessageWasReplay;
        }

        public ITrackingToken LowerBound()
        {
            return IWrappedToken.UnwrapLowerBound(_currentToken);
        }

        public ITrackingToken UpperBound()
        {
            return IWrappedToken.UnwrapUpperBound(_currentToken);
        }

        public R? Unwrap<R>() where R : class, ITrackingToken
        {
            return this as R ?? IWrappedToken.Unwrap<R>(_currentToken);
        }

        public override bool Equals(object? obj)
        {
            if (this == obj) {
                return true;
            }

            if (obj == null) {
                return false;
            }

            ReplayToken that = (ReplayToken) obj;
            return Equals(_tokenAtReset, that._tokenAtReset) &&
                   Equals(_currentToken, that._currentToken);
        }

        public override int GetHashCode()
        {
            return System.HashCode.Combine(_tokenAtReset, _currentToken);
        }
        
        public override string ToString() {
            return "ReplayToken{" +
                   "currentToken=" + _currentToken +
                   ", tokenAtReset=" + _tokenAtReset +
                   '}';
        }
        
        public long? Position() {
            return _currentToken?.Position();
        }
    }
}