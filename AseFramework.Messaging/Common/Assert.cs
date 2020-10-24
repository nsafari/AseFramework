using System;

namespace Ase.Messaging.Common
{
    public abstract class Assert
    {
        private Assert()
        {
            // utility class
        }

        /// <summary>
        /// Asserts that the value of {@code state} is true. If not, an IllegalStateException is thrown.
        /// </summary>
        /// <param name="state">the state validation expression</param>
        /// <param name="messageSupplier">Supplier of the exception message if state evaluates to false</param>
        /// <exception cref="ArgumentException"></exception>
        public static void State(bool state, Func<string> messageSupplier)
        {
            if (!state)
            {
                throw new ArgumentException(messageSupplier());
            }
        }

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
        /// Asserts that the given {@code expression} is false. If not, an IllegalArgumentException is thrown.
        /// </summary>
        /// <param name="expression">the state validation expression</param>
        /// <param name="messageSupplier">Supplier of the exception message if the expression evaluates to true</param>
        public static void IsFalse(bool expression, Func<string> messageSupplier)
        {
            if (expression)
            {
                throw new ArgumentException(messageSupplier());
            }
        }


        /// <summary>
        /// Assert that the given {@code value} is not {@code null}. If not, an ArgumentException is thrown. 
        /// </summary>
        /// <param name="value">the value not to be {@code null}</param>
        /// <param name="messageSupplier">Supplier of the exception message if the assertion fails</param>
        public static void NotNull(object? value, Func<string> messageSupplier)
        {
            IsTrue(value != null, messageSupplier);
        }

        /// <summary>
        /// Assert that the given {@code value} will result to {@code true} through the {@code assertion} {@link Predicate}.
        /// If not, the {@code exceptionSupplier} provides an exception
        /// to be thrown.
        /// </summary>
        /// <param name="value">a {@code T} specifying the value to assert</param>
        /// <param name="assertion">a {@link Predicate} to test {@code value} against</param>
        /// <param name="exceptionSupplier">a {@link Supplier} of the exception {@code X} if {@code assertion} evaluates to false</param>
        /// <typeparam name="T">a generic specifying the type of the {@code value}, which is the input for the {@code assertion}</typeparam>
        /// <typeparam name="X">a generic extending {@link Throwable} which will be provided by the
        /// {@code exceptionSupplier}</typeparam>
        /// <exception cref="X">if the {@code value} asserts to {@code false} by the {@code assertion}</exception>
        public static void AssertThat<T, X>(
            T value,
            Predicate<T> assertion,
            Func<X> exceptionSupplier)
            where X : Exception
        {
            if (!assertion(value))
            {
                throw exceptionSupplier();
            }
        }

        /// <summary>
        /// Assert that the given {@code value} is non null. If not, the {@code exceptionSupplier} provides an exception to
        /// be thrown.
        /// </summary>
        /// <param name="value">a {@code T} specifying the value to assert</param>
        /// <param name="exceptionSupplier">a {@link Supplier} of the exception {@code X} if {@code value} equals null</param>
        /// <typeparam name="T">a generic specifying the type of the {@code value}, which is the input for the {@code assertion}</typeparam>
        /// <typeparam name="X">a generic extending {@link Throwable} which will be provided by the
        /// {@code exceptionSupplier}</typeparam>
        public static void AssertNonNull<T, X>(
            T value,
            Func<X> exceptionSupplier)
            where X : Exception
        {
            AssertThat(value, (param) => param != null, exceptionSupplier);
        }
    }
}