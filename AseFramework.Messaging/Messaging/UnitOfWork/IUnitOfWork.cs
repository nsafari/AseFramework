using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using Ase.Messaging.Common.transaction;
using Ase.Messaging.Messaging.correlation;

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
    public interface IUnitOfWork<out T, R>
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

        /// <summary>
        /// Returns an optional for the parent of this Unit of Work. The optional holds the Unit of Work that was active when
        /// this Unit of Work was started. In case no other Unit of Work was active when this Unit of Work was started the
        /// optional is empty, indicating that this is the Unit of Work root.
        /// </summary>
        /// <typeparam name="PT"></typeparam>
        /// <typeparam name="PR"></typeparam>
        /// <returns>an optional parent Unit of Work</returns>
        IUnitOfWork<PT, PR>? Parent<PT, PR>()
            where PT : IMessage<PR> where PR : class;

        /// <summary>
        /// Check that returns {@code true} if this Unit of Work has not got a parent.
        /// </summary>
        /// <returns>{@code true} if this Unit of Work has no parent</returns>
        public bool IsRoot()
        {
            return Parent<IMessage<object>, object>() == null;
        }


        /// <summary>
        /// Returns the root of this Unit of Work. If this Unit of Work has no parent (see {@link #parent()}) it returns
        /// itself, otherwise it returns the root of its parent.
        /// </summary>
        /// <typeparam name="PT"></typeparam>
        /// <typeparam name="PR"></typeparam>
        /// <returns>the root of this Unit of Work</returns>
        public IUnitOfWork<PT, PR> Root<PT, PR>()
            where PT : IMessage<PR> where PR : class
        {
            //noinspection unchecked // cast is used to remove inspection error in IDE
            var unitOfWork = Parent<PT, PR>();
            return unitOfWork != null ? unitOfWork.Root<PT, PR>() : (IUnitOfWork<PT, PR>) this;
        }

        /// <summary>
        /// Get the message that is being processed by the Unit of Work. A Unit of Work processes a single Message over
        /// its life cycle.
        /// </summary>
        /// <returns>the Message being processed by this Unit of Work</returns>
        T GetMessage();

        /// <summary>
        /// Transform the Message being processed using the given operator and stores the result.
        /// <p>
        /// Implementations should take caution not to change the message type to a type incompatible with the current Unit
        /// of Work. For example, do not return a CommandMessage when transforming an EventMessage.
        /// </summary>
        /// <param name="transformOperator">The transform operator to apply to the stored message</param>
        /// <typeparam name="TR"></typeparam>
        /// <returns>this Unit of Work</returns>
        IUnitOfWork<T, R> TransformMessage<TR>(Func<T, TR> transformOperator)
            where TR : IMessage<object>;

        /// <summary>
        /// Get the correlation data contained in the {@link #getMessage() message} being processed by the Unit of Work.
        /// <p/>
        /// By default this correlation data will be copied to other {@link Message messages} created in the context of this
        /// Unit of Work, so long as these messages extend from {@link org.axonframework.messaging.GenericMessage}.
        /// </summary>
        /// <returns>The correlation data contained in the message processed by this Unit of Work</returns>
        MetaData GetCorrelationData();

        /// <summary>
        /// Register given {@code correlationDataProvider} with this Unit of Work. Correlation data providers are used
        /// to provide meta data based on this Unit of Work's {@link #getMessage() Message} when {@link
        /// #getCorrelationData()} is invoked.
        /// </summary>
        /// <param name="correlationDataProvider">the Correlation Data Provider to register</param>
        /// <typeparam name="R"></typeparam>
        void RegisterCorrelationDataProvider(ICorrelationDataProvider.CorrelationDataFor<R> correlationDataProvider);

        /// <summary>
        /// Returns a mutable map of resources registered with the Unit of Work.
        /// </summary>
        /// <returns>mapping of resources registered with this Unit of Work</returns>
        IImmutableDictionary<string, object> Resources();

        /// <summary>
        /// Returns the resource attached under given {@code name}, or {@code null} if no such resource is
        /// available.
        /// </summary>
        /// <param name="name">The name under which the resource was attached</param>
        /// <typeparam name="TR">The type of resource</typeparam>
        /// <returns>The resource mapped to the given {@code name}, or {@code null} if no resource was found.</returns>
        TR GetResource<TR>(string name)
        {
            return (TR) Resources()[name];
        }

        /// <summary>
        /// Returns the resource attached under given {@code name}. If there is no resource mapped to the given key yet
        /// the {@code mappingFunction} is invoked to provide the mapping.
        /// </summary>
        /// <param name="key">The name under which the resource was attached</param>
        /// <param name="mappingFunction">The function that provides the mapping if there is no mapped resource yet</param>
        /// <typeparam name="R">The type of resource</typeparam>
        /// <returns>The resource mapped to the given {@code key}, or the resource returned by the
        /// {@code mappingFunction} if no resource was found.</returns>
        TR GetOrComputeResource<TR>(string key, Func<string, TR> mappingFunction)
        {
            if (!Resources().TryGetValue(key, out var val))
            {
                val = mappingFunction(key)!;
                Resources().Add(key, val);
            }

            return (TR) val;
        }

        /// <summary>
        /// Returns the resource attached under given {@code name}. If there is no resource mapped to the given key,
        /// the {@code defaultValue} is returned.
        /// </summary>
        /// <param name="key">The name under which the resource was attached</param>
        /// <param name="defaultValue">The value to return if no mapping is available</param>
        /// <typeparam name="TR">The type of resource</typeparam>
        /// <returns>The resource mapped to the given {@code key}, or the resource returned by the
        /// {@code mappingFunction} if no resource was found.</returns>
        TR GetOrDefaultResource<TR>(string key, TR defaultValue)
        {
            return (TR) Resources().GetValueOrDefault(key, defaultValue!);
        }

        /// <summary>
        /// Attach a transaction to this Unit of Work, using the given {@code transactionManager}. The transaction will be
        /// managed in the lifecycle of this Unit of Work. Failure to start a transaction will cause this Unit of Work
        /// to be rolled back.
        /// </summary>
        /// <param name="transactionManager">The Transaction Manager to create, commit and/or rollback the transaction</param>
        /// <exception cref="Exception"></exception>
        void AttachTransaction(ITransactionManager transactionManager)
        {
            try
            {
                ITransaction transaction = transactionManager.StartTransaction();
                OnCommit(u => transaction.Commit());
                OnRollback(u => transaction.Rollback());
            }
            catch (Exception t)
            {
                Rollback(t);
                throw t;
            }
        }

        /// <summary>
        /// Execute the given {@code task} in the context of this Unit of Work. If the Unit of Work is not started yet
        /// it will be started.
        /// <p/>
        /// If the task executes successfully the Unit of Work is committed. If any exception is raised while executing the
        /// task, the Unit of Work is rolled back and the exception is thrown.
        /// </summary>
        /// <param name="task"></param>
        void Execute(Action task)
        {
            Execute(task, RollbackConfigurationType.AnyThrowable);
        }

        /// <summary>
        /// Execute the given {@code task} in the context of this Unit of Work. If the Unit of Work is not started yet
        /// it will be started.
        /// <p/>
        /// If the task executes successfully the Unit of Work is committed. If any exception is raised while executing the
        /// task, the Unit of Work is rolled back and the exception is thrown.
        /// </summary>
        /// <param name="task"></param>
        /// <param name="rollbackConfiguration"></param>
        /// <exception cref="RuntimeException"></exception>
        void Execute(Action task, IRollbackConfiguration rollbackConfiguration)
        {
            IResultMessage<R> resultMessage = ExecuteWithResult(() =>
            {
                task();
                return null;
            }, rollbackConfiguration);
            if (resultMessage.IsExceptional())
            {
                throw resultMessage.ExceptionResult();
            }
        }

        /// <summary>
        /// Execute the given {@code task} in the context of this Unit of Work. If the Unit of Work is not started yet
        /// it will be started.
        /// <p/>
        /// If the task executes successfully the Unit of Work is committed and the result of the task is returned. If any
        /// exception is raised while executing the task, the Unit of Work is rolled back and the exception is thrown.
        /// </summary>
        /// <param name="task">the task to execute</param>
        /// <returns>The result of the task wrapped in Result Message</returns>
        IResultMessage<R> ExecuteWithResult(Func<R> task)
        {
            return ExecuteWithResult(task, RollbackConfigurationType.AnyThrowable);
        }

        /// <summary>
        /// Execute the given {@code task} in the context of this Unit of Work. If the Unit of Work is not started yet
        /// it will be started.
        /// <p/>
        /// If the task executes successfully the Unit of Work is committed and the result of the task is returned. If
        /// execution fails, the {@code rollbackConfiguration} determines if the Unit of Work should be rolled back or
        /// committed.
        /// </summary>
        /// <param name="task">the task to execute</param>
        /// <param name="rollbackConfiguration">configuration that determines whether or not to rollback the
        ///     unit of work when task execution fails</param>
        /// <typeparam name="R">the type of result that is returned after successful execution</typeparam>
        /// <returns></returns>
        IResultMessage<R> ExecuteWithResult(Func<R?> task, IRollbackConfiguration rollbackConfiguration);

        /// <summary>
        /// Get the result of the task that was executed by this Unit of Work. If the Unit of Work has not been given a task
        /// to execute this method returns {@code null}.
        /// <p>
        /// Note that the value of the returned ExecutionResult's {@link ExecutionResult#isExceptionResult()} does not
        /// determine whether or not the UnitOfWork has been rolled back. To check whether or not the UnitOfWork was rolled
        /// back check {@link #isRolledBack}.
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <returns>The result of the task executed by this Unit of Work, or {@code null} if the Unit of Work has not
        /// been given a task to execute.</returns>
        ExecutionResult<R> GetExecutionResult();

        /// <summary>
        /// Check if the Unit of Work has been rolled back.
        /// </summary>
        /// <returns>{@code true} if the unit of work was rolled back, {@code false} otherwise.</returns>
        bool IsRolledBack();

        /// <summary>
        /// Check if the Unit of Work is the 'currently' active Unit of Work returned by {@link CurrentUnitOfWork#get()}.
        /// </summary>
        /// <returns>{@code true} if the Unit of Work is the currently active Unit of Work</returns>
        bool IsCurrent()
        {
            return CurrentUnitOfWork<T, R>.IsStarted() && CurrentUnitOfWork<T, R>.Get() == this;
        }
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
        private static readonly IDictionary<Phase, PhaseUtil> PhaseUtils =
            new ConcurrentDictionary<Phase, PhaseUtil>();

        static PhaseUtil()
        {
            PhaseUtils.Add(Phase.NotStarted, new PhaseUtil(false, false, 1000));
            PhaseUtils.Add(Phase.Started, new PhaseUtil(true, false, 2000));
            PhaseUtils.Add(Phase.PrepareCommit, new PhaseUtil(true, false, 3000));
            PhaseUtils.Add(Phase.Commit, new PhaseUtil(true, true, 4000));
            PhaseUtils.Add(Phase.Rollback, new PhaseUtil(true, true, 5000));
            PhaseUtils.Add(Phase.AfterCommit, new PhaseUtil(true, true, 6000));
            PhaseUtils.Add(Phase.Cleanup, new PhaseUtil(false, true, 7000));
            PhaseUtils.Add(Phase.Closed, new PhaseUtil(false, true, 8000));
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
            return PhaseUtils[phase];
        }
    }
}