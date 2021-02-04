using Ase.Messaging.Messaging.Annotation;

namespace AseFramework.Modeling.Deadline.Annotation
{
    /// <summary>
    /// Interface describing a message handler capable of handling a specific deadline.
    /// </summary>
    /// <typeparam name="T">The type of entity to which the message handler will delegate the actual handling of the deadline</typeparam>
    public interface IDeadlineHandlingMember<in T>: IMessageHandlingMember<T>
    {
        
    }
}