using System.Collections;
using System.Collections.Generic;

namespace Ase.Messaging.Messaging
{
    public static class DictionaryExtension
    {
        public static IEnumerable<V> YieldIfPresent<K, V>(this IReadOnlyDictionary<K, V> source, K key)
        where K : notnull
        {
            if (source.TryGetValue(key, out var value))
            {
                yield return value;
            }
        }
    }
}