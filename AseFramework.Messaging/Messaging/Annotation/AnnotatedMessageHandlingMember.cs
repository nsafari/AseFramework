using System;
using System.Reflection;
using Ase.Messaging.Common;

namespace Ase.Messaging.Messaging.Annotation
{
    public class AnnotatedMessageHandlingMember<T> : IMessageHandlingMember<T>
    {
        private readonly Type payloadType;
        private readonly int parameterCount;
        private readonly IParameterResolver<object>[] parameterResolvers;
        private readonly MethodBase executable;
        private readonly Type messageType;

        public AnnotatedMessageHandlingMember(
            MethodBase executable,
            Type messageType,
            Type explicitPayloadType,
            IParameterResolverFactory parameterResolverFactory)
        {
            this.executable = executable;
            this.messageType = messageType;
            ReflectionUtils.ensureAccessible(this.executable);
            Parameter[] parameters = executable.getParameters();
            this.parameterCount = executable.getParameterCount();
            parameterResolvers = new ParameterResolver[parameterCount];
            Class < ?> supportedPayloadType = explicitPayloadType;
            for (int i = 0; i < parameterCount; i++)
            {
                parameterResolvers[i] = parameterResolverFactory.createInstance(executable, parameters, i);
                if (parameterResolvers[i] == null)
                {
                    throw new UnsupportedHandlerException(
                        "Unable to resolve parameter " + i + " (" + parameters[i].getType().getSimpleName() +
                        ") in handler " + executable.toGenericString() + ".", executable);
                }

                if (supportedPayloadType.isAssignableFrom(parameterResolvers[i].supportedPayloadType()))
                {
                    supportedPayloadType = parameterResolvers[i].supportedPayloadType();
                }
                else if (!parameterResolvers[i].supportedPayloadType().isAssignableFrom(supportedPayloadType))
                {
                    throw new UnsupportedHandlerException(String.format(
                        "The method %s seems to have parameters that put conflicting requirements on the payload type" +
                        " applicable on that method: %s vs %s", executable.toGenericString(),
                        supportedPayloadType, parameterResolvers[i].supportedPayloadType()), executable);
                }
            }

            this.payloadType = supportedPayloadType;
        }
    }
}