using System;
using System.Collections.Generic;
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
        /// Creates a QueryResponseMessage for the given {@code result}. If result already implements QueryResponseMessage,
        /// it is returned directly. Otherwise a new QueryResponseMessage is created with the result as payload.
        /// </summary>
        /// <param name="result">The result of a Query, to be wrapped in a QueryResponseMessage</param>
        /// <returns>a QueryResponseMessage for the given {@code result}, or the result itself, if already a QueryResponseMessage.</returns>
        public static IQueryResponseMessage<R> AsResponseMessage(object result)
        {
            if (result is IQueryResponseMessage<R>)
            {
                return (IQueryResponseMessage<R>)result;
            }
            else if (result is IResultMessage<R>)
            {
                IResultMessage<R> resultMessage = (IResultMessage<R>)result;
                return new GenericQueryResponseMessage<R>(resultMessage.GetPayload(), resultMessage.GetMetaData());
            }
            else if (result is IMessage<R>)
            {
                IMessage<R> message = (IMessage<R>)result;
                return new GenericQueryResponseMessage<R>(message.GetPayload(), message.GetMetaData());
            }
            else
            {
                return new GenericQueryResponseMessage<R>((R)result);
            }
        }


        /// <summary>
        /// Creates a QueryResponseMessage for the given {@code result} with a {@code declaredType} as the result type.
        /// Providing both the result type and the result allows the creation of a nullable response message, as the
        /// implementation does not have to check the type itself, which could result in a
        /// {@link java.lang.NullPointerException}. If result already implements QueryResponseMessage, it is returned
        /// directly. Otherwise a new QueryResponseMessage is created with the declared type as the result type and the
        /// result as payload.
        /// </summary>
        /// <param name="declaredType">The declared type of the Query Response Message to be created.</param>
        /// <param name="result">The result of a Query, to be wrapped in a QueryResponseMessage</param>
        /// <returns>a QueryResponseMessage for the given {@code result}, or the result itself, if already a QueryResponseMessage.</returns>
        public static IQueryResponseMessage<R> AsNullableResponseMessage(Type declaredType, Object result)
        {
            if (result is IQueryResponseMessage<R>)
            {
                return (IQueryResponseMessage<R>)result;
            }
            else if (result is IResultMessage<R>)
            {
                IResultMessage<R> resultMessage = (IResultMessage<R>)result;
                if (resultMessage.IsExceptional())
                {
                    Exception cause = resultMessage.ExceptionResult();
                    return new GenericQueryResponseMessage<R>(declaredType, cause, resultMessage.GetMetaData());
                }
                return new GenericQueryResponseMessage<R>(resultMessage.GetPayload(), resultMessage.GetMetaData());
            }
            else if (result is IMessage<R>)
            {
                IMessage<R> message = (IMessage<R>)result;
                return new GenericQueryResponseMessage<R>(message.GetPayload(), message.GetMetaData());
            }
            else
            {
                return new GenericQueryResponseMessage<R>(declaredType, (R)result);
            }
        }

        /// <summary>
        /// Creates a Query Response Message with given {@code declaredType} and {@code exception}.
        /// </summary>
        /// <param name="declaredType">The declared type of the Query Response Message to be created</param>
        /// <param name="exception">The Exception describing the cause of an error</param>
        /// <returns>a message containing exception result</returns>
        public static IQueryResponseMessage<R> AsResponseMessage(Type declaredType, Exception exception)
        {
            return new GenericQueryResponseMessage<R>(declaredType, exception);
        }

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
        public GenericQueryResponseMessage(R? result, IImmutableDictionary<string, object> metaData) : base(
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

        public IQueryResponseMessage<R> WithMetaData(IReadOnlyDictionary<string, object> metaData)
        {
            return new GenericQueryResponseMessage<R>(Delegate().WithMetaData(metaData));
        }

        public IQueryResponseMessage<R> AndMetaData(IReadOnlyDictionary<string, object> additionalMetaData)
        {
            return new GenericQueryResponseMessage<R>(Delegate().AndMetaData(additionalMetaData));
        }
    }
}