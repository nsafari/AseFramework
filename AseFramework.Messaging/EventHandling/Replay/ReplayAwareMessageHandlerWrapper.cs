using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;
using Ase.Messaging.Common.Annotation;
using Ase.Messaging.Messaging;
using Ase.Messaging.Messaging.Annotation;

namespace Ase.Messaging.EventHandling.Replay
{
    /// <summary>
    /// An implementation of the {@link org.axonframework.messaging.annotation.HandlerEnhancerDefinition} that is used for
    /// {@link org.axonframework.eventhandling.AllowReplay} annotated message handling methods.
    /// </summary>
    public class ReplayAwareMessageHandlerWrapper : IHandlerEnhancerDefinition
    {
        private static readonly ImmutableDictionary<string, object> DefaultSetting =
            ImmutableDictionary<string, object>.Empty.Add("allowReplay", true);

        public IMessageHandlingMember<T> WrapHandler<T>(IMessageHandlingMember<T> original)
            where T : class
        {
            IDictionary<string, object?>? attributes = original.AnnotationAttributes<AllowReplayAttribute>();
            var annotationAttributes =
                AnnotationUtils.FindAnnotationAttributes<AllowReplayAttribute>(
                    original.Unwrap<MemberInfo>()?.DeclaringType
                );
            var isReplayAllowed =
                attributes == null ? annotationAttributes ?? DefaultSetting! : attributes["allowReplay"];

            return isReplayAllowed == null ? new ReplayBlockingMessageHandlingMember<T>(original) : original;
        }

        private class ReplayBlockingMessageHandlingMember<T> : WrappedMessageHandlingMember<T>
        {
            public ReplayBlockingMessageHandlingMember(IMessageHandlingMember<T> original) : base(original)
            {
            }

            public new object? Handle(IMessage<object> message, T target)
            {
                if (ReplayToken.IsReplay(message))
                {
                    return null;
                }

                return base.Handle(message, target);
            }
        }
    }
}