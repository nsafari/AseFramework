using System;

namespace Ase.Messaging.Messaging.UnitOfWork
{
    /// <summary>
    /// The RollbackConfiguration defines if a Unit of Work should be rolled back when an exception is raised during the
    /// processing of a Message.
    /// <p/>
    /// Note that the Unit of Work will always throw any exceptions raised during processing, regardless of whether or not
    /// the Unit of Work is rolled back.
    /// </summary>
    public interface IRollbackConfiguration
    {
        /// <summary>
        /// Decides whether the given {@code throwable} should trigger a rollback.
        /// </summary>
        /// <param name="throwable">the Throwable to evaluate</param>
        /// <returns>{@code true} if the UnitOfWork should be rolled back, {@code false} otherwise</returns>
        bool rollBackOn(Exception throwable);
    }
}