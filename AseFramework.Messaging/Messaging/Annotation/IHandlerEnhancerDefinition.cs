namespace Ase.Messaging.Annotation
{
    /// <summary>
    /// Interface describing objects that are capable of enhancing a {@link MessageHandler}, giving it additional
    /// functionality.
    /// </summary>
    public interface IHandlerEnhancerDefinition
    {
        /// <summary>
        /// Enhance the given {@code original} handler. Implementations may return the original message handler.
        /// </summary>
        /// <param name="original">The original message handler</param>
        /// <typeparam name="T">The type of object that will perform the actual handling of the message</typeparam>
        /// <returns>The enhanced message handler</returns>
        IMessageHandlingMember<T> WrapHandler<T>(IMessageHandlingMember<T> original)
            where T : class;

    }
}