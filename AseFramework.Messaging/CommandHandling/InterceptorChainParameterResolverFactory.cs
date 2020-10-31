using System;
using System.Reflection;
using Ase.Messaging.Common;
using Ase.Messaging.Messaging;
using Ase.Messaging.Messaging.Annotation;
using Ase.Messaging.Messaging.UnitOfWork;

namespace Ase.Messaging.CommandHandling
{
    public class InterceptorChainParameterResolverFactory : IParameterResolverFactory,
        IParameterResolver<IInterceptorChain>
    {
        private static readonly string INTERCEPTOR_CHAIN_EMITTER_KEY = nameof(IInterceptorChain);

        public static void Initialize<T, R>(IInterceptorChain interceptorChain)
            where T : IMessage<R> where R : class
        {
            Assert.State(CurrentUnitOfWork<T, R>.IsStarted(),
                () => "An active Unit of Work is required for injecting interceptor chain");
            CurrentUnitOfWork<T, R>.Get().Resources().Add(INTERCEPTOR_CHAIN_EMITTER_KEY, interceptorChain);
        }

        public IInterceptorChain ResolveParameterValue<TMessage, TPayload>(IMessage<object> message)
            where TMessage : IMessage<TPayload> where TPayload : class
        {
            return CurrentUnitOfWork<TMessage, TPayload>.Map(
                uow => uow.GetResource<IInterceptorChain>(INTERCEPTOR_CHAIN_EMITTER_KEY)
            ) ?? throw new ArgumentException("InterceptorChain should have been injected");
        }

        public bool Matches<T>(IMessage<T> message)
            where T : class
        {
            return message is ICommandMessage<T>;
        }

        public IParameterResolver<T>? CreateInstance<T>(
            MethodBase executable,
            ParameterInfo[] parameters,
            int parameterIndex
        )
        {
            if (typeof(IInterceptorChain) == parameters[parameterIndex].GetType())
            {
                return (IParameterResolver<T>) this;
            }

            return null;
        }
    }
}