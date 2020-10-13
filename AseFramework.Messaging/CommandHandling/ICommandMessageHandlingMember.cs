using Ase.Messaging.Annotation;

namespace Ase.Messaging.CommandHandling
{
    /// <summary>
    /// Interface describing a message handler capable of handling a specific command.
    /// </summary>
    /// <typeparam name="T">The type of entity to which the message handler will delegate the actual handling of the command</typeparam>
    public interface ICommandMessageHandlingMember<in T>: IMessageHandlingMember<T>
    {
        /// <summary>
        /// Returns the name of the command that can be handled.
        /// </summary>
        /// <returns>The name of the command that can be handled</returns>
        string CommandName();
        
        /// <summary>
        /// Returns the property of the command that is to be used as routing key towards this command handler instance. If
        /// multiple handlers instances are available, a sending component is responsible to route commands with the same
        /// routing key value to the correct instance.
        /// </summary>
        /// <returns>The property of the command to use as routing key</returns>
        string RoutingKey();
        
        /// <summary>
        /// Check if this message handler creates a new instance of the entity of type {@code T} to handle this command.
        /// <p>
        /// This is for instance the case if the message is handled in the constructor method of the entity.
        /// </summary>
        /// <returns>{@code true} if this handler is also factory for entities, {@code false} otherwise.</returns>
        bool IsFactoryHandler();
    }
}