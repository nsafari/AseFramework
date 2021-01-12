using System.Collections.Generic;
using Ase.Messaging.EventHandling;
using Ase.Messaging.Messaging.Annotation;

namespace AseFramework.Modeling.Command
{
    /// <summary>
    /// Interface describing en entity that is a child of another entity.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IChildEntity<T>
    {
        /// <summary>
        /// Publish the given {@code msg} to the appropriate handlers on the given {@code declaringInstance}
        /// </summary>
        /// <param name="msg">The message to publish</param>
        /// <param name="declaringInstance">The instance of this entity to invoke handlers on</param>
        void Publish(IEventMessage<object> msg, T declaringInstance);
        
        /// <summary>
        /// Returns the command handlers declared in this entity
        /// </summary>
        /// <typeparam name="TR"></typeparam>
        /// <returns>a list of message handling members that are capable of processing command messages</returns>
        IList<IMessageHandlingMember<T>> CommandHandlers();
    }
}