using System;
using System.Collections.Generic;
using Ase.Messaging.EventHandling;
using Ase.Messaging.Messaging.Annotation;

namespace AseFramework.Modeling.Command.Inspection
{
    /// <summary>
    /// Interface of an entity model that describes the properties and capabilities of an entity of type {@code T}. The
    /// entity may be child entity or an aggregate root.
    /// </summary>
    /// <typeparam name="T">The type of entity described by this model</typeparam>
    public interface IEntityModel<in T>
    {
        /// <summary>
        /// Get the identifier of the given {@code target} entity.
        /// </summary>
        /// <param name="target">The entity instance</param>
        /// <returns>The identifier of the given target entity</returns>
        object GetIdentifier(T target);

        /// <summary>
        /// Get the name of the routing key property on commands and events that provides the identifier that should be used
        /// to target entities of this kind.
        /// </summary>
        /// <returns>The name of the routing key that holds the identifier used to target this sort of entity</returns>
        string RoutingKey();

        /// <summary>
        /// Publish given event {@code message} on the given {@code target} entity.
        /// </summary>
        /// <param name="message">The event message to publish</param>
        /// <param name="target">The target entity for the event</param>
        void Publish(IEventMessage<object> message, T target);

        /// <summary>
        /// Get a mapping of {@link MessageHandlingMember} to command name (obtained via {@link
        /// org.axonframework.commandhandling.CommandMessage#getCommandName()}).
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <returns>Map of message handler to command name</returns>
        List<IMessageHandlingMember<R>> CommandHandlers<R>()
            where R : T;

        /// <summary>
        /// Gets a list of command handler interceptors for this entity.
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <returns>list of command handler interceptors</returns>
        List<IMessageHandlingMember<R>> CommandHandlerInterceptors<R>()
            where R : T;

        /// <summary>
        /// Get the EntityModel of an entity of type {@code childEntityType} in case it is the child of the modeled entity.
        /// </summary>
        /// <param name="childEntityType">The class instance of the child entity type</param>
        /// <typeparam name="C">the type of the child entity</typeparam>
        /// <returns>An EntityModel for the child entity</returns>
        IEntityModel<C> ModelOf<C>(Type childEntityType);

        /// <summary>
        /// Returns the class this model describes
        /// </summary>
        /// <returns>the class this model describes</returns>
        Type EntityClass();
    }
}