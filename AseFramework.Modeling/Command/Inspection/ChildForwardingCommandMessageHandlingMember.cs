using System;
using System.Collections.Generic;
using Ase.Messaging.Annotation;
using Ase.Messaging.CommandHandling;

namespace AseFramework.Modeling.Command.Inspection
{
    public class ChildForwardingCommandMessageHandlingMember<P, C> : ICommandMessageHandlingMember<P>
    {
        private readonly List<IMessageHandlingMember<C>> childHandlingInterceptors;
        private readonly IMessageHandlingMember<C> childHandler;
        private readonly Func<ICommandMessage<object>, P, C> childEntityResolver;
        private readonly string commandName;
        private readonly bool isFactoryHandler;

        public ChildForwardingCommandMessageHandlingMember(
            List<IMessageHandlingMember<C>> childHandlerInterceptors,
            IMessageHandlingMember<C> childHandler,
            Func<ICommandMessage<object>, P, C> childEntityResolver
        )
        {
            childHandlingInterceptors = childHandlerInterceptors;
            this.childHandler = childHandler;
            this.childEntityResolver = childEntityResolver;
            var commandMessageHandlingMember =
                childHandler
                    .Unwrap<ICommandMessageHandlingMember<P>>(typeof(ICommandMessageHandlingMember<>));
            commandName = commandMessageHandlingMember?.CommandName();
            isFactoryHandler = commandMessageHandlingMember?.IsFactoryHandler() ?? false;
        }
    }
}