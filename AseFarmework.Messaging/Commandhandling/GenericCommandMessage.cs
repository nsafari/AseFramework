using System;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Text;
using Ase.Messaging.Messaging;

namespace Ase.Messaging.CommandHandling
{
    /// <summary>
    /// Implementation of the CommandMessage that takes all properties as constructor parameters.
    /// </summary>
    /// <typeparam name="T">The type of payload contained in this Message</typeparam>
    public class GenericCommandMessage<T> : MessageDecorator<T>, ICommandMessage<T>
        where T : class
    {
        private readonly string _commandName;

        /// <summary>
        /// Create a CommandMessage with the given {@code command} as payload and empty metaData
        /// </summary>
        /// <param name="payload">the payload for the Message</param>
        public GenericCommandMessage(T payload) : this(payload, MetaData.EmptyInstance)
        {
        }


        /// <summary>
        /// Create a CommandMessage with the given {@code command} as payload.
        /// </summary>
        /// <param name="payload">the payload for the Message</param>
        /// <param name="metaData">The meta data for this message</param>
        public GenericCommandMessage(T payload, IImmutableDictionary<string, object> metaData) :
            this(new GenericMessage<T>(payload, metaData), typeof(T).Name)
        {
        }

        /// <summary>
        /// Create a CommandMessage from the given {@code delegate} message containing payload, metadata and message
        /// identifier, and the given {@code commandName}.
        /// </summary>
        /// <param name="delegate">the delegate message</param>
        /// <param name="commandName">The name of the command</param>
        public GenericCommandMessage(IMessage<T> @delegate, string commandName) : base(@delegate)
        {
            _commandName = commandName;
        }

        /// <summary>
        /// Returns the given command as a CommandMessage. If {@code command} already implements CommandMessage, it is
        /// returned as-is. Otherwise, the given {@code command} is wrapped into a GenericCommandMessage as its payload.
        /// </summary>
        /// <param name="command">the command to wrap as CommandMessage</param>
        /// <typeparam name="C">a CommandMessage containing given {@code command} as payload, or {@code command} if it 
        /// already implements CommandMessage.</typeparam>
        /// <returns></returns>
        public static ICommandMessage<C> AsCommandMessage<C>(object command)
            where C : class
        {
            if (command is ICommandMessage<C> asCommandMessage)
            {
                return asCommandMessage;
            }

            return new GenericCommandMessage<C>((C) command, MetaData.EmptyInstance);
        }

        public string CommandName()
        {
            return _commandName;
        }

        public ICommandMessage<T> WithMetaData(ReadOnlyDictionary<string, object> metaData)
        {
            return new GenericCommandMessage<T>(Delegate().WithMetaData(metaData), _commandName);
        }

        public ICommandMessage<T> AndMetaData(ReadOnlyDictionary<string, object> metaData)
        {
            return new GenericCommandMessage<T>(Delegate().AndMetaData(metaData), _commandName);
        }

        protected override void DescribeTo(StringBuilder stringBuilder)
        {
            base.DescribeTo(stringBuilder);
            stringBuilder.Append(", commandName='")
                .Append(CommandName())
                .Append('\'');
        }

        protected override string DescribeType()
        {
            return typeof(GenericCommandMessage<>).Name;
        }
    }
}