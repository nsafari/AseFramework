using System;
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
        private static readonly ThreadLocal<Deque<IUnitOfWork<T, R>>> Current =
            new ThreadLocal<Deque<IUnitOfWork<T, R>>>();


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
            throw new NotImplementedException();
        }
    }
}