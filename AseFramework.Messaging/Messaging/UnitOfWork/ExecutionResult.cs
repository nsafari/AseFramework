using System;

namespace Ase.Messaging.Messaging.UnitOfWork
{
    /// <summary>
    /// Class of objects that contain the result of an executed task.
    /// </summary>
    public class ExecutionResult<R>
        where R : class
    {
        private readonly IResultMessage<R> _result;
        
        /// <summary>
        /// Initializes an {@link ExecutionResult} from the given {@code result}.
        /// </summary>
        /// <param name="result">the result message of an executed task</param>
        public ExecutionResult(IResultMessage<R> result) {
            _result = result;
        }
        
        /// <summary>
        /// Return the execution result message.
        /// </summary>
        /// <returns>the execution result message</returns>
        public IResultMessage<R> GetResult() {
            return _result;
        }
        
        /// <summary>
        /// Get the execution result in case the result is an exception. If the execution yielded no exception this method
        /// returns {@code null}.
        /// </summary>
        /// <returns>The exception raised during execution of the task if any, {@code null} otherwise.</returns>
        public Exception? GetExceptionResult() {
            return IsExceptionResult() ? _result.ExceptionResult() : null;
        }

        /// <summary>
        /// Check if the result of the execution yielded an exception.
        /// </summary>
        /// <returns>{@code true} if execution of the task gave rise to an exception, {@code false} otherwise.</returns>
        public bool IsExceptionResult() {
            return _result.IsExceptional();
        }

        public override bool Equals(object? obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            ExecutionResult<R> that = (ExecutionResult<R>) obj;
            return _result.Equals(that._result);
        }

        public override int GetHashCode()
        {
            return _result.GetHashCode();
        }

        public override string ToString()
        {
            return $"ExecutionResult containing {_result}";
        }
    }
}