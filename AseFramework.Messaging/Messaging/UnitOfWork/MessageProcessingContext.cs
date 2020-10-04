using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Ase.Messaging.Common;
using Nito.Collections;

namespace Ase.Messaging.Messaging.UnitOfWork
{
    /// <summary>
    /// Maintains the context around the processing of a single Message. This class notifies handlers when the Unit of Work
    /// processing the Message transitions to a new {@link Phase}.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="R"></typeparam>
    public class MessageProcessingContext<T, R>
        where T : class, IMessage<object>, IMessage<R> where R : class
    {
        private static readonly Deque<Action<IUnitOfWork<T, R>>> Empty = new Deque<Action<IUnitOfWork<T, R>>>();

        private readonly IDictionary<Phase, Deque<Action<IUnitOfWork<T, R>>>> _handlers =
            new ConcurrentDictionary<Phase, Deque<Action<IUnitOfWork<T, R>>>>();

        private T? _message;
        private ExecutionResult<T>? _executionResult;

        /// <summary>
        /// Creates a new processing context for the given {@code message}. 
        /// </summary>
        /// <param name="message">The Message that is to be processed.</param>
        public MessageProcessingContext(T message)
        {
            this._message = message;
        }

        /// <summary>
        /// Invoke the handlers in this collection attached to the given {@code phase}.
        /// </summary>
        /// <param name="unitOfWork">The Unit of Work that is changing its phase</param>
        /// <param name="phase">The phase for which attached handlers should be invoked</param>
        public void NotifyHandlers(IUnitOfWork<T, R> unitOfWork, Phase phase)
        {
            // if (LOGGER.isDebugEnabled()) {
            //     LOGGER.debug("Notifying handlers for phase {}", phase.toString());
            // }
            Deque<Action<IUnitOfWork<T, R>>> l = _handlers[phase] ?? Empty;
            while (l.Count != 0)
            {
                l[0](unitOfWork);
                l.RemoveAt(0);
            }
        }

        /// <summary>
        /// Adds a handler to the collection. Note that the order in which you register the handlers determines the order
        /// in which they will be handled during the various stages of a unit of work.
        /// </summary>
        /// <param name="phase">The phase of the unit of work to attach the handler to</param>
        /// <param name="handler">The handler to invoke in the given phase</param>
        public void AddHandler(Phase phase, Action<IUnitOfWork<T, R>> handler)
        {
            // if (LOGGER.isDebugEnabled()) {
            //     LOGGER.debug("Adding handler {} for phase {}", handler.getClass().getName(), phase.toString());
            // }
            if (!_handlers.TryGetValue(phase, out Deque<Action<IUnitOfWork<T, R>>>? consumers))
            {
                consumers = new Deque<Action<IUnitOfWork<T, R>>>();
                _handlers.Add(phase, consumers);
            }

            if (PhaseUtil.GetPhase(phase).IsReverseCallbackOrder())
            {
                consumers.AddToFront(handler);
            }
            else
            {
                consumers.AddToBack(handler);
            }
        }

        /// <summary>
        /// Set the execution result of processing the current {@link #getMessage() Message}. In case this context has a
        /// previously set ExecutionResult, setting a new result is only allowed if the new result is an exception result.
        /// <p/>
        /// In case the previously set result is also an exception result, the exception in the new execution result is
        /// added to the original exception as a suppressed exception.
        /// </summary>
        /// <param name="newExecutionResult">the ExecutionResult of the currently handled Message</param>
        public void SetExecutionResult(ExecutionResult<T>? newExecutionResult)
        {
            Assert.IsTrue(
                this._executionResult == null || newExecutionResult == null || newExecutionResult.IsExceptionResult(),
                () =>
                    $"Cannot change execution result [{_message}] to [{_executionResult}] for message [{newExecutionResult}].");

            if (_executionResult != null && _executionResult.IsExceptionResult())
            {
                _executionResult.GetExceptionResult()!.AddInnerException(newExecutionResult?.GetExceptionResult());
            }
            else
            {
                _executionResult = newExecutionResult;
            }
        }

        /// <summary>
        /// Get the Message that is being processed in this context.
        /// </summary>
        /// <returns>the Message that is being processed</returns>
        public T? GetMessage()
        {
            return _message;
        }

        /// <summary>
        /// Get the result of processing the {@link #getMessage() Message}. If the Message has not been processed yet this
        /// method returns {@code null}.
        /// </summary>
        /// <returns>The result of processing the Message, or {@code null} if the Message hasn't been processed</returns>
        public ExecutionResult<T>? GetExecutionResult()
        {
            return _executionResult;
        }

        /// <summary>
        /// Transform the Message being processed using the given operator.
        /// </summary>
        /// <param name="transformOperator">The transform operator to apply to the stored message</param>
        /// <typeparam name="TR"></typeparam>
        public void TransformMessage<TR>(Func<T, TR> transformOperator) 
            where TR : IMessage<object>
        {
            _message = transformOperator(_message!) as T;
        }
        
        /// <summary>
        /// Reset the processing context. This clears the execution result and map with registered handlers, and replaces
        /// the current Message with the given {@code message}.
        /// </summary>
        /// <param name="message">The new message that is being processed</param>
        public void Reset(T message) {
            _message = message;
            _handlers.Clear();
            _executionResult = null;
        }
    }
}