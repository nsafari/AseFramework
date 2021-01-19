using Ase.Messaging.Messaging.Annotation;

namespace AseFramework.Modeling.Saga
{
    public class SagaMethodMessageHandlingMember<T>: WrappedMessageHandlingMember<T>
    {
        
        private readonly IMessageHandlingMember<T> @delegate;
        private readonly SagaCreationPolicy creationPolicy;
        private readonly string associationKey;
        private readonly string associationPropertyName;
        private readonly IAssociationResolver associationResolver;
        private readonly bool endingHandler;

    }
}