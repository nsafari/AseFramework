namespace Ase.Messaging.Common.transaction
{
    public interface ITransaction
    {
        
        /// <summary>
        /// Commit this transaction.
        /// </summary>
        void Commit();
        
        /// <summary>
        /// Roll back this transaction.
        /// </summary>
        void Rollback();
        
    }
}