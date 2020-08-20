using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

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
    public interface IUnitOfWork<out T, out R>
        where T : IMessage<R> where R : class
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
        void Rollback()
        {
            Rollback(null);
        }

        /// <summary>
        /// Initiates the rollback of this Unit of Work, invoking all registered rollback ({@link #onRollback(Consumer) and
        /// clean-up handlers {@link #onCleanup(Consumer)}} respectively. Finally, the Unit of Work is unregistered from the
        /// {@link CurrentUnitOfWork}.
        /// </summary>
        /// <param name="cause">The cause of the rollback. May be {@code null}.</param>
        void Rollback(Exception? cause);

        /// <summary>
        /// Indicates whether this UnitOfWork is started. It is started when the {@link #start()} method has been called, and
        /// if the UnitOfWork has not been committed or rolled back.
        /// </summary>
        /// <returns>{@code true} if this UnitOfWork is started, {@code false} otherwise.</returns>
        bool IsActive()
        {
            return PhaseUtil.GetPhase(GetPhase()).IsStarted();
        }

        /// <summary>
        /// Returns the current phase of the Unit of Work.
        /// </summary>
        /// <returns>the Unit of Work phase</returns>
        Phase GetPhase();

        /// <summary>
        /// Register given {@code handler} with the Unit of Work. The handler will be notified when the phase of the
        /// Unit of Work changes to {@link Phase#PREPARE_COMMIT}.
        /// </summary>
        /// <param name="handler">the handler to register with the Unit of Work</param>
        void OnPrepareCommit(Action<IUnitOfWork<T, R>> handler);

        /// <summary>
        /// Register given {@code handler} with the Unit of Work. The handler will be notified when the phase of the
        /// Unit of Work changes to {@link Phase#COMMIT}.
        /// </summary>
        /// <param name="handler">the handler to register with the Unit of Work</param>
        void OnCommit(Action<IUnitOfWork<T, R>> handler);

        /// <summary>
        /// Register given {@code handler} with the Unit of Work. The handler will be notified when the phase of the
        /// Unit of Work changes to {@link Phase#AFTER_COMMIT}.
        /// </summary>
        /// <param name="handler">the handler to register with the Unit of Work</param>
        void AfterCommit(Action<IUnitOfWork<T, R>> handler);

        /// <summary>
        /// Register given {@code handler} with the Unit of Work. The handler will be notified when the phase of the
        /// Unit of Work changes to {@link Phase#ROLLBACK}. On rollback, the cause for the rollback can obtained from the
        /// supplied
        /// </summary>
        /// <param name="handler">the handler to register with the Unit of Work</param>
        void OnRollback(Action<IUnitOfWork<T, R>> handler);

        /// <summary>
        /// Register given {@code handler} with the Unit of Work. The handler will be notified when the phase of the
        /// Unit of Work changes to {@link Phase#CLEANUP}.
        /// </summary>
        /// <param name="handler">the handler to register with the Unit of Work</param>
        void OnCleanup(Action<IUnitOfWork<T, R>> handler);

        IUnitOfWork<PT, PR>? Parent<PT, PR>()
            where PT : IMessage<PR> where PR : class;
    }

    public enum Phase
    {
        /// <summary>
        /// Indicates that the unit of work has been created but has not been registered with the {@link
        /// CurrentUnitOfWork} yet.
        /// </summary>
        NotStarted,

        /// <summary>
        /// Indicates that the Unit of Work has been registered with the {@link CurrentUnitOfWork} but has not been
        /// committed, because its Message has not been processed yet.
        /// </summary>
        Started,

        /// <summary>
        /// Indicates that the Unit of Work is preparing its commit. This means that {@link #commit()} has been invoked
        /// on the Unit of Work, indicating that the Message {@link #getMessage()} of the Unit of Work has been
        /// processed.
        /// <p/>
        /// All handlers registered to be notified before commit {@link #onPrepareCommit} will be invoked. If no
        /// exception is raised by any of the handlers the Unit of Work will go into the {@link #COMMIT} phase, otherwise
        /// it will be rolled back.
        /// </summary>
        PrepareCommit,

        /// <summary>
        /// Indicates that the Unit of Work has been committed and is passed the {@link #PREPARE_COMMIT} phase.
        /// </summary>
        Commit,

        /// <summary>
        /// Indicates that the Unit of Work is being rolled back. Generally this is because an exception was raised while
        /// processing the {@link #getMessage() message} or while the Unit of Work was being committed.
        /// </summary>
        Rollback,

        /// <summary>
        /// Indicates that the Unit of Work is after a successful commit. In this phase the Unit of Work cannot be rolled
        /// back anymore.
        /// </summary>
        AfterCommit,

        /// <summary>
        /// Indicates that the Unit of Work is after a successful commit or after a rollback. Any resources tied to this
        /// Unit of Work should be released.
        /// </summary>
        Cleanup,

        /// <summary>
        /// Indicates that the Unit of Work is at the end of its life cycle. This phase is final.
        /// </summary>
        Closed
    }

    /// <summary>
    /// Enum indicating possible phases of the Unit of Work.
    /// </summary>
    sealed class PhaseUtil
    {
        private static readonly IDictionary<Phase, PhaseUtil> _phaseUtils =
            new ConcurrentDictionary<Phase, PhaseUtil>();

        static PhaseUtil()
        {
            _phaseUtils.Add(Phase.NotStarted, new PhaseUtil(false, false, 1000));
            _phaseUtils.Add(Phase.Started, new PhaseUtil(true, false, 2000));
            _phaseUtils.Add(Phase.PrepareCommit, new PhaseUtil(true, false, 3000));
            _phaseUtils.Add(Phase.Commit, new PhaseUtil(true, true, 4000));
            _phaseUtils.Add(Phase.Rollback, new PhaseUtil(true, true, 5000));
            _phaseUtils.Add(Phase.AfterCommit, new PhaseUtil(true, true, 6000));
            _phaseUtils.Add(Phase.Cleanup, new PhaseUtil(false, true, 7000));
            _phaseUtils.Add(Phase.Closed, new PhaseUtil(false, true, 8000));
        }

        private readonly bool _started;
        private readonly bool _reverseCallbackOrder;
        private readonly int _ordinal;

        private PhaseUtil(bool started, bool reverseCallbackOrder, int ordinal)
        {
            _started = started;
            _reverseCallbackOrder = reverseCallbackOrder;
            _ordinal = ordinal;
        }

        /// <summary>
        /// Check if a Unit of Work in this phase has been started, i.e. is registered with the {@link
        /// CurrentUnitOfWork}.
        /// </summary>
        /// <returns>{@code true} if the Unit of Work is started when in this phase, {@code false} otherwise</returns>
        public bool IsStarted()
        {
            return _started;
        }

        /// <summary>
        /// Check whether registered handlers for this phase should be invoked in the order of registration (first
        /// registered handler is invoked first) or in the reverse order of registration (last registered handler is
        /// invoked first).
        /// </summary>
        /// <returns>{@code true} if the order of invoking handlers in this phase should be in the reverse order of
        /// registration, {@code false} otherwise.</returns>
        public bool IsReverseCallbackOrder()
        {
            return _reverseCallbackOrder;
        }

        /// <summary>
        /// Check if this Phase comes before given other {@code phase}.
        /// </summary>
        /// <param name="phase">The other Phase</param>
        /// <returns>{@code true} if this comes before the given {@code phase}, {@code false} otherwise.</returns>
        public bool IsBefore(Phase phase)
        {
            return _ordinal < GetPhase(phase)._ordinal;
        }

        /// <summary>
        /// Check if this Phase comes after given other {@code phase}.
        /// </summary>
        /// <param name="phase">The other Phase</param>
        /// <returns>{@code true} if this comes after the given {@code phase}, {@code false} otherwise.</returns>
        public bool IsAfter(Phase phase)
        {
            return _ordinal > GetPhase(phase)._ordinal;
        }

        public static PhaseUtil GetPhase(Phase phase)
        {
            return _phaseUtils[phase];
        }
    }
}