using System;
using System.Reflection;

namespace Ase.Messaging.Messaging.Annotation
{
    /// <summary>
    /// Interface that describes an object capable of inspecting a method to determine if the method is suitable for message
    /// handling. If the method is suitable the definition returns a {@link MessageHandler} instance to invoke the method.
    /// </summary>
    public interface IHandlerDefinition
    {
        /// <summary>
        /// Create a {@link MessageHandlingMember} for the given {@code executable} method. Use the given {@code
        /// parameterResolverFactory} to resolve the method's parameters.
        /// </summary>
        /// <param name="declaringType">The type of object declaring the given executable method</param>
        /// <param name="executable">The method to inspect</param>
        /// <param name="parameterResolverFactory">Factory for a {@link ParameterResolver} of the method</param>
        /// <typeparam name="T">The type of the declaring object</typeparam>
        /// <returns>An optional containing the handler if the method is suitable, or an empty Nullable otherwise</returns>
        IMessageHandlingMember<T> CreateHandler<T>(Type declaringType,
            MethodInfo executable,
            IParameterResolverFactory parameterResolverFactory);

    }
}