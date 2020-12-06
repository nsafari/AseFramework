using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ase.Messaging.Messaging.Annotation
{
    /// <summary>
    /// Interface describing a handler for specific messages targeting entities of a specific type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMessageHandlingMember<in T>
    {
        /// <summary>
        /// Returns the payload type of messages that can be processed by this handler.
        /// </summary>
        /// <returns>The payload type of messages expected by this handler</returns>
        Type PayloadType();

        /// <summary>
        /// Returns a number representing the priority of this handler over other handlers capable of processing the same
        /// message.
        /// <p>
        /// In general, a handler with a higher priority will receive the message before (or instead of) handlers with a
        /// lower priority. However, the priority value may not be the only indicator that is used to determine the order of
        /// invocation. For instance, a message processor may decide to ignore the priority value if one message handler is a
        /// more specific handler of the message than another handler.
        /// </summary>
        /// <returns>Number indicating the priority of this handler over other handlers</returns>
        int Priority();

        /// <summary>
        /// Checks if this handler is capable of handling messages with the given {@code payloadType}.
        /// </summary>
        /// <param name="message">The message that is to be handled</param>
        /// <returns>{@code true} if the handler is capable of handling the message, {@code false} otherwise</returns>
        bool CanHandle(IMessage<object> message);

        /// <summary>
        /// Checks if this handler is capable of handling messages with the given {@code payloadType}.
        /// </summary>
        /// <param name="payloadType">The payloadType of a message that is to be handled</param>
        /// <returns>{@code true} if the handler is capable of handling the message with given type, {@code false} otherwise</returns>
        bool CanHandleType(Type payloadType)
        {
            return true;
        }

        /// <summary>
        /// Handles the given {@code message} by invoking the appropriate method on given {@code target}. This may result in
        /// an exception if the given target is not capable of handling the message or if an exception is thrown during
        /// invocation of the method.
        /// </summary>
        /// <param name="message">The message to handle</param>
        /// <param name="target">The target to handle the message</param>
        /// <returns>The message handling result in case the invocation was successful</returns>
        object? Handle(IMessage<object> message, T target);

        /// <summary>
        /// Returns the wrapped handler object if its type is an instance of the given {@code handlerType}. For instance, if
        /// this method is invoked with {@link java.lang.reflect.Executable} and the message is handled by a method of the
        /// target entity, then this method will return the method handle as a {@link java.lang.reflect.Method}.
        /// </summary>
        /// <typeparam name="THandler">The wrapped handler type</typeparam>
        /// <returns>An Optional containing the wrapped handler object or an empty Optional if the handler is not an
        /// instance of the given handlerType</returns>
        THandler? Unwrap<THandler>()
            where THandler : class;

        /// <summary>
        /// Checks whether the method of the target entity contains the given {@code annotationType}.
        /// </summary>
        /// <typeparam name="TAttribute">Annotation to check for on the target method</typeparam>
        /// <returns>{@code true} if the annotation is present on the target method, {@code false} otherwise</returns>
        bool HasAnnotation<TAttribute>() where TAttribute : Attribute;

        /// <summary>
        /// Get the attributes of an annotation of given {@code annotationType} on the method of the target entity. If the
        /// annotation is present on the target method an Optional is returned containing the properties mapped by their
        /// name. If the annotation is not present an empty Optional is returned.
        /// </summary>
        /// <typeparam name="TAttribute">The annotation to check for on the target method</typeparam>
        /// <returns>An optional containing a map of the properties of the annotation, or an empty optional if the
        /// annotation is missing on the method</returns>
        IDictionary<string, object?>? AnnotationAttributes<TAttribute>() where TAttribute : Attribute;
    }
}