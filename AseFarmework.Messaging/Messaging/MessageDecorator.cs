using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Ase.Messaging.Messaging
{
    public abstract class MessageDecorator<T> : IMessage<T>
    {
        private readonly IMessage<T> _delegate;

        /// <summary>
        /// Initializes a new decorator with given {@code delegate} message. The decorator delegates to the delegate for
        /// the message's payload, metadata and identifier.
        /// </summary>
        /// <param name="delegate">the message delegate</param>
        protected MessageDecorator(IMessage<T> @delegate)
        {
            _delegate = @delegate;
        }

        public virtual string GetIdentifier()
        {
            return _delegate.GetIdentifier();
        }

        public virtual MetaData GetMetaData()
        {
            return _delegate.GetMetaData();
        }

        public virtual T GetPayload()
        {
            return _delegate.GetPayload();
        }

        public Type GetPayloadType()
        {
            return _delegate.GetPayloadType();
        }

        public virtual IMessage<T> WithMetaData(IReadOnlyDictionary<string, object> metaData)
        {
            return _delegate.WithMetaData(metaData);
        }

        public virtual IMessage<T> AndMetaData(IReadOnlyDictionary<string, object> metaData)
        {
            return _delegate.AndMetaData(metaData);
        }

        /// <summary>
        /// Returns the wrapped message delegate.
        /// </summary>
        /// <returns>the delegate message</returns>
        protected IMessage<T> Delegate() => _delegate;

        
        /// <summary>
        /// Describe the message specific properties to the given {@code stringBuilder}. Subclasses should override this
        /// method, calling the super method and appending their own properties to the end (or beginning).
        ///     <p>
        /// As convention, String values should be enclosed in single quotes, Objects in curly brackets and numeric values
        ///     may be appended without enclosing. All properties should be preceded by a comma when appending, or finish with a
        /// comma when prefixing values.
        /// </summary>
        /// <param name="stringBuilder">the builder to append data to</param>
        protected virtual void DescribeTo(StringBuilder stringBuilder) {
            stringBuilder.Append("payload={")
                .Append(GetPayload())
                .Append('}')
                .Append(", metadata={")
                .Append(GetMetaData())
                .Append('}')
                .Append(", messageIdentifier='")
                .Append(GetIdentifier())
                .Append('\'');
        }

        /// <summary>
        /// Describe the type of message, used in {@link #toString()}.
        /// <p>
        /// Defaults to the simple class name of the actual instance.
        /// </summary>
        /// <returns>the type of message</returns>
        protected virtual string DescribeType()
        {
            return typeof(MessageDecorator<>).Name;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder()
                .Append(DescribeType())
                .Append("{");
            DescribeTo(sb);
            return sb.Append("}")
                .ToString();
        }
        
        //TODO: serializePayload
        //TODO: serializeMetaData
        
    }
}