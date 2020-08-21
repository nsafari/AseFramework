using System;
using Ase.Messaging.Common;

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
        
        private IUnitOfWork<IMessage<R>, R> _parentUnitOfWork;
        private bool _rolledBack;

        public void Start()
        {
            // if (logger.isDebugEnabled()) {
                // logger.debug("Starting Unit Of Work");
            // }
            Assert.IsTrue(Phase.NotStarted.Equals(GetPhase()), () => "UnitOfWork is already started");
            _rolledBack = false;
            OnRollback(u => _rolledBack = true);
            CurrentUnitOfWork<T, R>.IfStarted(parent => {
                // we're nesting.
                this._parentUnitOfWork = (IUnitOfWork<IMessage<R>, R>) parent;
                root().onCleanup(r -> changePhase(IUnitOfWork<,>.Phase.CLEANUP, IUnitOfWork<,>.IUnitOfWork<,>.Phase.CLOSED));
            });
            // changePhase(IUnitOfWork<,>.IUnitOfWork<,>.Phase.STARTED);
            // CurrentUnitOfWork.set(this);

        }

        public void Commit()
        {
            throw new NotImplementedException();
        }

        public void Rollback(Exception? cause)
        {
            throw new NotImplementedException();
        }

        public IUnitOfWork<T, R>.Phase GetPhase()
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
    }
}