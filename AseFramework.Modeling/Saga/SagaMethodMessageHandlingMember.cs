using System;
using System.Collections.Generic;
using Ase.Messaging.EventHandling;
using Ase.Messaging.Messaging;
using Ase.Messaging.Messaging.Annotation;

namespace AseFramework.Modeling.Saga
{
    /// <summary>
    /// Abstract implementation of a {@link MessageHandlingMember} that delegates to a wrapped MessageHandlingMember. Extend
    /// this class to provide additional functionality to the delegate member.
    /// </summary>
    /// <typeparam name="T">the entity type</typeparam>
    public class SagaMethodMessageHandlingMember<T> : WrappedMessageHandlingMember<T>
    {
        private readonly IMessageHandlingMember<T> _delegate;
        private readonly SagaCreationPolicy _creationPolicy;
        private readonly string _associationKey;
        private readonly string _associationPropertyName;
        private readonly IAssociationResolver _associationResolver;
        private readonly bool _endingHandler;

        /// <summary>
        /// Initializes the member using the given {@code delegate}.
        /// </summary>
        /// <param name="delegate">the actual message handling member to delegate to</param>
        public SagaMethodMessageHandlingMember(IMessageHandlingMember<T> @delegate, SagaCreationPolicy creationPolicy,
            string associationKey,
            string associationPropertyName,
            IAssociationResolver associationResolver, bool endingHandler) : base(@delegate)
        {
            _delegate = @delegate;
            _creationPolicy = creationPolicy;
            _associationKey = associationKey;
            _associationPropertyName = associationPropertyName;
            _associationResolver = associationResolver;
            _endingHandler = endingHandler;
        }
        
        public AssociationValue GetAssociationValue(IEventMessage<object> eventMessage) {
            object associationValue = _associationResolver.Resolve(_associationPropertyName, eventMessage, this);
            return new AssociationValue(_associationKey, associationValue.ToString());
        }


        public override object? Handle(IMessage<object> message, T target)
        {
            return _delegate.Handle(message, target);
        }

        public override THandler? Unwrap<THandler>() 
            where THandler : class
        {
            if (this is THandler) {
                return this as THandler;
            }
            return _delegate.Unwrap<THandler>();        
        }

        public override IDictionary<string, object?>? AnnotationAttributes<TAttribute>()
        {
            return _delegate.AnnotationAttributes<TAttribute>();
        }

        public override bool HasAnnotation<TAttribute>()
        {
            return _delegate.HasAnnotation<TAttribute>();
        }
    }
}