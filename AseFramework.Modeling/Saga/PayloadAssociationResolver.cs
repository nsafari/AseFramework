using System;
using System.Collections.Generic;
using System.Reflection;
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

        private IProperty<T> GetProperty<T>(string associationPropertyName, IMessageHandlingMember<T> handler)
        {
            var property = _propertyMap[handler.PayloadType().Name + associationPropertyName];
            if (property == null)
            {
                property = (IProperty<object>) CreateProperty(associationPropertyName, handler);
                _propertyMap.Add(handler.PayloadType().Name + associationPropertyName, property);
            }

            return (IProperty<T>) property;
        }

        private IProperty<T> CreateProperty<T>(String associationPropertyName, IMessageHandlingMember<T> handler)
        {
            IProperty<T> associationProperty = PropertyAccessStrategy.getProperty(
                handler.PayloadType(),
                associationPropertyName
            );
            if (associationProperty != null) return associationProperty;
            string handlerName = handler.Unwrap<MemberInfo>()?.ToString() ?? "unknown";
            throw new AxonConfigurationException(
                $"SagaEventHandler {handlerName} defines a property {associationPropertyName} " +
                $"that is not defined on the Event it declares to handle ({handler.PayloadType().Name})");

        }
    }
}