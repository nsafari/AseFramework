using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ase.Messaging.Annotation;
using Ase.Messaging.CommandHandling;
using Ase.Messaging.EventHandling;

namespace AseFramework.Modeling.Command.Inspection
{
    public class AnnotatedChildEntity<P, C> : IChildEntity<P>
    {
        private readonly IEntityModel<C> _entityModel;
        private readonly IList<IMessageHandlingMember<P>> _commandHandlers;
        private readonly Func<IEventMessage<object>, P, Stream> _eventTargetResolver;


        public AnnotatedChildEntity(
            IEntityModel<C> entityModel,
            bool forwardCommands,
            Func<ICommandMessage<object>, P, C> commandTargetResolver,
            Func<IEventMessage<object>, P, Stream> eventTargetResolver
        )
        {
            _entityModel = entityModel;
            _eventTargetResolver = eventTargetResolver;
            _commandHandlers = new List<IMessageHandlingMember<P>>();
            if (forwardCommands)
            {
                entityModel.CommandHandlers<C>()
                    .Where(eh=>eh.Unwrap<>(ICommandMessageHandlingMember.class).isPresent())
                    .forEach(
                    (childHandler)->commandHandlers
                    .add(new ChildForwardingCommandMessageHandlingMember<>(
                        entityModel.commandHandlerInterceptors(),
                        childHandler,
                        commandTargetResolver)));
            }
        }
    }
}