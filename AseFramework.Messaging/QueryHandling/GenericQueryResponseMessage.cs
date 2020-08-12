using System;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using Ase.Messaging.Messaging;

namespace Ase.Messaging.QueryHandling
{
    /// <summary>
    /// QueryResponseMessage implementation that takes all properties as constructor parameters.
    /// </summary>
    /// <typeparam name="R">The type of return value contained in this response</typeparam>
    public class GenericQueryResponseMessage<R> : GenericResultMessage<R>, IQueryResponseMessage<R>
        where R : class
    {
        /// <summary>
        /// Initialize the response message with given {@code result}.
        /// </summary>
        /// <param name="result">The result reported by the Query Handler, may not be {@code null}</param>
        public GenericQueryResponseMessage(R result) : this(result.GetType(), result, MetaData.EmptyInstance)
        {
        }


        /// <summary>
        /// Initialize a response message with given {@code result} and {@code declaredResultType}. This constructor allows
        /// the actual result to be {@code null}.
        /// </summary>
        /// <param name="declaredResultType">The declared type of the result</param>
        /// <param name="result">The actual result. May be {@code null}</param>
        public GenericQueryResponseMessage(Type declaredResultType, R result) : this(result.GetType(), result,
            MetaData.EmptyInstance)
        {
        }

        /// <summary>
        /// Initialize the response message with given {@code declaredResultType} and {@code exception}.
        /// </summary>
        /// <param name="declaredResultType">The declared type of the Query Response Message to be created</param>
        /// <param name="exception">The Exception describing the cause of an error</param>
        public GenericQueryResponseMessage(Type declaredResultType, Exception exception) : this(
            declaredResultType.GetType(), exception,
            MetaData.EmptyInstance)
        {
        }

        /// <summary>
        /// Initialize the response message with given {@code result} and {@code metaData}.
        /// </summary>
        /// <param name="result">The result reported by the Query Handler, may not be {@code null}</param>
        /// <param name="metaData">The meta data to contain in the message</param>
        public GenericQueryResponseMessage(R result, IImmutableDictionary<string, object> metaData) : base(
            new GenericMessage<R>(result, metaData))
        {
        }

        /// <summary>
        /// Initialize the response message with a specific {@code declaredResultType}, the given {@code result} as payload
        /// and {@code metaData}.
        /// </summary>
        /// <param name="declaredResultType">A {@link java.lang.Class} denoting the declared result type of this query response message</param>
        /// <param name="result">The result reported by the Query Handler, may be {@code null}</param>
        /// <param name="metaData">The meta data to contain in the message</param>
        public GenericQueryResponseMessage(Type declaredResultType, R result,
            IImmutableDictionary<string, object> metaData) : base(new GenericMessage<R>(declaredResultType, result,
            metaData))
        {
        }


        /// <summary>
        /// Initialize the response message with given {@code declaredResultType}, {@code exception} and {@code metaData}.
        /// </summary>
        /// <param name="declaredResultType">The declared type of the Query Response Message to be created</param>
        /// <param name="exception">The Exception describing the cause of an error</param>
        /// <param name="metaData">The meta data to contain in the message</param>
        public GenericQueryResponseMessage(Type declaredResultType, Exception exception,
            IImmutableDictionary<string, object> metaData) : base(
            new GenericMessage<R>(declaredResultType, null, metaData), exception)
        {
        }

        /// <summary>
        /// Copy-constructor that takes the payload, meta data and message identifier of the given {@code delegate} for this
        /// message.
        /// <p>
        /// Unlike the other constructors, this constructor will not attempt to retrieve any correlation data from the Unit
        /// of Work.
        /// </summary>
        /// <param name="@delegate">The message to retrieve message details from</param>
        public GenericQueryResponseMessage(IMessage<R> @delegate) : base(@delegate)
        {
        }

        /// <summary>
        /// Copy-constructor that takes the payload, meta data and message identifier of the given {@code delegate} for this
        /// message and given {@code exception} as a cause for the failure.
        /// <p>
        /// Unlike the other constructors, this constructor will not attempt to retrieve any correlation data from the Unit
        /// of Work.
        /// </summary>
        /// <param name="@delegate">The message to retrieve message details from</param>
        /// <param name="exception">The Exception describing the cause of an error</param>
        public GenericQueryResponseMessage(IMessage<R> @delegate, Exception exception) : base(@delegate, exception)
        {
        }

        public IQueryResponseMessage<R> WithMetaData(ReadOnlyDictionary<string, object> metaData)
        {
            throw new NotImplementedException();
        }

        public IQueryResponseMessage<R> AndMetaData(ReadOnlyDictionary<string, object> additionalMetaData)
        {
            throw new NotImplementedException();
        }
    }
}