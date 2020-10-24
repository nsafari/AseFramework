using System;

namespace Ase.Messaging.Messaging.Annotation
{
    /// <summary>
    /// Interface for a mechanism that resolves handler method parameter values from a given {@link Message}.
    /// 
    /// </summary>
    /// <typeparam name="T">The type of parameter returned by this resolver</typeparam>
    public interface IParameterResolver<T>
    {
        /// <summary>
        /// Resolves the parameter value to use for the given {@code message}, or {@code null} if no suitable
        /// parameter value can be resolved.
        /// </summary>
        /// <param name="message">The message to resolve the value from</param>
        /// <returns>the parameter value for the handler</returns>
        T ResolveParameterValue<TMessage, TPayload>(IMessage<object> message)
            where TMessage : IMessage<TPayload> where TPayload : class;

        /// <summary>
        /// Indicates whether this resolver is capable of providing a value for the given {@code message}.
        /// </summary>
        /// <param name="message">The message to evaluate</param>
        /// <returns>{@code true} if this resolver can provide a value for the message, otherwise {@code false}</returns>
        bool Matches<TPayload>(IMessage<TPayload> message) 
            where TPayload : class;

        /// <summary>
        /// Returns the class of the payload that is supported by this resolver. Defaults to the {@link Object} class
        /// indicating that the payload type is irrelevant for this resolver.
        /// </summary>
        /// <returns>The class of the payload that is supported by this resolver</returns>
        Type SupportedPayloadType()
        {
            return typeof(object);
        }
    }
}