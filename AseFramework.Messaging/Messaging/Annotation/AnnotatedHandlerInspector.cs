using System;
using System.Collections.Generic;
using System.Reflection;
using NHibernate.Mapping;

namespace Ase.Messaging.Messaging.Annotation
{
    public class AnnotatedHandlerInspector<T>
    {
        private readonly Type inspectedType;
        private readonly IParameterResolverFactory parameterResolverFactory;
        private readonly Dictionary<Type, AnnotatedHandlerInspector<T>> registry;
        private readonly List<AnnotatedHandlerInspector<T>> superClassInspectors;
        private readonly List<IMessageHandlingMember<T>> handlers;
        private readonly IHandlerDefinition handlerDefinition;

        private AnnotatedHandlerInspector(Type inspectedType,
            List<AnnotatedHandlerInspector<T>> superClassInspectors,
            IParameterResolverFactory parameterResolverFactory,
            IHandlerDefinition handlerDefinition,
            Dictionary<Type, AnnotatedHandlerInspector<T>> registry)
        {
            this.inspectedType = inspectedType;
            this.parameterResolverFactory = parameterResolverFactory;
            this.registry = registry;
            this.superClassInspectors = new List<AnnotatedHandlerInspector<T>>(superClassInspectors);
            this.handlers = new List<IMessageHandlingMember<T>>();
            this.handlerDefinition = handlerDefinition;
        }

        public static AnnotatedHandlerInspector<T> InspectType(Type handlerType)
        {
            return InspectType(handlerType, ClasspathParameterResolverFactory.forClass(handlerType));
        }

        public static AnnotatedHandlerInspector<T> InspectType(
            Type handlerType,
            IParameterResolverFactory parameterResolverFactory
        )
        {
            return InspectType(handlerType,
                parameterResolverFactory,
                ClasspathHandlerDefinition.forClass(handlerType));
        }

        public static AnnotatedHandlerInspector<T> InspectType(
            Type handlerType,
            IParameterResolverFactory parameterResolverFactory,
            IHandlerDefinition handlerDefinition
        )
        {
            return CreateInspector(
                handlerType,
                parameterResolverFactory,
                handlerDefinition,
                new Dictionary<Type, AnnotatedHandlerInspector<T>>()
            );
        }

        private static AnnotatedHandlerInspector<C> CreateInspector<C>(
            IParameterResolverFactory parameterResolverFactory,
            IHandlerDefinition handlerDefinition,
            Dictionary<Type, AnnotatedHandlerInspector<T>> registry)
        {
            return CreateInspector<C>(
                typeof(C),
                parameterResolverFactory,
                handlerDefinition,
                registry
            );
        }

        private static AnnotatedHandlerInspector<C> CreateInspector<C>(
            Type inspectedType,
            IParameterResolverFactory parameterResolverFactory,
            IHandlerDefinition handlerDefinition,
            Dictionary<Type, AnnotatedHandlerInspector<T>> registry)
        {
            if (!registry.ContainsKey(inspectedType))
            {
                registry.Add(inspectedType,
                    AnnotatedHandlerInspector.Initialize<C>(
                        parameterResolverFactory,
                        handlerDefinition,
                        registry)
                );
            }

            //noinspection unchecked
            return registry[inspectedType];
        }

        private static AnnotatedHandlerInspector<C> Initialize<C>(
            IParameterResolverFactory parameterResolverFactory,
            IHandlerDefinition handlerDefinition,
            Dictionary<Type, AnnotatedHandlerInspector<C>> registry)
        {
            var inspectedType = typeof(C);
            List<AnnotatedHandlerInspector<T>> parents = new List<AnnotatedHandlerInspector<T>>();
            foreach (Type iFace in inspectedType.GetInterfaces())
            {
                //noinspection unchecked
                parents.Add(
                    CreateInspector(
                        iFace,
                        parameterResolverFactory,
                        handlerDefinition,
                        registry
                    )
                );
            }

            if (inspectedType.BaseType != null && typeof(object) != inspectedType.BaseType)
            {
                parents.Add(
                    CreateInspector(inspectedType.BaseType,
                        parameterResolverFactory,
                        handlerDefinition,
                        registry)
                );
            }

            AnnotatedHandlerInspector<T> inspector = new AnnotatedHandlerInspector<T>(
                inspectedType,
                parents,
                parameterResolverFactory,
                handlerDefinition,
                registry
            );
            inspector.InitializeMessageHandlers(parameterResolverFactory, handlerDefinition);
            return inspector;
        }

        private void InitializeMessageHandlers(
            IParameterResolverFactory parameterResolverFactory,
            IHandlerDefinition handlerDefinition
        )
        {
            foreach (MethodInfo method in inspectedType.GetMethods())
            {
                var messageHandlingMember =
                    handlerDefinition.CreateHandler<T>(inspectedType, method, parameterResolverFactory);
                this.RegisterHandler(messageHandlingMember);
            }

            foreach (ConstructorInfo constructor in inspectedType.GetConstructors())
            {
                this.RegisterHandler(
                    handlerDefinition.CreateHandler(inspectedType, constructor, parameterResolverFactory)
                );
            }

            superClassInspectors.ForEach(sci => handlers.AddRange(sci.handlers));
            handlers.Sort(HandlerComparator.instance());
        }

        private void RegisterHandler(IMessageHandlingMember<T> handler)
        {
            handlers.Add(handler);
        }

        public AnnotatedHandlerInspector<C> Inspect<C>()
        {
            return AnnotatedHandlerInspector<C>.CreateInspector<C>(
                parameterResolverFactory,
                handlerDefinition,
                registry
            );
        }
        
        public List<IMessageHandlingMember<T>> GetHandlers() {
            return handlers;
        }
    }
}