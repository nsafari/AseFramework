namespace Ase.Messaging.Messaging.UnitOfWork
{
    public enum RollbackConfigurationType
    {
        /// <summary>
        /// Configuration that never performs a rollback of the unit of work.
        /// </summary>
        NEVER,
        /// <summary>
        /// Configuration that prescribes a rollback on any sort of exception or error.
        /// </summary>
        ANY_THROWABLE,
        /// <summary>
        /// Configuration that prescribes a rollback on any sort of unchecked exception, including errors.
        /// </summary>
        UNCHECKED_EXCEPTIONS,
        /// <summary>
        /// Configuration that prescribes a rollback on runtime exceptions only.
        /// </summary>
        RUNTIME_EXCEPTIONS
    }
}