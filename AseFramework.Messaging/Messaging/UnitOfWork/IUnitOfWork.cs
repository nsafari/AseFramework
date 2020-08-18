using System;

namespace Ase.Messaging.Messaging.UnitOfWork
{
    /// <summary>
    /// This class represents a Unit of Work that monitors the processing of a {@link Message}.
    /// <p/>
    /// Before processing begins a Unit of Work is bound to the active thread by registering it with the {@link
    ///     CurrentUnitOfWork}. After processing, the Unit of Work is unregistered from the {@link CurrentUnitOfWork}.
    /// <p/>
    /// Handlers can be notified about the state of the processing of the Message by registering with this Unit of Work.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IUnitOfWork<T>
        where T : IMessage<object>
    {
        /// <summary>
        /// Starts the current unit of work. The UnitOfWork instance is registered with the CurrentUnitOfWork.
        /// </summary>
        void Start();

        /// <summary>
        /// Commits the Unit of Work. This should be invoked after the Unit of Work Message has been processed. Handlers
        /// registered to the Unit of Work will be notified.
        /// <p/>
        /// After the commit (successful or not), any registered clean-up handlers ({@link #onCleanup(Consumer)}}) will be
        /// invoked and the Unit of Work is unregistered from the {@link CurrentUnitOfWork}.
        /// <p/>
        /// If the Unit of Work fails to commit, e.g. because an exception is raised by one of its handlers, the Unit of Work
        /// is rolled back.
        /// @throws IllegalStateException if the UnitOfWork wasn't started or if the Unit of Work is not the 'current' Unit
        /// of Work returned by {@link CurrentUnitOfWork#get()}.
        /// </summary>
        void Commit();
        
        /// <summary>
        /// Initiates the rollback of this Unit of Work, invoking all registered rollback ({@link #onRollback(Consumer) and
        /// clean-up handlers {@link #onCleanup(Consumer)}} respectively. Finally, the Unit of Work is unregistered from the
        /// {@link CurrentUnitOfWork}.
        /// <p/>
        /// If the rollback is a result of an exception, consider using {@link #rollback(Throwable)} instead.
        /// @throws IllegalStateException if the Unit of Work is not in a compatible phase.
        /// </summary>
        void Rollback() {
            Rollback(null);
        }

        /// <summary>
        /// Initiates the rollback of this Unit of Work, invoking all registered rollback ({@link #onRollback(Consumer) and
        /// clean-up handlers {@link #onCleanup(Consumer)}} respectively. Finally, the Unit of Work is unregistered from the
        /// {@link CurrentUnitOfWork}.
        /// </summary>
        /// <param name="cause">The cause of the rollback. May be {@code null}.</param>
        void Rollback(Exception? cause);

    }
}