using System;
using System.Collections.ObjectModel;
using Ase.Messaging.Messaging;

namespace Ase.Messaging.CommandHandling
{
    /// <summary>
    /// Represents a Message carrying a command as its payload. These messages carry an intention to change application
    /// state.
    /// </summary>
    /// <typeparam name="T">The type of payload contained in the message</typeparam>
    public interface ICommandMessage<T> : IMessage<T>
        where T : class
    {
        /// <summary>
        /// Returns the name of the command to execute. This is an indication of what should be done, using the payload as
        /// parameter.
        /// </summary>
        /// <returns></returns>
        string CommandName();

        /// <summary>
        /// Returns a copy of this CommandMessage with the given {@code metaData}. The payload remains unchanged.
        /// <p/>
        /// While the implementation returned may be different than the implementation of {@code this}, implementations
        /// must take special care in returning the same type of Message (e.g. EventMessage, DomainEventMessage) to prevent
        /// errors further downstream.
        /// </summary>
        /// <param name="metaData">The new MetaData for the Message</param>
        /// <returns>a copy of this message with the given MetaData</returns>
        ICommandMessage<T> WithMetaData(ReadOnlyDictionary<string, object> metaData);

        /// <summary>
        /// Returns a copy of this CommandMessage with it MetaData merged with the given {@code metaData}. The payload
        /// remains unchanged.
        /// </summary>
        /// <param name="metaData">The MetaData to merge with</param>
        /// <returns>a copy of this message with the given MetaData</returns>
        ICommandMessage<T> AndMetaData(ReadOnlyDictionary<string, object> metaData);
    }
}