using System;
using System.Collections.Generic;
using Ase.Messaging.Common;
using Ase.Messaging.Common.Property;
using Ase.Messaging.EventHandling;
using Ase.Messaging.Messaging.Annotation;
using NHibernate.Mapping.ByCode.Impl;

namespace AseFramework.Modeling.Saga
{
    public class PayloadAssociationResolver : IAssociationResolver
    {
        private Dictionary<string, IProperty<object>> _propertyMap = new Dictionary<string, IProperty<object>>();

        public void Validate<T>(string associationPropertyName, IMessageHandlingMember<T> handler)
        {
            GetProperty(associationPropertyName, handler);
        }

        public object Resolve<T>(
            string associationPropertyName,
            IEventMessage<object> message,
            IMessageHandlingMember<T> handler
        )
        {
            return GetProperty(associationPropertyName, handler).GetValue(message.GetPayload());
        }
        
        private  IProperty<T> GetProperty<T>(string associationPropertyName, IMessageHandlingMember<T> handler) {
            var property = _propertyMap[handler.PayloadType().Name + associationPropertyName];
            if (property == null)
            {
                property = (IProperty<object>) CreateProperty(associationPropertyName, handler);
                _propertyMap.Add(handler.PayloadType().Name + associationPropertyName, property);
            }

            return (IProperty<T>) property;
        }
        
        private  IProperty<T> CreateProperty<T>(String associationPropertyName, IMessageHandlingMember<T> handler) {
            IProperty<object> associationProperty = PropertyAccessStrategy.getProperty(handler.payloadType(),
                associationPropertyName);
            if (associationProperty == null) {
                String handlerName = handler.unwrap(Executable.class).map(Executable::toGenericString).orElse("unknown");
                throw new AxonConfigurationException(format(
                    "SagaEventHandler %s defines a property %s that is not defined on the Event it declares to handle (%s)",
                    handlerName,
                    associationPropertyName,
                    handler.payloadType().getName()
                ));
            }
            return associationProperty;
        }

    }
}