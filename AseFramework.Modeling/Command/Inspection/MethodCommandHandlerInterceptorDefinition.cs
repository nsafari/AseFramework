using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Ase.Messaging.CommandHandling;
using Ase.Messaging.Common;
using Ase.Messaging.Messaging;
using Ase.Messaging.Messaging.Annotation;

namespace AseFramework.Modeling.Command.Inspection
{
    /// <summary>
    /// Implementation of {@link HandlerEnhancerDefinition} used for {@link CommandHandlerInterceptor} annotated methods.
    /// </summary>
    public class MethodCommandHandlerInterceptorDefinition : IHandlerEnhancerDefinition
    {
        public IMessageHandlingMember<T> WrapHandler<T>(IMessageHandlingMember<T> original) where T : class
        {
            var annotationAttributes = original.AnnotationAttributes<CommandHandlerInterceptorAttribute>();
            return annotationAttributes != null
                ? new MethodCommandHandlerInterceptorHandlingMember<T>(original, annotationAttributes)
                : original;
        }

        private class MethodCommandHandlerInterceptorHandlingMember<T> : WrappedMessageHandlingMember<T>,
            ICommandHandlerInterceptorHandlingMember<T>
        {
            private readonly Regex _commandNamePattern;
            private readonly bool _shouldInvokeInterceptorChain;

            /// <summary>
            /// Initializes the member using the given {@code delegate}.
            /// </summary>
            /// <param name="delegate">the actual message handling member to delegate to</param>
            /// <param name="annotationAttributes"></param>
            /// <exception cref="AxonConfigurationException"></exception>
            internal MethodCommandHandlerInterceptorHandlingMember(IMessageHandlingMember<T> @delegate,
                IDictionary<string, object> annotationAttributes) : base(@delegate)
            {
                MethodInfo method = @delegate.Unwrap<MethodInfo>() ??
                                    throw new AxonConfigurationException(
                                        "The @CommandHandlerInterceptor must be on method.");
                _shouldInvokeInterceptorChain = method.GetParameters()
                    .Any(p => p.GetType() == typeof(IInterceptorChain));
                if (_shouldInvokeInterceptorChain && typeof(Void) != method.ReturnType)
                {
                    throw new AxonConfigurationException("@CommandHandlerInterceptor must return void or declare " +
                                                         "InterceptorChain parameter.");
                }

                _commandNamePattern = new Regex((string) annotationAttributes["commandNamePattern"]);
            }

            public bool ShouldInvokeInterceptorChain()
            {
                return _shouldInvokeInterceptorChain;
            }

            public new bool CanHandle(IMessage<object> message)
            {
                return base.CanHandle(message) &&
                       _commandNamePattern.IsMatch(((ICommandMessage<object>) message).CommandName());
            }
        }
    }
}