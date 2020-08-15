using System;
using System.Threading.Tasks;
using Ase.Messaging.Common;

namespace Ase.Messaging.Messaging.ResponseTypes
{
    /// <summary>
    /// A {@link ResponseType} implementation that will match with query
    /// handlers which return a single instance of the expected response type. If matching succeeds, the
    /// {@link ResponseType#convert(Object)} function will be called, which will cast the query handler it's
    /// response to {@code R}.
    ///
    /// <typeparam name="R">The response type which will be matched against and converted to</typeparam>
    /// </summary>
    public class InstanceResponseType<R> : AbstractResponseType<R>
        where R : class
    {
        /// <summary>
        /// Instantiate a {@link InstanceResponseType} with the given
        /// {@code expectedResponseType} as the type to be matched against and to which the query response should be
        /// converted to.
        /// </summary>
        /// <param name="expectedResponseType">the response type which is expected to be matched against and returned</param>
        // @JsonCreator
        // @ConstructorProperties({"expectedResponseType"})
        public InstanceResponseType( /*@JsonProperty("expectedResponseType")*/ Type expectedResponseType) : base(
            expectedResponseType)
        {
        }


        /// <summary>
        /// Match the query handler its response {@link java.lang.reflect.Type} with this implementation its
        /// responseType {@code R}.
        /// Will return true if the expected type is assignable to the response type, taking generic types into account.
        /// </summary>
        /// <param name="responseType">the response {@link java.lang.reflect.Type} of the query handler which is
        /// matched against</param>
        /// <returns>true if the response type is assignable to the expected type, taking generic types into
        /// account</returns>
        public override bool Matches(Type responseType)
        {
            Type unwrapped = ReflectionUtils.UnwrapIfType(responseType, typeof(Task));
            return IsGenericAssignableFrom(unwrapped) || IsAssignableFrom(unwrapped);
        }

        public override Type ResponseMessagePayloadType()
        {
            return (Type) System.Convert.ChangeType(ExpectedResponseType, typeof(R));
        }

        public override string ToString()
        {
            return "InstanceResponseType{" + ExpectedResponseType + "}";
        }
    }
}