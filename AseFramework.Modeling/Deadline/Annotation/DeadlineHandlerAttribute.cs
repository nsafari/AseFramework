using System;

namespace AseFramework.Modeling.Deadline.Annotation
{
    /// <summary>
    /// Annotation used to mark handlers which are capable of handling a {@link DeadlineMessage}. It is a specialization of
    /// {@link MessageHandler} were the {@code messageType} is set to DeadlineMessage. Hence, any parameter injections
    /// which works for event handlers work for deadline handlers as well.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class DeadlineHandlerAttribute : Attribute
    {
        /// <summary>
        /// The name of the Deadline this handler listens to. Defaults to the fully qualified class name of the payload type
        /// (i.e. first parameter).
        /// @return The name of the deadline as a {@link String}
        /// </summary>
        private string DeadlineName { get; set; } = "";

        /// <summary>
        /// Specifies the type of message payload that can be handled by the member method. The payload of the message should
        /// be assignable to this type. Defaults to any {@link Object}.
        /// </summary>
        private Type PayloadType { get; set; } = typeof(object);
        
    }
}