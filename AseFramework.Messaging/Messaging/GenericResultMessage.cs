using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Text;

namespace Ase.Messaging.Messaging
{
    /// <summary>
    ///  Generic implementation of <see cref="IResultMessage{R}"/>.
    /// </summary>
    /// <typeparam name="R"></typeparam>
    public class GenericResultMessage<R> : MessageDecorator<R>, IResultMessage<R>
        where R : class
    {
        private readonly Exception? _exception;

        /// <summary>
        ///  Creates a ResultMessage with the given {@code result} as the payload.
        /// </summary>
        /// <param name="result">the payload for the Message</param>
        public GenericResultMessage(R result) : this(result, MetaData.EmptyInstance)
        {
        }
        
        /// <summary>
        /// Creates a ResultMessage with the given {@code exception}.
        /// </summary>
        /// <param name="exception">the Exception describing the cause of an error</param>
        public GenericResultMessage(Exception exception) : this(exception, MetaData.EmptyInstance)
        {
        }
        
        /// <summary>
        /// Creates a ResultMessage with the given {@code result} as the payload and {@code metaData} as the meta data.
        /// </summary>
        /// <param name="result">the payload for the Message</param>
        /// <param name="metaData">the meta data for the Message</param>
        public GenericResultMessage(R result, IImmutableDictionary<string, object> metaData) : this(
            new GenericMessage<R>(result, metaData))
        {
        }

        /// <summary>
        /// Creates a ResultMessage with the given {@code exception} and {@code metaData}.
        /// </summary>
        /// <param name="exception">the Exception describing the cause of an error</param>
        /// <param name="metaData">the meta data for the Message</param>
        public GenericResultMessage(Exception exception, IImmutableDictionary<string, object> metaData) : this(
            new GenericMessage<R>(null, metaData), exception)
        {
        }

        /// <summary>
        /// Creates a new ResultMessage with given {@code delegate} message.
        /// </summary>
        /// <param name="delegate">delegate the message delegate</param>
        public GenericResultMessage(IMessage<R> @delegate) : this(@delegate, FindExceptionResult(@delegate))
        {
        }

        /// <summary>
        ///  Creates a ResultMessage with given {@code delegate} message and {@code exception}.
        /// </summary>
        /// <param name="delegate">the Message delegate</param>
        /// <param name="exception">the Exception describing the cause of an error</param>
        public GenericResultMessage(IMessage<R> @delegate, Exception? exception) : base(@delegate)
        {
            this._exception = exception;
        }

        public bool IsExceptional()
        {
            return _exception != null;
        }

        public Exception? OptionalExceptionResult()
        {
            return _exception;
        }

        public override IMessage<R> WithMetaData(IReadOnlyDictionary<string, object> metaData)
        {
            return new GenericResultMessage<R>(Delegate().WithMetaData(metaData), _exception);
        }

        public override IMessage<R> AndMetaData(IReadOnlyDictionary<string, object> metaData)
        {
            return new GenericResultMessage<R>(Delegate().AndMetaData(metaData), _exception);
        }

        protected override void DescribeTo(StringBuilder stringBuilder)
        {
            stringBuilder.Append("payload={")
                .Append(IsExceptional() ? null : GetPayload())
                .Append('}')
                .Append(", metadata={")
                .Append(GetMetaData())
                .Append('}')
                .Append(", messageIdentifier='")
                .Append(GetIdentifier())
                .Append('\'')
                .Append(", exception='")
                .Append(_exception)
                .Append('\'');

        }

        protected override string DescribeType()
        {
            return typeof(GenericResultMessage<>).Name;
        }

        private static Exception? FindExceptionResult(IMessage<R> @delegate)
        {
            if (@delegate is IResultMessage<R> resultMessage && resultMessage.IsExceptional())
            {
                return resultMessage.ExceptionResult();
            }

            return null;
        }
    }
}