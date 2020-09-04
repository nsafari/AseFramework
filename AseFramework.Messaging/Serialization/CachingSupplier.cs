using System;
using System.Threading;

namespace AseFramework.Messaging.Serialization
{
    public class CachingSupplier<T>
    where T : class
    {
        private readonly Func<T> _delegate;
        private T? _value;

        /// <summary>
        /// Factory method for a {@link CachingSupplier} that will supply the given {@code value}.
        /// <p>
        /// This factory method should be used when the value is already available. Used this way this supplier can be
        /// serialized.
        /// </summary>
        /// <param name="value">the value to supply</param>
        /// <typeparam name="R">the type of results supplied by this supplier</typeparam>
        /// <returns>a {@link CachingSupplier} that supplies the given value</returns>
        public static CachingSupplier<R> Of<R>(R value)
        where R : class
        {
            return new CachingSupplier<R>(value);
        }

        /// <summary>
        /// Factory method for a {@link CachingSupplier} that delegates to the given {@code supplier} when asked to supply a
        /// value. If the given {@code supplier} is a {@link CachingSupplier} the instance is returned as is, if not a new
        /// {@link CachingSupplier} instance is created.
        /// </summary>
        /// <param name="supplier">supplier for which to cache the result</param>
        /// <typeparam name="R">the type of results supplied by this supplier</typeparam>
        /// <returns>a {@link CachingSupplier} based on given {@code supplier}</returns>
        public static CachingSupplier<R> Of<R>(Func<R> supplier)
        where R : class
        {
            // CachingSupplier is not func
            // if (supplier is CachingSupplier<T>)
            // {
            //     return (CachingSupplier<T>)supplier;
            // }
            return new CachingSupplier<R>(supplier);
        }

        private CachingSupplier(Func<T> @delegate)
        {
            this._delegate = @delegate;
        }

        private CachingSupplier(T value)
        {
            this._value = value;
            _delegate = () => value;
        }


        public T? Get()
        {
            T? result = _value;
            if (result == null)
            {
                result = updateAndGet(_value, (v) => v == null ? _delegate() : v);
            }
            return result;
        }


        private V? updateAndGet<V>(V? destination, Func<V?, V?> var1)
        where V : class
        {
            V? var2;
            V? var3;
            do
            {
                var2 = destination;
                var3 = var1(var2);
            } while (Interlocked.CompareExchange(ref var2, var3, var3) != var3);

            return var3;
        }

    }
}