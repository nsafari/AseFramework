using System.Reflection;
using Ase.Messaging.Common;

namespace Ase.Messaging.Messaging.Annotation
{
    /// <summary>
    /// Thrown when an @...Handler annotated method was found that does not conform to the rules that apply to those
    /// methods.
    /// </summary>
    public class UnsupportedHandlerException : AseConfigurationException
    {
        private readonly MemberInfo? _violatingMethod;

        /// <summary>
        /// Initialize the exception with a {@code message} and the {@code violatingMethod}.
        /// </summary>
        /// <param name="message">a descriptive message of the violation</param>
        /// <param name="violatingMethod">the method that violates the rules of annotated Event Handlers</param>
        public UnsupportedHandlerException(string message, MemberInfo? violatingMethod) : base(message)
        {
            _violatingMethod = violatingMethod;
        }
        
        /// <summary>
        /// A reference to the method that violated the event handler rules.
        /// </summary>
        /// <returns>the method that violated the event handler rules</returns>
        public MemberInfo? GetViolatingMethod() {
            return _violatingMethod;
        }
    }
}