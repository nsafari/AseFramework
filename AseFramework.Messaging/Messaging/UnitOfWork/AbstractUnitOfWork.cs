using System;
using System.Collections.Immutable;
using Ase.Messaging.Common;
using Ase.Messaging.Messaging.correlation;

namespace Ase.Messaging.Messaging.UnitOfWork
{
    /// <summary>
    /// Abstract implementation of the Unit of Work. It provides default implementations of all methods related to the
    /// processing of a Message.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="R"></typeparam>
    public abstract class AbstractUnitOfWork<T, R> : IUnitOfWork<T, R>
        where T : IMessage<R> where R : class
    {
        private IUnitOfWork<IMessage<R>, R>? _parentUnitOfWork;
        private bool _rolledBack;
        private Phase _phase = Phase.NotStarted;


        public void Start()
        {
            // if (logger.isDebugEnabled()) {
            // logger.debug("Starting Unit Of Work");
            // }
            // Assert.IsTrue(Phase.NotStarted.Equals(GetPhase()), () => "UnitOfWork is already started");
            _rolledBack = false;
            OnRollback(u => _rolledBack = true);
            CurrentUnitOfWork<T, R>.IfStarted(parent =>
            {
                // we're nesting.
                _parentUnitOfWork = (IUnitOfWork<IMessage<R>, R>) parent;
                // ((IUnitOfWork<IMessage<R>, R>)this).Root<T, R>().OnCleanup(r => ChangePhase(IUnitOfWork <,  >.Phase.CLEANUP, IUnitOfWork <,);  >.IUnitOfWork <, >
                // .Phase.CLOSED));
            });
            // changePhase(IUnitOfWork<,>.IUnitOfWork<,>.Phase.STARTED);
            // CurrentUnitOfWork.set(this);
        }

        public void Commit()
        {
            // if (logger.isDebugEnabled()) {
            //     logger.debug("Committing Unit Of Work");
            // }
            Assert.IsTrue(GetPhase() == Phase.Started,
                () => $"The UnitOfWork is in an incompatible phase: {GetPhase()}");
            Assert.IsTrue( ((IUnitOfWork<IMessage<R>, R>)this).IsCurrent(), () => "The UnitOfWork is not the current Unit of Work");
            try
            {
                if (((IUnitOfWork<IMessage<R>, R>)this).IsRoot())
                {
                    CommitAsRoot();
                }
                else
                {
                    CommitAsNested();
                }
            }
            finally
            {
                CurrentUnitOfWork<T, R>.Clear(this);
            }
        }

        private void CommitAsRoot() {
            try {
                try {
                    ChangePhase(Phase.PrepareCommit, Phase.Commit);
                } catch (Exception e) {
                    SetRollbackCause(e);
                    ChangePhase(Phase.Rollback);
                    throw e;
                }
                if (GetPhase() == Phase.Commit) {
                    ChangePhase(Phase.AfterCommit);
                }
            } finally {
                ChangePhase(Phase.Cleanup, Phase.Closed);
            }
        }

        private void CommitAsNested() {
            try {
                ChangePhase(Phase.PrepareCommit, Phase.Commit);
                DelegateAfterCommitToParent(this);
                _parentUnitOfWork!.OnRollback(u => ChangePhase(Phase.Rollback));
            } catch (Exception e) {
                SetRollbackCause(e);
                ChangePhase(Phase.Rollback);
                throw e;
            }
        }
        
        private void DelegateAfterCommitToParent(IUnitOfWork<T, R> unitOfWork) {
            var parent = unitOfWork.Parent<T, R>();
            if (parent != null) {
                parent.AfterCommit(this.DelegateAfterCommitToParent);
            } else {
                ChangePhase(Phase.AfterCommit);
            }
        }

        public Phase GetPhase()
        {
            return _phase;
        }
                
        /// <summary>
        /// Overwrite the current phase with the given {@code phase}.
        /// </summary>
        /// <param name="phase">the new phase of the Unit of Work</param>
        protected void SetPhase(Phase phase) {
            _phase = phase;
        }
        
        
        /// <summary>
        /// Ask the unit of work to transition to the given {@code phases} sequentially. In each of the phases the
        /// unit of work is responsible for invoking the handlers attached to each phase.
        /// <p/>
        /// By default this sets the Phase and invokes the handlers attached to the phase.
        /// </summary>
        /// <param name="phases">The phases to transition to in sequential order</param>
        protected void ChangePhase(params Phase[] phases) {
            foreach (var phase in phases) {
                SetPhase(phase);
                NotifyHandlers(phase);
            }
        }
        
        /// <summary>
        /// Notify the handlers attached to the given {@code phase}.
        /// </summary>
        /// <param name="phase">The phase for which to invoke registered handlers.</param>
        protected abstract void NotifyHandlers(Phase phase);
        
        /// <summary>
        /// Sets the cause for rolling back this Unit of Work.
        /// </summary>
        /// <param name="cause">The cause for rolling back this Unit of Work</param>
        protected abstract void SetRollbackCause(Exception cause);
        
        public void Rollback(Exception? cause)
        {
            throw new NotImplementedException();
        }

        public void OnPrepareCommit(Action<IUnitOfWork<T, R>> handler)
        {
            throw new NotImplementedException();
        }

        public void OnCommit(Action<IUnitOfWork<T, R>> handler)
        {
            throw new NotImplementedException();
        }

        public void AfterCommit(Action<IUnitOfWork<T, R>> handler)
        {
            throw new NotImplementedException();
        }

        public void OnRollback(Action<IUnitOfWork<T, R>> handler)
        {
            throw new NotImplementedException();
        }

        public void OnCleanup(Action<IUnitOfWork<T, R>> handler)
        {
            throw new NotImplementedException();
        }

        public IUnitOfWork<PT, PR>? Parent<PT, PR>() where PT : IMessage<PR> where PR : class
        {
            throw new NotImplementedException();
        }

        public T GetMessage()
        {
            throw new NotImplementedException();
        }

        public IUnitOfWork<T, R> TransformMessage<TR>(Func<T, TR> transformOperator) where TR : IMessage<object>
        {
            throw new NotImplementedException();
        }

        public MetaData GetCorrelationData()
        {
            throw new NotImplementedException();
        }

        public void RegisterCorrelationDataProvider(
            ICorrelationDataProvider.CorrelationDataFor<R> correlationDataProvider)
        {
            throw new NotImplementedException();
        }

        public IImmutableDictionary<string, object> Resources()
        {
            throw new NotImplementedException();
        }

        public IResultMessage<R> ExecuteWithResult(Func<R?> task, IRollbackConfiguration rollbackConfiguration)
        {
            throw new NotImplementedException();
        }

        public ExecutionResult<R> GetExecutionResult()
        {
            throw new NotImplementedException();
        }

        public bool IsRolledBack()
        {
            throw new NotImplementedException();
        }
    }
}