namespace AseFramework.Modeling.Command
{
    /// <summary>
    /// Enumeration containing the possible creation policies for aggregates.
    /// </summary>
    public enum AggregateCreationPolicy
    {
        /// <summary>
        /// Always create a new instance of the aggregate on invoking the method. Fail if already exists.
        /// </summary>
        Always,
        /// <summary>
        /// Create a new instance of the aggregate when it is not found.
        /// </summary>
        CreateIfMissing,
        /// <summary>
        /// Expect instance of the aggregate to exist. Fail if missing.
        /// </summary>
        Never
    }
}