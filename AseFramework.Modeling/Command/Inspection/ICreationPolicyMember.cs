using Ase.Messaging.Messaging.Annotation;

namespace AseFramework.Modeling.Command.Inspection
{
    /// <summary>
    /// Interface specifying a message handler containing a creation policy definition.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICreationPolicyMember<in T>: IMessageHandlingMember<T>
    {
        
        /// <summary>
        /// Returns the creation policy set on the {@link MessageHandlingMember}.
        /// </summary>
        /// <returns>the creation policy set on the handler</returns>
        AggregateCreationPolicy CreationPolicy();

    }
}