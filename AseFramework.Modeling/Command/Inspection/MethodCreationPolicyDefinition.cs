using System.Collections.Generic;
using Ase.Messaging.Messaging.Annotation;

namespace AseFramework.Modeling.Command.Inspection
{
    public class MethodCreationPolicyDefinition : IHandlerEnhancerDefinition
    {
        public IMessageHandlingMember<T> WrapHandler<T>(IMessageHandlingMember<T> original) where T : class
        {
            var annotationAttributes = original.AnnotationAttributes<CreationPolicyAttribute>();
            return annotationAttributes != null
                ? new MethodCreationPolicyHandlingMember<T>(original, annotationAttributes)
                : original;
        }

        private class MethodCreationPolicyHandlingMember<T> : WrappedMessageHandlingMember<T>,
            ICreationPolicyMember<T>
        {
            private readonly AggregateCreationPolicy _creationPolicy;

            internal MethodCreationPolicyHandlingMember(
                IMessageHandlingMember<T> @delegate,
                IDictionary<string, object?> attr
            ) : base(@delegate)
            {
                _creationPolicy = (AggregateCreationPolicy) attr["creationPolicy"]!;
            }
            
            public AggregateCreationPolicy CreationPolicy()
            {
                return _creationPolicy;
            }
        }
    }
}