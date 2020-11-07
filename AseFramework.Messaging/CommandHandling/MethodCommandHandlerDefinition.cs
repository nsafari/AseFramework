using Ase.Messaging.Annotation;

namespace Ase.Messaging.CommandHandling
{
    public class MethodCommandHandlerDefinition: IHandlerEnhancerDefinition
    {
        public IMessageHandlingMember<T> WrapHandler<T>(IMessageHandlingMember<T> original)
        {
            return original.AnnotationAttributes(typeof(CommandHandler))
                .map(attr -> (MessageHandlingMember<T>) new MethodCommandMessageHandlingMember(original, attr))
                .orElse(original);

        }
    }
}