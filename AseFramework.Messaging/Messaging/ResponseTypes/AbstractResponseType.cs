using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using Ase.Messaging.Common;

namespace Ase.Messaging.Messaging.ResponseTypes
{
    /// <summary>
    /// 
    /// Abstract implementation of the {@link ResponseType} which contains
    /// match functions for the majority of the {@link java.lang.reflect.Type} options available.
    /// For single instance response types, a direct assignable to check will be performed. For multiple instances
    /// response types, the match will be performed against the containing type of that array/collection/etc.
    /// Proves useful for reuse among ResponseType implementations.
    /// 
    /// </summary>
    /// <typeparam name="R"></typeparam>
    public abstract class AbstractResponseType<R>: IResponseType<R>
    {
        protected readonly Type ExpectedResponseType;

        /// <summary>
        /// Instantiate a {@link ResponseType} with the given
        /// {@code expectedResponseType} as the type to be matched against and to which the query response should be
        /// converted to, as is or as the contained type for an array/list/etc.
        /// </summary>
        /// <param name="expectedResponseType"></param>
        protected AbstractResponseType(Type expectedResponseType)
        {
            ExpectedResponseType = expectedResponseType;
        }

        public abstract bool Matches(Type responseType);

        public abstract Type ResponseMessagePayloadType();
        
        public Type GetExpectedResponseType()
        {
            return ExpectedResponseType;
        }
        
        protected bool IsGenericAssignableFrom(Type responseType) {
            return responseType.IsGenericType &&
                   responseType.GetGenericParameterConstraints().Any(IsAssignableFrom);
        }
        
        protected bool IsAssignableFrom(Type responseType) {
            return ExpectedResponseType.IsAssignableFrom(responseType);
        }

        protected bool IsIterableOfExpectedType(Type responseType) {
            Type? iterableType = ReflectionUtils.GetExactSuperType(responseType, typeof(IEnumerator));
            return iterableType != null && IsParameterizedTypeOfExpectedType(iterableType);
        }
        
        protected bool IsParameterizedTypeOfExpectedType(Type responseType) {
            bool isGenericType = responseType.IsGenericType;
            if (!isGenericType) {
                return false;
            }

            Type[] actualTypeArguments = responseType.GetGenericArguments();
            bool hasOneTypeArgument = actualTypeArguments.Length == 1;
            if (!hasOneTypeArgument) {
                return false;
            }

            Type actualTypeArgument = actualTypeArguments[0];
            return IsAssignableFrom(actualTypeArgument) ||
                   IsGenericAssignableFrom(actualTypeArgument); //||
            //isWildcardTypeWithMatchingUpperBound(actualTypeArgument);
        }
        


    }
}