using System.Collections.Immutable;
using Ase.Messaging.EventHandling;

namespace Ase.Messaging.Deadline
{
    /// <summary>
    /// Represents a Message for a Deadline, specified by its deadline name and optionally containing a deadline payload.
    /// Implementations of DeadlineMessage represent a fact (it's a specialization of EventMessage) that some deadline was
    /// reached. The optional payload contains relevant data of the scheduled deadline.
    /// </summary>
    /// <typeparam name="T">The type of payload contained in this Message; may be {@link Void} if no payload was provided</typeparam>
    public interface IDeadlineMessage<out T> : IEventMessage<T>
        where T : class
    {
        
        /// <summary>
        /// Retrieve a {@link String} representing the name of this DeadlineMessage.
        /// </summary>
        /// <returns>a {@link String} representing the name of this DeadlineMessage</returns>
        string GetDeadlineName();

        /// <summary>
        /// Returns a copy of this DeadlineMessage with the given {@code metaData}. The payload remains unchanged.
        /// </summary>
        /// <param name="metaData">The new MetaData for the Message</param>
        /// <returns>a copy of this message with the given MetaData</returns>
        new IDeadlineMessage<T> WithMetaData(IImmutableDictionary<string, object> metaData);

        /// <summary>
        /// Returns a copy of this DeadlineMessage with its MetaData merged with given {@code additionalMetaData}. The
        /// payload remains unchanged.
        /// </summary>
        /// <param name="additionalMetaData">The MetaData to merge into the DeadlineMessage</param>
        /// <returns>a copy of this message with added additional MetaData</returns>
        new IDeadlineMessage<T> AndMetaData(IImmutableDictionary<string, object> additionalMetaData);
    }
}