using System;
using System.Collections.Generic;
using Nito.Collections;

namespace Ase.Messaging.Messaging.UnitOfWork
{
    public class MessageProcessingContext<T>
        where T : IMessage<object>
    {

        private static readonly Deque<T> Empty = new Deque<T>();

        private readonly IDictionary<Phase, Deque<Action<IUnitOfWork<T>>>> handlers = new IDi<>(Phase.class);
        private T message;
        private ExecutionResult executionResult;

    }
}