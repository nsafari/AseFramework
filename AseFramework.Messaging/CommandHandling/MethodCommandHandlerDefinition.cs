using System;
using System.Collections.Generic;
using System.Reflection;
using Ase.Messaging.Annotation;
using Ase.Messaging.Common;
using Ase.Messaging.Messaging;
using Ase.Messaging.Messaging.Annotation;
using NHibernate.Util;

namespace Ase.Messaging.CommandHandling
{
    public class MethodCommandHandlerDefinition : IHandlerEnhancerDefinition
    {
        public IMessageHandlingMember<T> WrapHandler<T>(IMessageHandlingMember<T> original)
            where T : class
        {
            IDictionary<string, object>? annotationAttributes = original.AnnotationAttributes(typeof(CommandHandler));
            return annotationAttributes == null
                ? new MethodCommandMessageHandlingMember<T>(original, annotationAttributes)
                : original;
        }
    }

    internal class MethodCommandMessageHandlingMember<T> : WrappedMessageHandlingMember<T>,
        ICommandMessageHandlingMember<T> where T : class
    {
        private readonly string? _commandName;
        private readonly bool _isFactoryHandler;
        private readonly string? _routingKey;

        internal MethodCommandMessageHandlingMember(
            IMessageHandlingMember<T> @delegate,
            IDictionary<string, object>? annotationAttributes
        ) : base(@delegate)
        {
            _routingKey = "".Equals(annotationAttributes?["routingKey"])
                ? null
                : (string) annotationAttributes?["routingKey"]!;
            MethodBase? executable = @delegate.Unwrap<MethodBase>(typeof(MethodBase) ??
                                                                  throw new AxonConfigurationException(
                                                                      "The @CommandHandler annotation must be put on an Executable (either directly or as Meta " +
                                                                      "Annotation)"));
            if ("".Equals(annotationAttributes?["commandName"]))
            {
                _commandName = @delegate.PayloadType().Name;
            }
            else
            {
                _commandName = (string) annotationAttributes?["commandName"]!;
            }

            bool factoryMethod = executable is MethodInfo && executable.IsStatic;
            if (executable?.DeclaringType != null &&
                factoryMethod &&
                !executable.DeclaringType.IsInstanceOfType(((MethodInfo) executable).ReturnType))
            {
                throw new AxonConfigurationException("static @CommandHandler methods must declare a return value " +
                                                     "which is equal to or a subclass of the declaring type");
            }

            _isFactoryHandler = executable is ConstructorInfo || factoryMethod;
        }

        public new bool CanHandle(IMessage<object> message)
        {
            return _commandName != null &&
                   base.CanHandle(message) &&
                   _commandName.Equals(((ICommandMessage<T>) message).CommandName());
        }

        public string? RoutingKey()
        {
            return _routingKey;
        }

        public string? CommandName()
        {
            return _commandName;
        }

        public bool IsFactoryHandler()
        {
            return _isFactoryHandler;
        }
    }
}