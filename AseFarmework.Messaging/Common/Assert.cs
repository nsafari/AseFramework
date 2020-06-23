using System;

namespace Ase.Messaging.Common
{
    public class Assert
    {

        /// <summary>
        /// Asserts that the given {@code expression} is true. If not, an ArgumentException is thrown.
        /// </summary>
        /// <param name="expression">the state validation expression</param>
        /// <param name="messageSupplier">Supplier of the exception message if the expression evaluates to false</param>
        /// <exception cref="ArgumentException"></exception>
        public static void IsTrue(bool expression, Func<string> messageSupplier)
        {
            if (!expression)
            {
                throw new ArgumentException(messageSupplier.Invoke());
            }
        }

        /// <summary>
        /// Assert that the given {@code value} is not {@code null}. If not, an ArgumentException is thrown. 
        /// </summary>
        /// <param name="value">the value not to be {@code null}</param>
        /// <param name="messageSupplier">Supplier of the exception message if the assertion fails</param>
        public static void NotNull(object value, Func<string> messageSupplier) {
            IsTrue(value != null, messageSupplier);
        }
    }
}