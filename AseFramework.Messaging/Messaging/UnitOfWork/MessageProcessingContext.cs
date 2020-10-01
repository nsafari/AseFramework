using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Ase.Messaging.Common;
using Nito.Collections;

namespace Ase.Messaging.Messaging.UnitOfWork
{
    public class MessageProcessingContext<T, R>
        where T : class, IMessage<object>, IMessage<R> where R : class
    {
        private static readonly Deque<Action<IUnitOfWork<T, R>>> Empty = new Deque<Action<IUnitOfWork<T, R>>>();

        private readonly IDictionary<Phase, Deque<Action<IUnitOfWork<T, R>>>> _handlers =
            new ConcurrentDictionary<Phase, Deque<Action<IUnitOfWork<T, R>>>>();

        private T _message;
        private ExecutionResult<T>? executionResult;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public MessageProcessingContext(T message)
        {
            this._message = message;
        }

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
        
        public void setExecutionResult(ExecutionResult<T>? executionResult) {
            Assert.IsTrue(this.executionResult == null || executionResult == null || executionResult.IsExceptionResult(),
                () =>
                    $"Cannot change execution result [{_message}] to [{this.executionResult}] for message [{executionResult}].");
            if (this.executionResult != null && this.executionResult.IsExceptionResult()) {
                new System.AggregateException(this.executionResult.GetExceptionResult(), executionResult.GetExceptionResult());
            } else {
                this.executionResult = executionResult;
            }
        }
    }
}