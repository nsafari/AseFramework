using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ase.Messaging.Common;
using Ase.Messaging.Common.Annotation;
using NHibernate;
using NHibernate.Mapping.ByCode.Impl;

namespace Ase.Messaging.Messaging.Annotation
{
    public class AnnotatedMessageHandlingMember<T> : IMessageHandlingMember<T>
    {
        private readonly Type _payloadType;
        private readonly int _parameterCount;
        private readonly List<IParameterResolver<object>?> _parameterResolvers;
        private readonly MethodBase _executable;
        private readonly Type _messageType;

        public AnnotatedMessageHandlingMember(
            MethodBase executable,
            Type messageType,
            Type explicitPayloadType,
            IParameterResolverFactory parameterResolverFactory)
        {
            this._executable = executable;
            this._messageType = messageType;
            ParameterInfo[] parameters = executable.GetParameters();
            _parameterCount = executable.GetParameters().Length;
            _parameterResolvers = new List<IParameterResolver<object>?>();
            Type supportedPayloadType = explicitPayloadType;
            for (int i = 0; i < _parameterCount; i++)
            {
                _parameterResolvers[i] = parameterResolverFactory.CreateInstance<object>(executable, parameters, i);
                if (_parameterResolvers[i] == null)
                {
                    throw new UnsupportedHandlerException(
                        "Unable to resolve parameter " + i + " (" + parameters[i].ParameterType.Name +
                        ") in handler " + executable + ".", executable);
                }

                if (supportedPayloadType.IsAssignableFrom(_parameterResolvers[i]!.SupportedPayloadType()))
                {
                    supportedPayloadType = _parameterResolvers[i]!.SupportedPayloadType();
                }
                else if (!_parameterResolvers[i]!.SupportedPayloadType().IsAssignableFrom(supportedPayloadType))
                {
                    throw new UnsupportedHandlerException(string.Format(
                        "The method {0} seems to have parameters that put conflicting requirements on the payload type" +
                        " applicable on that method: {1} vs {2}", executable,
                        supportedPayloadType, _parameterResolvers[i]!.SupportedPayloadType()
                    ), executable);
                }
            }

            _payloadType = supportedPayloadType;
        }

        public Type PayloadType()
        {
            return _payloadType;
        }

        public int Priority()
        {
            return _parameterCount;
        }

        public bool CanHandle(IMessage<object> message)
        {
            return TypeMatches(message) && _payloadType.IsAssignableFrom(message.GetPayloadType()) &&
                   ParametersMatch(message);
        }

        protected bool TypeMatches(IMessage<object> message)
        {
            return _messageType.IsInstanceOfType(message);
        }

        protected bool ParametersMatch(IMessage<object> message)
        {
            return _parameterResolvers.All(resolver => resolver == null || resolver.Matches(message));
        }

        public object? Handle(IMessage<object> message, T target)
        {
            try
            {
                if (_executable is MethodInfo)
                {
                    return ((MethodInfo) _executable).Invoke(target, ResolveParameterValues(message));
                }
                else if (_executable is ConstructorInfo)
                {
                    return ((ConstructorInfo) _executable).Invoke(ResolveParameterValues(message));
                }
                else
                {
                    throw new InvalidOperationException("What kind of handler is this?");
                }
            }
            catch (Exception ex) when (
                ex is FieldAccessException ||
                ex is TargetInvocationException ||
                ex is InstantiationException)
            {
                throw new MessageHandlerInvocationException(
                    $"Error handling an object of type {message.GetPayloadType()}", ex);
            }
        }

        public IDictionary<string, object?>? AnnotationAttributes(Attribute attribute)
        {
            return AnnotationUtils.FindAnnotationAttributes(_executable, attribute);
        }

        public bool HasAnnotation(Attribute attribute)
        {
            return AnnotationUtils.IsAnnotationPresent(_executable, attribute);
        }

        public MethodBase? Unwrap<THandler>(Type handlerType)
            where THandler : class
        {
            return handlerType.IsInstanceOfType(_executable) ? _executable : null;
        }

        public override string ToString()
        {
            return GetType().Name + " " + _executable;
        }

        private object?[] ResolveParameterValues(IMessage<object> message)
        {
            object?[] @params = new object[_parameterCount];
            for (int i = 0; i < _parameterCount; i++)
            {
                @params[i] = _parameterResolvers[i]?.ResolveParameterValue<IMessage<object>, object>(message);
            }

            return @params;
        }
    }
}