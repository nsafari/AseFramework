using System;
using Ase.Messaging.Annotation;
using Ase.Messaging.CommandHandling;
using Ase.Messaging.Messaging;
using Ase.Messaging.Messaging.UnitOfWork;

namespace AseFramework.Modeling.Command.Inspection
{
    /// <summary>
    /// Annotated command handler interceptor on aggregate. Will invoke the delegate to the real interceptor method.
    /// </summary>
    /// <typeparam name="TTarget">The type of entity to which the message handler will delegate the actual handling of the message</typeparam>
    /// <typeparam name="TPayload">The type of payload that has been passed to the command message</typeparam>
    public class AnnotatedCommandHandlerInterceptor<TTarget, TPayload>
        : IMessageHandlerInterceptor<ICommandMessage<TPayload>, TPayload>
        where TPayload : class
    {
        private readonly IMessageHandlingMember<TTarget> _delegate;
        private readonly TTarget _target;

        /// <summary>
        /// Initializes annotated command handler interceptor with delegate handler and target on which handler is to be
        /// invoked.
        /// </summary>
        /// <param name="delegate">delegate command handler interceptor</param>
        /// <param name="target">on which command handler interceptor is to be invoked</param>
        public AnnotatedCommandHandlerInterceptor(IMessageHandlingMember<TTarget> @delegate, TTarget target)
        {
            _delegate = @delegate;
            _target = target;
        }

        public object Handle(IUnitOfWork<ICommandMessage<TPayload>, TPayload> unitOfWork,
            IInterceptorChain interceptorChain)
        {
            InterceptorChainParameterResolverFactory.Initialize<ICommandMessage<TPayload>, TPayload>(interceptorChain);

            object result = _delegate.Handle(unitOfWork.GetMessage(), _target);

            if (_delegate
                .Unwrap<ICommandHandlerInterceptorHandlingMember<TTarget>>(
                    typeof(ICommandHandlerInterceptorHandlingMember<>)
                )?.ShouldInvokeInterceptorChain() ?? false)
            {
                result = interceptorChain.Proceed();
            }

            return result;
        }
    }
}