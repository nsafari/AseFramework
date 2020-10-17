namespace Ase.Messaging.Messaging
{
    /// <summary>
    /// The interceptor chain manages the flow of a message through a chain of interceptors and ultimately to the message
    /// handler. Interceptors may continue processing via this chain by calling the {@link #proceed()} method.
    /// Alternatively, they can block processing by returning without calling either of these methods.
    /// </summary>
    public interface IInterceptorChain
    {
        
        /// <summary>
        /// Signals the Interceptor Chain to continue processing the message.
        /// </summary>
        /// <returns>The return value of the message processing</returns>
        object Proceed();
    }
}