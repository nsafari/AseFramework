using System;

namespace Ase.Messaging.Messaging
{
    public class GenericResultMessage<R> : MessageDecorator<R>, IResultMessage<R>
    {
        private readonly Exception _exception;

        public GenericResultMessage(IMessage<R> @delegate) : base(@delegate)
        {
        }

        public bool IsExceptional()
        {
            throw new NotImplementedException();
        }

        public Exception? OptionalExceptionResult()
        {
            throw new NotImplementedException();
        }
    }
}