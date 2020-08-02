using System;

namespace Ase.Messaging.Messaging.ResponseTypes
{
    /// <summary>
    /// Specifies the expected response type required when performing a query through the
    /// {@link org.axonframework.queryhandling.QueryBus}/{@link org.axonframework.queryhandling.QueryGateway}.
    /// By wrapping the response type as a generic {@code R}, we can easily service the expected response as a single
    /// instance, a list, a page etc., based on the selected implementation even while the query handler return type might be
    /// slightly different.
    /// <p>
    /// It is in charge of matching the response type of a query handler with the given generic {@code R}.
    /// If this match returns true, it signals the found query handler can handle the intended query.
    /// As a follow up, the response retrieved from a query handler should move through the
    /// {@link ResponseType#convert(Object)} function to guarantee the right response type is returned.
    /// <typeparam name="R">the generic type of this {@link ResponseType} to be matched and converted.</typeparam>
    /// <seealso cref="ResponseTypes"/>
    /// </summary>
    public interface IResponseType<R>
    {
        /// <summary>
        /// Match the query handler its response {@link java.lang.reflect.Type} with the {@link ResponseType} implementation
        /// its expected response type {@code R}. Will return true if a response can be converted based on the given
        /// {@code responseType} and false if it cannot.
        /// </summary>
        /// <param name="responseType">the response {@link java.lang.reflect.Type} of the query handler which is matched against</param>
        /// <returns>true if a response can be converted based on the given {@code responseType} and false if it cannot</returns>
        bool Matches(Type responseType);

        /// <summary>
        /// Converts the given {@code response} of type {@link java.lang.Object} into the type {@code R} of this
        /// {@link ResponseType} instance. Should only be called if {@link ResponseType#matches(Type)} returns true.
        /// It is unspecified what this function does if the {@link ResponseType#matches(Type)} returned false.
        /// </summary>
        /// <param name="response">the {@link java.lang.Object} to convert into {@code R}</param>
        /// <returns>a {@code response} of type {@code R}</returns>
        R Convert(object response)
        {
            return (R) response;
        }

        /// <summary>
        /// Returns a {@link java.lang.Class} representing the type of the payload to be contained in the response message.
        /// </summary>
        /// <returns>Returns a {@link java.lang.Class} representing the type of the payload to be contained in the response message.</returns>
        Type ResponseMessagePayloadType();

        /// <summary>
        /// Gets actual response type or generic placeholder.
        /// </summary>
        /// <returns>Gets actual response type or generic placeholder.</returns>
        Type GetExpectedResponseType();

        /// <summary>
        /// Returns the {@code ResponseType} instance that should be used when serializing responses. This method
        /// has a default implementation that returns {@code this}. Implementations that describe a Response Type
        /// that is not suited for serialization, should return an alternative that is suitable, and ensure the
        /// {@link #convert(Object)} is capable of converting that type of response to the request type in this instance.
        /// </summary>
        /// <returns>a {@code ResponseType} instance describing a type suitable for serialization</returns>
        IResponseType<R> ForSerialization() => this;
    }
}