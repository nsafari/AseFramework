using Ase.Messaging.Annotation;

namespace AseFramework.Modeling.Command.Inspection
{
    /// <summary>
    /// Interface specifying a message handler capable of intercepting a command.
    /// </summary>
    /// <typeparam name="T">the type of entity to which the message handler will delegate tha actual interception</typeparam>
    public interface ICommandHandlerInterceptorHandlingMember<T> : IMessageHandlingMember<T>
    {
        
        /// <summary>
        /// Indicates whether interceptor chain (containing a command handler) should be invoked automatically or command
        /// handler interceptor will invoke it manually.
        /// </summary>
        /// <returns>{@code true} if interceptor chain should be invoked automatically</returns>
        bool ShouldInvokeInterceptorChain();

        
    }
}