using System;
using System.Collections.Generic;
using Ase.Messaging.Annotation;
using Ase.Messaging.CommandHandling;
using Ase.Messaging.Messaging;
using Ase.Messaging.Messaging.UnitOfWork;

namespace AseFramework.Modeling.Command.Inspection
{
    public class ChildForwardingCommandMessageHandlingMember<P, C> : ICommandMessageHandlingMember<P>
    {
        private readonly List<IMessageHandlingMember<C>> _childHandlingInterceptors;
        private readonly IMessageHandlingMember<C> _childHandler;
        private readonly Func<ICommandMessage<object>, P, C> _childEntityResolver;

        public ChildForwardingCommandMessageHandlingMember(
            List<IMessageHandlingMember<C>> childHandlerInterceptors,
            IMessageHandlingMember<C> childHandler,
            Func<ICommandMessage<object>, P, C> childEntityResolver
        )
        {
            _childHandlingInterceptors = childHandlerInterceptors;
            _childHandler = childHandler;
            _childEntityResolver = childEntityResolver;
            var commandMessageHandlingMember =
                childHandler
                    .Unwrap<ICommandMessageHandlingMember<P>>(typeof(ICommandMessageHandlingMember<>));
            CommandName = commandMessageHandlingMember?.CommandName();
            IsFactoryHandler = commandMessageHandlingMember?.IsFactoryHandler() ?? false;
        }

        public string CommandName { get; }


        public string RoutingKey()
        {
            return null;
        }

        public bool IsFactoryHandler { get; }

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

        public object Handle(IMessage<object> message, P target)
        {
            C childEntity = _childEntityResolver.Invoke((ICommandMessage<object>) message, target);
            if (childEntity == null) {
                throw new AggregateEntityNotFoundException(
                    "Aggregate cannot handle this command, as there is no entity instance to forward it to."
                );
            }
            List<AnnotatedCommandHandlerInterceptor<? super C>> interceptors =
                childHandlingInterceptors.stream()
                    .filter(chi -> chi.canHandle(message))
                    .sorted((chi1, chi2) -> Integer.compare(chi2.priority(), chi1.priority()))
                    .map(chi -> new AnnotatedCommandHandlerInterceptor<>(chi, childEntity))
                .collect(Collectors.toList());

            Object result;
            if (interceptors.isEmpty()) {
                result = childHandler.handle(message, childEntity);
            } else {
                result = new DefaultInterceptorChain<>((UnitOfWork<CommandMessage<?>>) CurrentUnitOfWork<,>.get(),
                interceptors,
                m -> childHandler.handle(message, childEntity)).proceed();
            }
            return result;
        }

        public T? Unwrap<T>(Type handlerType) 
            where T : class
        {
            if (handlerType.IsInstanceOfType(this)) {
                return this as T;
            }
            return _childHandler.Unwrap<T>(handlerType);
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