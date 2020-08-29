using System;
using Microsoft.CSharp.RuntimeBinder;

namespace Ase.Messaging.Messaging.UnitOfWork
{
    public class RollbackConfigurationType : IRollbackConfiguration
    {
        private readonly string _display;
        private readonly Func<Exception, bool> _rollBackOn;

        public RollbackConfigurationType(string display, Func<Exception, bool> rollBackOn)
        {
            _display = display;
            _rollBackOn = rollBackOn;
        }

        /// <summary>
        /// Configuration that never performs a rollback of the unit of work.
        /// </summary>
        public static readonly RollbackConfigurationType Never =
            new RollbackConfigurationType("NEVER", ex => false);

        /// <summary>
        /// Configuration that prescribes a rollback on any sort of exception.
        /// </summary>
        public static readonly RollbackConfigurationType AnyThrowable =
            new RollbackConfigurationType("ANY_THROWABLE", exception => exception != null);

        /// <summary>
        /// Configuration that prescribes a rollback on any sort of system exception.
        /// </summary>
        public static readonly RollbackConfigurationType UncheckedExceptions =
            new RollbackConfigurationType("SYSTEM_EXCEPTIONS", exception => exception is SystemException);

        /// <summary>
        /// Configuration that prescribes a rollback on not system exception.
        /// </summary>
        public static readonly RollbackConfigurationType RuntimeException =
            new RollbackConfigurationType("NOT_SYSTEM_EXCEPTION", exception => exception is SystemException);

        public bool rollBackOn(Exception throwable)
        {
            return _rollBackOn(throwable);
        }

        public override bool Equals(object? obj)
        {
            return obj is RollbackConfigurationType rollbackConfigurationType &&
                   rollbackConfigurationType.GetHashCode().Equals(GetHashCode());
        }

        public override int GetHashCode()
        {
            return _display.GetHashCode();
        }
    }
}