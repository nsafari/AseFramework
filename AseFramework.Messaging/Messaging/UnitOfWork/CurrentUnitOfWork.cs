using System;
using System.Linq;
using System.Threading;
using Nito.Collections;

namespace Ase.Messaging.Messaging.UnitOfWork
{
    /// <summary>
    /// Default entry point to gain access to the current UnitOfWork. Components managing transactional boundaries can
    /// register and clear UnitOfWork instances, which components can use.
    /// </summary>
    public class CurrentUnitOfWork<T, R>
        where T : IMessage<R> where R : class
    {
        private static readonly ThreadLocal<Deque<IUnitOfWork<T, R>>?> Current =
            new ThreadLocal<Deque<IUnitOfWork<T, R>>?>();



        /// <summary>
        /// Indicates whether a unit of work has already been started. This method can be used by interceptors to prevent
        /// nesting of UnitOfWork instances.
        /// </summary>
        /// <returns>whether a UnitOfWork has already been started.</returns>
        public static bool IsStarted()
        {
            return Current.Value != null && Current.Value.Count != 0;
        }

        /// <summary>
        /// If a UnitOfWork is started, invokes the given {@code consumer} with the active Unit of Work. Otherwise,
        /// it does nothing
        /// </summary>
        /// <param name="consumer">The consumer to invoke if a Unit of Work is active</param>
        /// <returns>{@code true} if a unit of work is active, {@code false} otherwise</returns>
        public static bool IfStarted(Action<IUnitOfWork<T, R>> consumer)
        {
            if (!IsStarted()) return false;
            consumer(Get());
            return true;
        }


        /// <summary>
        /// If a Unit of Work is started, execute the given {@code function} on it. Otherwise, returns an empty Optional.
        /// Use this method when you wish to retrieve information from a Unit of Work, reverting to a default when no Unit
        /// of Work is started.
        /// </summary>
        /// <param name="function">The function to apply to the unit of work, if present</param>
        /// <returns>an optional containing the result of the function, or an empty Optional when no Unit of Work was started</returns>
        public static RT? Map<RT>(Func<IUnitOfWork<T, R>, RT> func)
        where RT : class
        {
            return IsStarted() ? func(Get()) : null;
        }


        /// <summary>
        /// Gets the UnitOfWork bound to the current thread. If no UnitOfWork has been started, an {@link
        /// IllegalStateException} is thrown.
        /// <p/>
        /// To verify whether a UnitOfWork is already active, use {@link #isStarted()}.
        /// </summary>
        /// <returns>The UnitOfWork bound to the current thread.</returns>
        /// <exception cref="InvalidOperationException">if no UnitOfWork is active</exception>
        public static IUnitOfWork<T, R> Get()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("No UnitOfWork is currently started for this thread.");
            }

            return Current.Value!.RemoveFromFront();
        }

        private static bool IsEmpty()
        {
            Deque<IUnitOfWork<T, R>>? unitsOfWork = Current.Value;
            return unitsOfWork == null || unitsOfWork.Count == 0;
        }

        /// <summary>
        /// Commits the current UnitOfWork. If no UnitOfWork was started, an {@link IllegalStateException} is thrown.
        /// </summary>
        public static void Commit()
        {
            Get().Commit();
        }

        /// <summary>
        /// Binds the given {@code unitOfWork} to the current thread. If other UnitOfWork instances were bound, they
        /// will be marked as inactive until the given UnitOfWork is cleared.
        /// </summary>
        /// <param name="unitOfWork">The UnitOfWork to bind to the current thread.</param>
        public static void Set(IUnitOfWork<T, R> unitOfWork)
        {
            if (Current.Value == null)
            {
                Current.Value = new Deque<IUnitOfWork<T, R>>();
            }
            Current.Value.AddToFront(unitOfWork);
        }

        /// <summary>
        /// Clears the UnitOfWork currently bound to the current thread, if that UnitOfWork is the given
        /// {@code unitOfWork}.
        /// </summary>
        /// <param name="unitOfWork">The UnitOfWork expected to be bound to the current thread.</param>
        public static void Clear(IUnitOfWork<T, R> unitOfWork)
        {
            if (!IsStarted())
            {
                throw new InvalidOperationException("Could not clear this UnitOfWork. There is no UnitOfWork active.");
            }
            if (Current.Value![0] == unitOfWork)
            {
                Current.Value.RemoveFromFront();
                if (Current.Value.Any())
                {
                    Current.Value = null;
                }
            }
            else
            {
                throw new InvalidOperationException("Could not clear this UnitOfWork. It is not the active one.");
            }
        }


        /// <summary>
        /// Returns the Correlation Data attached to the current Unit of Work, or an empty {@link MetaData} instance
        /// if no Unit of Work is started.
        /// </summary>
        /// <returns>a MetaData instance representing the current Unit of Work's correlation data, or an empty MetaData
        /// instance if no Unit of Work is started.</returns>
        /// <see cref="UnitOfWork.getCorrelationData()"/>
        /// <exception cref="NotImplementedException"></exception>
        public static MetaData CorrelationData()
        {
            return CurrentUnitOfWork<T, R>.Map((unitOfWork) => unitOfWork.GetCorrelationData()) ?? MetaData.EmptyInstance;
        }


    }
}