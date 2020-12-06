using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ase.Messaging.Common.Annotation;
using NHibernate;

namespace Ase.Messaging.Messaging.Annotation
{
    /// <summary>
    /// Implementation of a {@link MessageHandlingMember} that is used to invoke message handler methods on the target type.
    /// </summary>
    /// <typeparam name="T">the target type</typeparam>
    public class AnnotatedMessageHandlingMember<T> : IMessageHandlingMember<T>
    {
        private readonly Type _payloadType;
        private readonly int _parameterCount;
        private readonly List<IParameterResolver<object>?> _parameterResolvers;
        private readonly MemberInfo _executable;
        private readonly Type _messageType;

        /// <summary>
        /// Initializes a new instance that will invoke the given {@code executable} (method) on a target to handle a message
        /// of the given {@code messageType}.
        /// </summary>
        /// <param name="executable">the method to invoke on a target</param>
        /// <param name="messageType">the type of message that is expected by the target method</param>
        /// <param name="explicitPayloadType">the expected message payload type</param>
        /// <param name="parameterResolverFactory">factory used to resolve method parameters</param>
        /// <exception cref="UnsupportedHandlerException"></exception>
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

        public bool CanHandleType(Type payloadType)
        {
            return _payloadType.IsAssignableFrom(payloadType);
        }

        /// <summary>
        /// Checks if this member can handle the type of the given {@code message}. This method does not check if the
        /// parameter resolvers of this member are compatible with the given message. Use {@link #parametersMatch(Message)}
        /// for that.
        /// </summary>
        /// <param name="message">the message to check for</param>
        /// <returns>{@code true} if this member can handle the message type. {@code false} otherwise</returns>
        protected bool TypeMatches(IMessage<object> message)
        {
            return _messageType.IsInstanceOfType(message);
        }

        /// <summary>
        /// Checks if the parameter resolvers of this member are compatible with the given {@code message}.
        /// </summary>
        /// <param name="message">the message to check for</param>
        /// <returns>{@code true} if the parameter resolvers can handle this message. {@code false} otherwise</returns>
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

        public IDictionary<string, object?>? AnnotationAttributes<TAttribute>()
            where TAttribute : Attribute
        {
            return AnnotationUtils.FindAnnotationAttributes<TAttribute>(_executable);
        }

        public bool HasAnnotation<TAttribute>()
            where TAttribute : Attribute
        {
            return AnnotationUtils.IsAnnotationPresent<TAttribute>(_executable);
        }

        public THandler? Unwrap<THandler>()
            where THandler : class
        {
            return (_executable is THandler ? _executable : null) as THandler;
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