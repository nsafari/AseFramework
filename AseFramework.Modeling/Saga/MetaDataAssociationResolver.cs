using Ase.Messaging.EventHandling;
using Ase.Messaging.Messaging.Annotation;

namespace AseFramework.Modeling.Saga
{
    /// <summary>
    /// Used to derive the value of an association property by looking it up the event message's
    /// {@link org.axonframework.messaging.MetaData}.
    /// </summary>
    public class MetaDataAssociationResolver: IAssociationResolver
    {
        /// <summary>
        /// Does nothing because we can only check for existence of property in the metadata during event handling.
        /// </summary>
        public void Validate<T>(string associationPropertyName, IMessageHandlingMember<T> handler)
        {
            // Do nothing
        }

        /// <summary>
        /// Finds the association property value by looking up the association property name in the event message's
        /// {@link org.axonframework.messaging.MetaData}.
        /// </summary>
        public object Resolve<T>(string associationPropertyName, IEventMessage<object> message, IMessageHandlingMember<T> handler)
        {
            return message.GetMetaData()[associationPropertyName];
        }
    }
}