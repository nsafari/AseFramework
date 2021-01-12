using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ase.Messaging.CommandHandling;
using Ase.Messaging.EventHandling;
using Ase.Messaging.Messaging.Annotation;
using NHibernate.Util;

namespace AseFramework.Modeling.Command.Inspection
{
    /// <summary>
    /// Implementation of a {@link ChildEntity} that uses annotations on a target entity to resolve event and command
    /// handlers.
    /// </summary>
    /// <typeparam name="TParent">the parent entity type.</typeparam>
    /// <typeparam name="TChild">the child entity type.</typeparam>
    public class AnnotatedChildEntity<TParent, TChild> : IChildEntity<TParent>
    {
        private readonly IEntityModel<TChild> _entityModel;
        private readonly IList<IMessageHandlingMember<TParent>> _commandHandlers;
        private readonly Func<IEventMessage<object>, TParent, List<TChild>> _eventTargetResolver;


        /// <summary>
        /// Initiates a new AnnotatedChildEntity instance that uses the provided {@code entityModel} to delegate command
        /// and event handling to an annotated child entity.
        /// </summary>
        /// <param name="entityModel">A {@link EntityModel} describing the entity.</param>
        /// <param name="forwardCommands">Flag indicating whether commands should be forwarded to the entity.</param>
        /// <param name="commandTargetResolver">Resolver for command handler methods on the target.</param>
        /// <param name="eventTargetResolver">Resolver for event handler methods on the target.</param>
        public AnnotatedChildEntity(
            IEntityModel<TChild> entityModel,
            bool forwardCommands,
            Func<ICommandMessage<object>, TParent, TChild> commandTargetResolver,
            Func<IEventMessage<object>, TParent, List<TChild>> eventTargetResolver
        )
        {
            _entityModel = entityModel;
            _eventTargetResolver = eventTargetResolver;
            _commandHandlers = new List<IMessageHandlingMember<TParent>>();
            if (forwardCommands)
            {
                var messageHandlingMembers =
                    entityModel.CommandHandlers<TChild>()
                        .Where(eh =>
                            eh.Unwrap<ICommandMessageHandlingMember<TParent>>() !=
                            null);

                foreach (var childHandler in messageHandlingMembers)
                    _commandHandlers
                        .Add(new ChildForwardingCommandMessageHandlingMember<TParent, TChild>(
                                entityModel.CommandHandlerInterceptors<TChild>(),
                                childHandler,
                                commandTargetResolver
                            )
                        );
            }
        }


        public void Publish(IEventMessage<object> msg, TParent declaringInstance)
        {
            var targetList = _eventTargetResolver(msg, declaringInstance);
            targetList.GetRange(0, targetList.Count) // Creates copy to prevent ConcurrentModificationException.
                .ForEach(target => _entityModel.Publish(msg, target));
        }

        public IList<IMessageHandlingMember<TParent>> CommandHandlers()
        {
            return _commandHandlers;
        }
    }
}