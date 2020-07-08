using System;
using System.Collections.Generic;

namespace Ase.Messaging.Messaging
{
    /// <summary>
    /// Abstract base class for Messages.
    /// </summary>
    public abstract class AbstractMessage<T>: IMessage<T>
    {
        private readonly string _identifier;

        /// <summary>
        /// Initializes a new message with given identifier.
        /// </summary>
        /// <param name="identifier">the message identifier</param>
        protected AbstractMessage(string identifier)
        {
            _identifier = identifier;
        }
        
        public string GetIdentifier()
        {
            return this._identifier;
        }

        public abstract MetaData GetMetaData();

        public abstract T GetPayload();

        public abstract Type GetPayloadType();

        
        /// <summary>
        /// Returns a new message instance with the same payload and properties as this message but given <code>metaData</code>.
        /// </summary>
        /// <param name="metaData">The metadata in the new message</param>
        /// <returns>a copy of this instance with given metadata</returns>
        protected abstract IMessage<T> WithMetaData(MetaData metaData);
        
        public IMessage<T> AndMetaData(IReadOnlyDictionary<string, object> metaData)
        {
            if (metaData?.Count == 0)
            {
                return this;
            }
            return WithMetaData(GetMetaData().MergedWith(metaData));
        }

        public IMessage<T> WithMetaData(IReadOnlyDictionary<string, object> metaData)
        {
            if (GetMetaData().Equals(metaData))
            {
                return this;
            }

            return WithMetaData(MetaData.From(metaData));
        }
        
    }
}