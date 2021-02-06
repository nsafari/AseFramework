using System;

namespace AseFramework.Modeling.Command
{
    /// <summary>
    /// Annotation used to specify the creation policy for a command handler. Default behavior is that command handlers
    /// defined on a constructor would create a new instance of the aggregate, and command handlers defined on other methods
    /// expect an existing aggregate. This annotation provides the option to define policy
    /// {@code AggregateCreationPolicy.CREATE_IF_MISSING} or {@code AggregateCreationPolicy.ALWAYS} on a command handler to
    /// create a new instance of the aggregate from a handler operation.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class CreationPolicyAttribute: Attribute
    {
        /// <summary>
        /// Specifies the {@link AggregateCreationPolicy} to apply. {@code NEVER} when not set.
        /// @return the creation policy
        /// </summary>
        private AggregateCreationPolicy Value { get; set; } = AggregateCreationPolicy.Never;

    }
}