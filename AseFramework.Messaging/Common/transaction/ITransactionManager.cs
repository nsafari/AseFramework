using System;

namespace Ase.Messaging.Common.transaction
{
    public interface ITransactionManager
    {
        /// <summary>
        /// Starts a transaction. The return value is the started transaction that can be committed or rolled back.
        /// </summary>
        /// <returns>The object representing the transaction</returns>
        ITransaction StartTransaction();

        /// <summary>
        /// Executes the given {@code task} in a new {@link Transaction}. The transaction is committed when the task
        /// completes normally, and rolled back when it throws an exception.
        /// </summary>
        /// <param name="task">The task to execute</param>
        /// <exception cref="Exception"></exception>
        void ExecuteInTransaction(Action task)
        {
            ITransaction transaction = StartTransaction();
            try
            {
                task();
                transaction.Commit();
            }
            catch (Exception e)
            {
                transaction.Rollback();
                throw e;
            }
        }

        /// <summary>
        /// Invokes the given {@code supplier} in a transaction managed by the current TransactionManager. Upon completion
        /// of the call, the transaction will be committed in the case of a regular return value, or rolled back in case an
        /// exception occurred.
        /// <p>
        /// This method is an alternative to {@link #executeInTransaction(Runnable)} in cases where a result needs to be
        /// returned from the code to be executed transactionally.
        /// </summary>
        /// <param name="supplier">The supplier of the value to return</param>
        /// <typeparam name="T">The type of value to return</typeparam>
        /// <returns>The value returned by the supplier</returns>
        /// <exception cref="Exception"></exception>
        T FetchInTransaction<T>(Func<T> supplier)
        {
            ITransaction transaction = StartTransaction();
            try
            {
                T result = supplier();
                transaction.Commit();
                return result;
            }
            catch (Exception e)
            {
                transaction.Rollback();
                throw e;
            }
        }
    }
}