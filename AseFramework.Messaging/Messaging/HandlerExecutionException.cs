using System;
using Ase.Messaging.Common;

namespace Ase.Messaging.Messaging
{
    /// <summary>
    /// Base exception for exceptions raised by Handler methods. Besides standard exception information (such as message and
    /// innerException), these exception may optionally carry an object with additional application-specific details about the
    /// exception.
    /// </summary>
    public class HandlerExecutionException<R> : AseException
        where R : class
    {
        private readonly R? _details;

        /// <summary>
        /// Initializes an execution exception with given {@code message}. The innerException and application-specific details are
        /// set to {@code null}.
        /// </summary>
        /// <param name="message">A message describing the exception</param>
        public HandlerExecutionException(string message) : this(message, null, null)
        {
        }

        /// <summary>
        /// Initializes an execution exception with given {@code message} and {@code innerException}. The application-specific
        /// details are set to {@code null}.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public HandlerExecutionException(string message, Exception innerException) : this(message, innerException,
            ResolveDetails(innerException))
        {
        }

        /// <summary>
        /// Initializes an execution exception with given {@code message}, {@code innerException} and application-specific
        /// {@code details}.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        /// <param name="details"></param>
        public HandlerExecutionException(string message, Exception? innerException, R? details) : base(message,
            innerException)
        {
            _details = details;
        }

        /// <summary>
        /// Returns an Optional containing application-specific details of the exception, if any were provided. These
        /// details are implicitly cast to the expected type. A mismatch in type may lead to a {@link ClassCastException}
        /// further downstream, when accessing the Optional's enclosed value.
        /// </summary>
        /// <typeparam name="TR">The type of details expected</typeparam>
        /// <returns>a nullable containing the details, if provided</returns>
        public R? GetDetails()
        {
            return _details as R;
        }

        /// <summary>
        /// Resolve details from the given {@code throwable}, taking into account that the details may be available in any
        /// of the {@code HandlerExecutionException}s is the "innerException" chain.
        /// </summary>
        /// <typeparam name="R">The type of details expected</typeparam>
        /// <returns>an Optional containing details, if present in the given {@code throwable}</returns>
        public static R? ResolveDetails(Exception? exception) 
        {
            while (true)
            {
                if (exception != null && exception is HandlerExecutionException<R> handlerExecutionException)
                {
                    return handlerExecutionException.GetDetails();
                }

                if (exception?.InnerException == null)
                {
                    return null;
                }

                exception = exception.InnerException;
            }
        }
    }
}