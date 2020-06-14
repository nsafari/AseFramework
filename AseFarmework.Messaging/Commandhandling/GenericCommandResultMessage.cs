using System;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using Ase.Messaging.Messaging;

namespace Ase.Messaging.CommandHandling
{
    /**
     * Generic implementation of {@link CommandResultMessage}.
     * <typeparam name="R">The type of the payload contained in this Message</typeparam>
     */
    public class GenericCommandResultMessage<R> : GenericResultMessage<R>, ICommandResultMessage<R>
        where R : class
    {
        /**
         * Returns the given {@code commandResult} as a {@link CommandResultMessage} instance. If {@code commandResult}
         * already implements {@link CommandResultMessage}, it is returned as-is. If {@code commandResult} implements
         * {@link Message}, payload and meta data will be used to construct new {@link GenericCommandResultMessage}.
         * Otherwise, the given {@code commandResult} is wrapped into a {@link GenericCommandResultMessage} as its payload.
         *
         * <param name="commandResult">the command result to be wrapped as {@link CommandResultMessage}</param>
         * <typeparam name="T">The type of the payload contained in returned Message</typeparam>
         * <returns>a Message containing given {@code commandResult} as payload, or {@code commandResult} if already
         * implements {@link CommandResultMessage}</returns>
         */
        public static ICommandResultMessage<T> AsCommandResultMessage<T>(object commandResult) 
            where T : class
        {
            return commandResult switch
            {
                ICommandResultMessage<T> commandResultMessage => commandResultMessage,
                IMessage<T> message => new GenericCommandResultMessage<T>(message),
                _ => new GenericCommandResultMessage<T>((T) commandResult)
            };
        }

        /**
         * Creates a Command Result Message with the given {@code exception} result.
         * <param name="exception">the Exception describing the cause of an error</param>
         * <typeparam name="T">the type of payload</typeparam>
         * <returns>a message containing exception result</returns>
         */
        public static ICommandResultMessage<T> AsCommandResultMessage<T>(Exception exception) 
            where T : class
        {
            return new GenericCommandResultMessage<T>(exception);
        }

        public GenericCommandResultMessage(R result) : base(result)
        {
        }

        public GenericCommandResultMessage(Exception exception) : base(exception)
        {
        }

        public GenericCommandResultMessage(R result, IImmutableDictionary<string, object> metaData) : base(result,
            metaData)
        {
        }

        public GenericCommandResultMessage(Exception exception, IImmutableDictionary<string, object> metaData) : base(
            exception, metaData)
        {
        }

        public GenericCommandResultMessage(IMessage<R> @delegate) : base(@delegate)
        {
        }

        public GenericCommandResultMessage(IMessage<R> @delegate, Exception exception) : base(@delegate, exception)
        {
        }

        public ICommandResultMessage<R> WithMetaData<T>(ReadOnlyDictionary<string, T> metaData)
        {
            throw new NotImplementedException();
        }

        public ICommandResultMessage<R> AndMetaData<T>(ReadOnlyDictionary<string, T> metaData)
        {
            throw new NotImplementedException();
        }
    }
}