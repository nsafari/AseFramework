using System;

namespace Ase.Messaging.Messaging.UnitOfWork
{
    public class CurrentUnitOfWork
    {

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