using System;
using System.Linq;
using Ase.Messaging.Messaging;
using Ase.Messaging.Messaging.Annotation;

namespace Ase.Messaging.EventHandling
{
    public class AnnotationEventHandlerAdapter<TMessage, TPayload> : IEventMessageHandler<TMessage, TPayload>
        where TMessage : IMessage<IEventMessage<TPayload>> where TPayload : class
    {
        private readonly AnnotatedHandlerInspector<object> _inspector;
        private readonly Type _listenerType;
        private readonly object _annotatedEventListener;

        public AnnotationEventHandlerAdapter(object annotatedEventListener)
            : this(annotatedEventListener,
                ClasspathParameterResolverFactory.ForClass(annotatedEventListener.GetType()))
        {
        }

        public AnnotationEventHandlerAdapter(
            object annotatedEventListener,
            IParameterResolverFactory parameterResolverFactory)
            : this(annotatedEventListener,
                parameterResolverFactory,
                ClasspathHandlerDefinition.ForClass(annotatedEventListener.GetType()))
        {
        }

        public AnnotationEventHandlerAdapter(
            object annotatedEventListener,
            IParameterResolverFactory parameterResolverFactory,
            IHandlerDefinition handlerDefinition
        )
        {
            _annotatedEventListener = annotatedEventListener;
            _listenerType = annotatedEventListener.GetType();
            _inspector = AnnotatedHandlerInspector<object>.InspectType(annotatedEventListener.GetType(),
                parameterResolverFactory,
                handlerDefinition);
        }

        public object? Handle(IEventMessage<TPayload> @event)
        {
            foreach (IMessageHandlingMember<object> handler in _inspector.GetHandlers())
            {
                if (handler.CanHandle(@event))
                {
                    return handler.Handle(@event, _annotatedEventListener);
                }
            }

            return null;
        }

        public bool CanHandle(IEventMessage<object> @event)
        {
            foreach (IMessageHandlingMember<object> handler in _inspector.GetHandlers())
            {
                if (handler.CanHandle(@event))
                {
                    return true;
                }
            }

            return false;
        }

        public bool CanHandleType(Type payloadType)
        {
            return _inspector.GetHandlers().Any(handler => handler.CanHandleType(payloadType));
        }

        object? IMessageHandler<TMessage, IEventMessage<TPayload>>.Handle(TMessage message)
        {
            var eventMessage = message as IEventMessage<TPayload>;
            return eventMessage == null ? null : Handle(eventMessage);
        }

        public Type GetTargetType()
        {
            return _listenerType;
        }

        public void PrepareReset()
        {
            try
            {
                Handle(GenericEventMessage<TPayload>.AsEventMessage<TPayload>(new ResetTriggeredEvent()));
            }
            catch (Exception e)
            {
                throw new ResetNotSupportedException("An Error occurred while notifying handlers of the reset", e);
            }
        }

        public bool SupportsReset()
        {
            return true;
        }
    }
}