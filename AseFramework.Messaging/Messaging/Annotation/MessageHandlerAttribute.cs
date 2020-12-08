using System;

namespace Ase.Messaging.Messaging.Annotation
{
    /// <summary>
    /// Annotation indicating that a member method should be able to respond to {@link Message}s.
    /// <p>
    /// It is not recommended to put this annotation on methods or constructors directly. Instead, put this annotation on
    /// another annotation that expresses the type of message handled.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Constructor)]
    public class MessageHandlerAttribute : Attribute
    {
        /// <summary>
        /// Specifies the type of message that can be handled by the member method. Defaults to any {@link Message}.
        /// </summary>
        /// <returns></returns>
        public Type? MessageType { get; set; } = typeof(IMessage<>);

        /// <summary>
        /// Specifies the type of message payload that can be handled by the member method. The payload of the message should
        /// be assignable to this type. Defaults to any {@link Object}.
        /// </summary>
        /// <returns></returns>
        public Type PayloadType { get; set; } = typeof(object);
    }
}