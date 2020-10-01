using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Nito.Collections;

namespace Ase.Messaging.Messaging.UnitOfWork
{
    public class MessageProcessingContext<T, R>
        where T : IMessage<object>, IMessage<R> where R : class
    {
        private static readonly Deque<T> Empty = new Deque<T>();

        private readonly IDictionary<Phase, Deque<Action<IUnitOfWork<T, R>>>> _handlers =
            new ConcurrentDictionary<Phase, Deque<Action<IUnitOfWork<T, R>>>>();

        private T message;
        private ExecutionResult<R> executionResult;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public MessageProcessingContext(T message) {
            this.message = message;
        }
    }
}