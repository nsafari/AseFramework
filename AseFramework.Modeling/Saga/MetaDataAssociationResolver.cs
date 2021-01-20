using Ase.Messaging.EventHandling;
using Ase.Messaging.Messaging.Annotation;

namespace AseFramework.Modeling.Saga
{
    public class MetaDataAssociationResolver: IAssociationResolver
    {
        public void Validate<T>(string associationPropertyName, IMessageHandlingMember<T> handler)
        {
            // Do nothing
        }

        public object Resolve<T>(string associationPropertyName, IEventMessage<object> message, IMessageHandlingMember<T> handler)
        {
            return message.GetMetaData()[associationPropertyName];
        }
    }
}