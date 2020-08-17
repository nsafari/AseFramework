using System;
using System.Collections.Generic;

namespace Ase.Messaging.Messaging.ResponseTypes
{
    /// <summary>
    /// Utility class containing static methods to obtain instances of {@link ResponseType}.
    /// </summary>
    public static class ResponseTypes
    {
        /// <summary>
        /// Specify the desire to retrieve a single instance of type {@code R} when performing a query.
        /// </summary>
        /// <param name="type">the {@code R} which is expected to be the response type</param>
        /// <typeparam name="R">the generic type of the instantiated {@link ResponseType}</typeparam>
        /// <returns>a {@link ResponseType} specifying the desire to retrieve a single instance of type {@code R}</returns>
        public static IResponseType<R> InstanceOf<R>(Type type)
            where R : class
        {
            return new InstanceResponseType<R>(type);
        }


        /// <summary>
        /// Specify the desire to retrieve a collection of instances of type {@code R} when performing a query.
        /// </summary>
        /// <param name="type">the {@code R} which is expected to be the response type</param>
        /// <typeparam name="R">the generic type of the instantiated {@link ResponseType}</typeparam>
        /// <returns>a {@link ResponseType} specifying the desire to retrieve a collection of instances of type {@code R}</returns>
        public static IResponseType<List<R>> MultipleInstancesOf<R>(Type type)
            where R : class
        {
            return new MultipleInstancesResponseType<R>(type);
        }
    }
}