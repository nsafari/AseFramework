using System;

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

        public Type ResponseMessagePayloadType()
        {
            throw new NotImplementedException();
        }

        public Type GetExpectedResponseType()
        {
            return ExpectedResponseType;
        }
        
        
        protected bool IsGenericAssignableFrom(Type responseType)
        {
            return false;
            // return isTypeVariable(responseType) &&
            // Arrays.stream(((TypeVariable) responseType).getBounds())
            // .anyMatch(this::isAssignableFrom);
        }
        
        protected bool IsAssignableFrom(Type responseType) {
            return ExpectedResponseType.IsAssignableFrom(responseType);
        }


    }
}