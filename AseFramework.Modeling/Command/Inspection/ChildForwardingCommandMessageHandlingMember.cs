#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ase.Messaging.Annotation;
using Ase.Messaging.CommandHandling;
using Ase.Messaging.Messaging;
using Ase.Messaging.Messaging.Annotation;
using Ase.Messaging.Messaging.UnitOfWork;
using NHibernate.Mapping;

namespace AseFramework.Modeling.Command.Inspection
{
    public class
        ChildForwardingCommandMessageHandlingMember<TParent, TChild> : ICommandMessageHandlingMember<TParent>
    {
        private readonly List<IMessageHandlingMember<TChild>> _childHandlingInterceptors;
        private readonly IMessageHandlingMember<TChild> _childHandler;
        private readonly Func<ICommandMessage<object>, TParent, TChild> _childEntityResolver;

        private readonly string _commandName;
        private readonly bool _isFactoryHandler;

        public ChildForwardingCommandMessageHandlingMember(
            List<IMessageHandlingMember<TChild>> childHandlerInterceptors,
            IMessageHandlingMember<TChild> childHandler,
            Func<ICommandMessage<object>, TParent, TChild> childEntityResolver
        )
        {
            _childHandlingInterceptors = childHandlerInterceptors;
            _childHandler = childHandler;
            _childEntityResolver = childEntityResolver;
            var commandMessageHandlingMember =
                childHandler
                    .Unwrap<ICommandMessageHandlingMember<TParent>>(typeof(ICommandMessageHandlingMember<>));
            _commandName = commandMessageHandlingMember?.CommandName();
            _isFactoryHandler = commandMessageHandlingMember?.IsFactoryHandler() ?? false;
        }


        string? ICommandMessageHandlingMember<TParent>.CommandName()
        {
            return _commandName;
        }

        public string? RoutingKey()
        {
            return null;
        }

        bool ICommandMessageHandlingMember<TParent>.IsFactoryHandler()
        {
            return _isFactoryHandler;
        }

        public Type PayloadType()
        {
            return _childHandler.PayloadType();
        }

        public int Priority()
        {
            return Int32.MaxValue;
        }

        public bool CanHandle(IMessage<object> message)
        {
            return _childHandler.CanHandle(message);
        }

        public object? Handle(IMessage<object> message, TParent target)
        {
            TChild childEntity = _childEntityResolver((ICommandMessage<object>) message, target);
            if (childEntity == null)
            {
                throw new AggregateEntityNotFoundException(
                    "Aggregate cannot handle this command, as there is no entity instance to forward it to."
                );
            }

            IList<AnnotatedCommandHandlerInterceptor<TChild, object>> interceptors =
                _childHandlingInterceptors
                    .Where(messageHandlingMember => messageHandlingMember.CanHandle(message))
                    .OrderByDescending((messageHandlingMember) => messageHandlingMember.Priority())
                    .Select(messageHandlingMember =>
                        new AnnotatedCommandHandlerInterceptor<TChild, object>(messageHandlingMember, childEntity)
                    )
                    .ToList();

            object result;
            if (interceptors.Count == 0)
            {
                result = _childHandler.Handle(message, childEntity);
            }
            else
            {
                result = new DefaultInterceptorChain<IMessage<object>, object>(
                    (IUnitOfWork<ICommandMessage<object>, object>) CurrentUnitOfWork<IMessage<object>, object>
                        .Get(),
                    interceptors.Cast<IMessageHandlerInterceptor<IMessage<object>, object>>().ToList(),
                    new MessageHandler<IMessage<object>, object>(
                        m => _childHandler.Handle(message, childEntity)
                    )
                ).Proceed();
            }

            return result;
        }

        public MethodBase? Unwrap<THandler>(Type handlerType)
            where THandler : class
        {
            if (handlerType.IsInstanceOfType(this))
            {
                return this as THandler;
            }

            return _childHandler.Unwrap<THandler>(handlerType);
        }

        public bool HasAnnotation(Type annotationType)
        {
            return _childHandler.HasAnnotation(annotationType);
        }

        public IDictionary<string, object>? AnnotationAttributes(Type annotationType)
        {
            return _childHandler.AnnotationAttributes(annotationType);
        }
        
    }
}