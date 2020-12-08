using Ase.Messaging.Annotation;
using Ase.Messaging.Messaging.Annotation;

namespace AseFramework.Modeling.Command.Inspection
{
    public class MethodCommandHandlerInterceptorDefinition: IHandlerEnhancerDefinition
    {
        public IMessageHandlingMember<T> WrapHandler<T>(IMessageHandlingMember<T> original) {
            return original.AnnotationAttributes(CommandHandlerInterceptorAttribute.class)
                .map(attr -> (MessageHandlingMember<T>) new MethodCommandHandlerInterceptorHandlingMember<>(
                    original, attr))
                .orElse(original);
        }

    }
}