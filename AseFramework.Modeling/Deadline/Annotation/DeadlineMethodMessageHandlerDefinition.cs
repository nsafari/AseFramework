using System.Collections.Generic;
using Ase.Messaging.Deadline;
using Ase.Messaging.Messaging;
using Ase.Messaging.Messaging.Annotation;

namespace AseFramework.Modeling.Deadline.Annotation
{
    /// <summary>
    /// Implementation of a {@link HandlerEnhancerDefinition} that is used for {@link DeadlineHandler} annotated methods.
    /// </summary>
    public class DeadlineMethodMessageHandlerDefinition : IHandlerEnhancerDefinition
    {
        public IMessageHandlingMember<T> WrapHandler<T>(IMessageHandlingMember<T> original)
            where T : class
        {
            IDictionary<string, object?>? annotationAttributes =
                original.AnnotationAttributes<DeadlineHandlerAttribute>();
            return annotationAttributes == null
                ? new DeadlineMethodMessageHandlingMember<T>(original, annotationAttributes)
                : original;
        }

        private class DeadlineMethodMessageHandlingMember<T> : WrappedMessageHandlingMember<T>,
            IDeadlineHandlingMember<T>
        {
            private readonly string? _deadlineName;

            internal DeadlineMethodMessageHandlingMember(
                IMessageHandlingMember<T> @delegate,
                IDictionary<string, object?>? annotationAttributes
            ) : base(@delegate)
            {
                _deadlineName = (string) annotationAttributes?["deadlineName"]!;
            }
            
            public new bool CanHandle(IMessage<object> message) {
                
                return message is IDeadlineMessage<object> deadlineMessage
                       && DeadlineNameMatch(deadlineMessage)
                       && CanHandle(deadlineMessage);
                
            }
            
            private bool DeadlineNameMatch(IDeadlineMessage<object> message) {
                return _deadlineName != null && (DeadlineNameMatchesAll() || _deadlineName.Equals(message.GetDeadlineName()));
            }

            private bool DeadlineNameMatchesAll() {
                return _deadlineName != null && _deadlineName.Equals("");
            }


        }
    }
}