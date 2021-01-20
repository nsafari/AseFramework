using System;
using Ase.Messaging.EventHandling;
using Ase.Messaging.Messaging.Annotation;

namespace AseFramework.Modeling.Saga
{
    /// <summary>
    /// Used to derive the value of an association property as designated by the association property name.
    /// </summary>
    public interface IAssociationResolver
    {
        /// <summary>
        /// Validates that the associationPropertyName supplied is compatible with the handler.
        /// </summary>
        void Validate<T>(string associationPropertyName, IMessageHandlingMember<T> handler);

        /// <summary>
        /// Resolves the associationPropertyName as a value.
        /// </summary>
        object Resolve<T>(string associationPropertyName, IEventMessage<object> message, IMessageHandlingMember<T> handler);
    }
}