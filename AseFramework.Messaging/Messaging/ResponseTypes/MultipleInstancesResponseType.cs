using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Threading.Tasks;
using Ase.Messaging.Common;

namespace Ase.Messaging.Messaging.ResponseTypes
{

    /// <summary>
    /// A {@link ResponseType} implementation that will match with query
    /// handlers which return a multiple instances of the expected response type. If matching succeeds, the
    /// {@link ResponseType#convert(Object)} function will be called, which will cast the query handler it's response to a
    /// {@link java.util.List} with generic type {@code R}.
    
    /// @param <R> The response type which will be matched against and converted to
    /// </summary>
    public class MultipleInstancesResponseType<R>: AbstractResponseType<List<R>>
    {
        
        // @JsonCreator
            // @ConstructorProperties({"expectedResponseType"})
        public MultipleInstancesResponseType(
            /*@JsonProperty("expectedResponseType")*/ Type expectedCollectionGenericType
            ):base(expectedCollectionGenericType) {
        }


        /// <summary>
        /// Match the query handler its response {@link java.lang.reflect.Type} with this implementation its responseType
        /// {@code R}.
        /// Will return true in the following scenarios:
        /// <ul>
        /// <li>If the response type is an array of the expected type. For example a {@code ExpectedType[]}</li>
        /// <li>If the response type is a {@link java.lang.reflect.GenericArrayType} of the expected type.
        ///     For example a {@code <E extends ExpectedType> E[]}</li>
        /// <li>If the response type is a {@link java.lang.reflect.ParameterizedType} containing a single
        /// {@link java.lang.reflect.TypeVariable} which is assignable to the response type, taking generic types into
        /// account. For example a {@code List<ExpectedType>} or {@code <E extends ExpectedType> List<E>}.</li>
        /// <li>If the response type is a {@link java.lang.reflect.ParameterizedType} containing a single
        /// {@link java.lang.reflect.WildcardType} which is assignable to the response type, taking generic types into
        /// account. For example a {@code <E extends ExpectedType> List<? extends E>}.</li>
        /// </ul>
 
        /// @param responseType the response {@link java.lang.reflect.Type} of the query handler which is matched against
        /// @return true for arrays, generic arrays and {@link java.lang.reflect.ParameterizedType}s (like a
        /// {@link java.lang.Iterable}) for which the contained type is assignable to the expected type
        /// </summary>
        /// <param name="responseType"></param>
        /// <returns></returns>
        public override bool Matches(Type responseType)
        {
            Type unwrapped = ReflectionUtils.UnwrapIfType(responseType, typeof(Task));
            return isIterableOfExpectedType(unwrapped) ||
                   isStreamOfExpectedType(unwrapped) ||
                   isGenericArrayOfExpectedType(unwrapped) ||
                   isArrayOfExpectedType(unwrapped);

        }

        public override Type ResponseMessagePayloadType()
        {
            throw new NotImplementedException();
        }
        
    }
}