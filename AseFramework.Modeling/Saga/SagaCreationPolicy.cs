namespace AseFramework.Modeling.Saga
{
    /// <summary>
    /// Enumeration containing the possible Creation Policies for Sagas.
    /// </summary>
    public enum SagaCreationPolicy
    {
        /// <summary>
        /// Never create a new Saga instance, even if none exists.
        /// </summary>
        None,
        /// <summary>
        /// Only create a new Saga instance if none can be found.
        /// </summary>
        IfNoneFound,
        /// <summary>
        /// Always create a new Saga, even if one already exists.
        /// </summary>
        Always
    }
}