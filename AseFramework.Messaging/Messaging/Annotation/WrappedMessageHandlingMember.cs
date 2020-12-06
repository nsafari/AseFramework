using System;
using System.Collections.Generic;

namespace Ase.Messaging.Messaging.Annotation
{
    /// <summary>
    /// Abstract implementation of a {@link MessageHandlingMember} that delegates to a wrapped MessageHandlingMember. Extend
    /// this class to provide additional functionality to the delegate member.
    /// </summary>
    /// <typeparam name="T">the entity type</typeparam>
    public class WrappedMessageHandlingMember<T>: IMessageHandlingMember<T>
    {
        private readonly IMessageHandlingMember<T> _delegate;

        /// <summary>
        /// Initializes the member using the given {@code delegate}.
        /// </summary>
        /// <param name="delegate">delegate the actual message handling member to delegate to</param>
        protected WrappedMessageHandlingMember(IMessageHandlingMember<T> @delegate) {
            
            _delegate = @delegate;
        }

        public Type PayloadType()
        {
            return _delegate.PayloadType();
        }

        public int Priority()
        {
            return _delegate.Priority();
        }

        public bool CanHandle(IMessage<object> message)
        {
            return _delegate.CanHandle(message);
        }

        public object? Handle(IMessage<object> message, T target)
        {
            return _delegate.Handle(message, target);
        }

        public THandler? Unwrap<THandler>() where THandler : class
        {
            if (this is THandler) {
                return this as THandler;
            }
            return _delegate.Unwrap<THandler>();
        }

        public IDictionary<string, object?>? AnnotationAttributes<TAttribute>()
            where TAttribute : Attribute
        {
            return _delegate.AnnotationAttributes<TAttribute>();
        }

        public bool HasAnnotation<TAttribute>()
            where TAttribute : Attribute
        {
            return _delegate.HasAnnotation<TAttribute>();
        }
    }
}