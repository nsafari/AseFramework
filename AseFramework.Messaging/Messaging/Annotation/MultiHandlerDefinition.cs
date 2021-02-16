using System;
using System.Collections.Generic;
using System.Threading;
using NHibernate.Engine;

namespace Ase.Messaging.Messaging.Annotation
{
    public class MultiHandlerDefinition: IHandlerDefinition
    {
        private readonly List<IHandlerDefinition> handlerDefinitions;
        private readonly IHandlerEnhancerDefinition handlerEnhancerDefinition;

        public static MultiHandlerDefinition Ordered(params IHandlerDefinition[] delegates) {
            return ordered(Arrays.asList(delegates));
        }

        public static MultiHandlerDefinition Ordered(IHandlerEnhancerDefinition handlerEnhancerDefinition,
            params IHandlerDefinition[] delegates) {
            return ordered(Arrays.asList(delegates), handlerEnhancerDefinition);
        }
        
        public static MultiHandlerDefinition Ordered(List<IHandlerDefinition> delegates) {
            return new MultiHandlerDefinition(delegates);
        }
        
        public static MultiHandlerDefinition Ordered(List<IHandlerDefinition> delegates,
            IHandlerEnhancerDefinition handlerEnhancerDefinition) {
            return new MultiHandlerDefinition(delegates, handlerEnhancerDefinition);
        }

        public MultiHandlerDefinition(params IHandlerDefinition[] delegates) {
            this(Arrays.asList(delegates));
        }

        public MultiHandlerDefinition(List<IHandlerDefinition> delegates) {
            this(delegates,
                ClasspathHandlerEnhancerDefinition.forClassLoader(Thread.currentThread().getContextClassLoader()));
        }
        
        public MultiHandlerDefinition(List<IHandlerDefinition> delegates,
            IHandlerEnhancerDefinition handlerEnhancerDefinition) {
            this.handlerDefinitions = flatten(delegates);
            this.handlerEnhancerDefinition = handlerEnhancerDefinition;
        }

        private static List<IHandlerDefinition> flatten(List<HandlerDefinition> handlerDefinitions) {
            List<HandlerDefinition> flattened = new ArrayList<>(handlerDefinitions.size());
            for (HandlerDefinition handlerDefinition : handlerDefinitions) {
                if (handlerDefinition instanceof MultiHandlerDefinition) {
                    flattened.addAll(((MultiHandlerDefinition) handlerDefinition).getDelegates());
                } else {
                    flattened.add(handlerDefinition);
                }
            }
            flattened.sort(PriorityAnnotationComparator.getInstance());
            return flattened;
        }

        public List<IHandlerDefinition> getDelegates() {
            return Collections.unmodifiableList(handlerDefinitions);
        }
        
        public IHandlerEnhancerDefinition getHandlerEnhancerDefinition() {
            return handlerEnhancerDefinition;
        }
        
        public IMessageHandlingMember<T> CreateHandler<T>(Type declaringType,
            Executable executable,
            ParameterResolverFactory parameterResolverFactory) {
            Optional<MessageHandlingMember<T>> handler = Optional.empty();
            for (HandlerDefinition handlerDefinition : handlerDefinitions) {
                handler = handlerDefinition.createHandler(declaringType, executable, parameterResolverFactory);
                if (handler.isPresent()) {
                    return Optional.of(handlerEnhancerDefinition.wrapHandler(handler.get()));
                }
            }
            return handler;
        }



    }
}