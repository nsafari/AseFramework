using System;
using Ase.Messaging.Messaging.Annotation;

namespace Ase.Messaging.QueryHandling.Annotation
{
    /// <summary>
    /// Interface indicating that a MessageHandlingMember is capable of handling specific query messages.
    /// </summary>
    /// <typeparam name="T">The type of entity to which the message handler will delegate the actual handling of the message</typeparam>
    public interface IQueryHandlingMember<in T> : IMessageHandlingMember<T>
    {
        /// <summary>
        /// Returns the name of the query the handler can handle
        ///
        /// @return the name of the query the handler can handle
        /// </summary>
        string GetQueryName { get; }

        /// <summary>
        /// Returns the return type declared by the handler
        ///
        /// @return the return type declared by the handler
        /// </summary>
        Type GetResultType { get; }
    }
}