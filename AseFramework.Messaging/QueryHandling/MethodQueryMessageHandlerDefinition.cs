using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Ase.Messaging.Messaging;
using Ase.Messaging.Messaging.Annotation;
using Ase.Messaging.QueryHandling.Annotation;

namespace Ase.Messaging.QueryHandling
{
    public class MethodQueryMessageHandlerDefinition : IHandlerEnhancerDefinition
    {
        public IMessageHandlingMember<T> WrapHandler<T>(IMessageHandlingMember<T> original) where T : class
        {
            var annotationAttributes = original.AnnotationAttributes<QueryHandlerAttribute>();
            return annotationAttributes != null
                ? new MethodQueryMessageHandlingMember<T>(original, annotationAttributes)
                : original;
        }

        internal class MethodQueryMessageHandlingMember<T> : WrappedMessageHandlingMember<T>, IQueryHandlingMember<T>
        {
            private readonly string _queryName;
            private readonly Type _resultType;

            public MethodQueryMessageHandlingMember(
                IMessageHandlingMember<T> original,
                IDictionary<string, object?> attr
            ) : base(original)
            {
                string? queryNameAttribute = (string) attr["queryName"]!;
                if ("".Equals(queryNameAttribute))
                {
                    queryNameAttribute = original.PayloadType().Name;
                }

                _queryName = queryNameAttribute;

                _resultType = original.Unwrap<MethodInfo>()?.ReturnType
                             ?? throw new UnsupportedHandlerException(
                                 "@QueryHandler annotation can only be put on methods.",
                                 original.Unwrap<MemberInfo>() ?? null
                             );

                if (typeof(void) == _resultType)
                {
                    throw new UnsupportedHandlerException(
                        "@QueryHandler annotated methods must not declare void return type",
                        original.Unwrap<MemberInfo>() ?? null
                    );
                }
            }

            public override object? Handle(IMessage<object> message, T target)
            {
                object? result = base.Handle(message, target);
                return result;
            }

            private Type QueryResultType(MethodInfo method)
            {
                if (typeof(void) == method.ReturnType)
                {
                    throw new UnsupportedHandlerException(
                        "@QueryHandler annotated methods must not declare void return type", method
                    );
                }

                return UnwrapType(method.ReturnType);
            }

            private Type UnwrapType(Type genericReturnType)
            {
                if (genericReturnType.IsGenericType)
                {
                    var taskType = genericReturnType.GenericTypeArguments.FirstOrDefault(
                        returnType => returnType == typeof(Task)
                    );
                    if (taskType is { }) return taskType.GenericTypeArguments[0];
                }

                return genericReturnType;
            }
            
            public new bool CanHandle(IMessage<object> message) {
                return base.CanHandle(message)
                       && message is IQueryMessage<object, object> queryMessage && _queryName.Equals(queryMessage.GetQueryName())
                    && queryMessage.GetResponseType().Matches(_resultType);
            }

            public string GetQueryName => _queryName;

            public Type GetResultType => _resultType;
            

        }
    }
}