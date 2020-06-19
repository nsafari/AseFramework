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
        protected readonly Type expectedResponseType;

        protected AbstractResponseType(Type expectedResponseType)
        {
            
        }

    }
}