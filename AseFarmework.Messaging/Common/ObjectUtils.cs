using System;

namespace Ase.Messaging.Common
{
    /// <summary>
    /// Miscellaneous object utility methods
    /// </summary>
    public abstract class ObjectUtils
    {
     
        private ObjectUtils() {
            // prevent instantiation
        }

        /// <summary>
        /// Returns the given instance, if not {@code null}, or otherwise the value provided by {@code defaultProvider}.
        /// </summary>
        /// <param name="instance">The value to return, if not {@code null}</param>
        /// <param name="defaultProvider">To provide the value, when {@code instance} is {@code null}</param>
        /// <typeparam name="T">The type of value to return</typeparam>
        /// <returns>{@code instance} if not {@code null}, otherwise the value provided by {@code defaultProvider}</returns>
        public static T GetOrDefault<T>(T instance, Func<T> defaultProvider)
        {
            return (instance ?? defaultProvider.Invoke())!;
        }
    }
}