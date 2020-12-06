#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Ase.Messaging.CommandHandling;
using Ase.Messaging.Messaging;
using Ase.Messaging.Messaging.Annotation;
using Ase.Messaging.Messaging.UnitOfWork;

namespace AseFramework.Modeling.Command.Inspection
{
    /// <summary>
    /// Implementation of a {@link CommandMessageHandlingMember} that forwards commands to a child entity.
    /// </summary>
    /// <typeparam name="TParent">the parent entity type</typeparam>
    /// <typeparam name="TChild">the child entity type</typeparam>
    public class
        ChildForwardingCommandMessageHandlingMember<TParent, TChild> : ICommandMessageHandlingMember<TParent>
    {
        private readonly List<IMessageHandlingMember<TChild>> _childHandlingInterceptors;
        private readonly IMessageHandlingMember<TChild> _childHandler;
        private readonly Func<ICommandMessage<object>, TParent, TChild> _childEntityResolver;

        private readonly string _commandName;
        private readonly bool _isFactoryHandler;

        /// <summary>
        /// Initializes a {@link ChildForwardingCommandMessageHandlingMember} that routes commands to a compatible child
        /// entity. Child entities are resolved using the given {@code childEntityResolver}. If an entity is found the
        /// command will be handled using the given {@code childHandler}.
        /// </summary>
        /// <param name="childHandlerInterceptors">interceptors for {@code childHandler}</param>
        /// <param name="childHandler">handler of the command once a suitable entity is found</param>
        /// <param name="childEntityResolver">resolver of child entities for a given command</param>
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
                    .Unwrap<ICommandMessageHandlingMember<TParent>>();
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

            object? result;
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
                        m => _childHandler.Handle(message, childEntity)!
                    )
                ).Proceed();
            }

            return result;
        }

        public THandler? Unwrap<THandler>()
            where THandler : class
        {
            if (this is THandler)
            {
                return this as THandler;
            }

            return _childHandler.Unwrap<THandler>();
        }

        public bool HasAnnotation<TAttribute>()
            where TAttribute : Attribute
        {
            return _childHandler.HasAnnotation<TAttribute>();
        }

        public IDictionary<string, object?>? AnnotationAttributes<TAttribute>()
            where TAttribute : Attribute
        {
            return _childHandler.AnnotationAttributes<TAttribute>();
        }
    }
}