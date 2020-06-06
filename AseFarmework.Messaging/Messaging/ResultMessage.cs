using System;

namespace Ase.Messaging.Messaging
{
    /// <summary>
    /// Message that represents a result of handling some form of request message.
    /// <param name="R">The type of payload contained in this Message</param>
    /// </summary>
    public interface IResultMessage<R> : IMessage<R>
    {
        /// <summary>
        /// Indicates whether the ResultMessage represents unsuccessful execution.
        /// </summary>
        /// <returns>{@code true} if execution was unsuccessful, {@code false} otherwise</returns>
        bool IsExceptional();

        /// <summary>
        /// Returns the Exception in case of exceptional result message or an <code>null</code> in case of successful
        /// execution.
        /// </summary>
        /// <returns>an nullable containing exception result or <code>null</code> in case of a successful execution</returns>
        Exception? OptionalExceptionResult();


        /// <summary>
        /// Returns the exception result. This method is to be called if {@link #isExceptional()} returns {@code true}.
        /// </summary>
        /// <returns>an exception defining the exception result</returns>
        Exception ExceptionResult()
        {
            return OptionalExceptionResult() ?? new InvalidOperationException();
        }


        /// <summary>
        /// If the this message contains an exception result, returns the details provided in the exception,
        /// if available. If this message does not carry an exception result, or the exception result doesn't
        /// provide any application-specific details, an empty optional is returned.
        /// </summary>
        /// <typeparam name="D">The type of application-specific details expected</typeparam>
        /// <returns>a nullable containing application-specific error details, if present</returns>
        D? ExceptionDetails<D>()
            where D : class
        {
            return HandlerExecutionException<D>.ResolveDetails<D>(OptionalExceptionResult());
        }
 
        
        
        //TODO: serializePayload
        //TODO: serializeExceptionResult
    }
}