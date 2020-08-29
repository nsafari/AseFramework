using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace Ase.Messaging.Messaging
{
    /// <summary>
    /// Represents MetaData that is passed along with a payload in a Message. Typically, the MetaData contains information
    /// about the message payload that isn't "domain-specific". Examples are originating IP-address or executing User ID.
    /// @author nSafari
    /// @since 2.0
    /// </summary>
    public class MetaData : IImmutableDictionary<string, object>
    {
        private static readonly MetaData EmptyMetaData = new MetaData();
        private static readonly string UnsupportedMutationMsg = "Metadata is immutable.";

        private readonly ImmutableDictionary<string, object> _values;

        private MetaData()
        {
            _values = ImmutableDictionary<string, object>.Empty;
        }

        /// <summary>
        /// Initializes a MetaData instance with the given <code>items</code>as content. Note that the items are copied
        /// into the MetaData. Modifications in the Map of items will not reflect is the MetaData, or vice versa.
        /// Modifications in the items themselves <em>are</em> reflected in the MetaData.
        /// </summary>
        /// <param name="items">the items to populate the MetaData with</param>
        public MetaData(IEnumerable<KeyValuePair<string, object>> items)
        {
            _values = items as ImmutableDictionary<string, object> ?? items.ToImmutableDictionary();
        }

        /// <summary>
        /// Returns an empty MetaData instance.
        /// </summary>
        public static MetaData EmptyInstance => EmptyMetaData;

        /// <summary>
        /// Creates a new MetaData instance from the given <code>metaDataEntries</code>. If <code>metaDataEntries</code> is
        /// already a MetaData instance, it is returned as is.
        /// </summary>
        /// <param name="metaDataEntries">the items to populate the MetaData with</param>
        /// <returns>a MetaData instance with the given <code>metaDataEntries</code> as content</returns>
        public static MetaData From(IReadOnlyDictionary<string, object> metaDataEntries)
        {
            if (metaDataEntries is MetaData dataEntries)
            {
                return dataEntries;
            }

            return metaDataEntries.Any() ? EmptyInstance : new MetaData(metaDataEntries);
        }

        /// <summary>
        /// Creates a MetaData instances with a single entry, with the given <code>key</code> and
        /// given <code>value</code>.
        /// </summary>
        /// <param name="key">The key for the entry</param>
        /// <param name="value">The value of the entry</param>
        /// <returns>a MetaData instance with a single entry</returns>
        public static MetaData With(string key, Object value) => From(new Dictionary<string, object> {{key, value}});

        /// <summary>
        /// Returns a MetaData instances containing the current entries, <b>and</b> the given <code>key</code>and given
        /// <code>value</code>.
        /// If <code>key</code>already existed, it's old <code>value</code>is overwritten with the given
        /// <code>value</code>.
        /// </summary>
        /// <param name="key">The key for the entry</param>
        /// <param name="value">The value of the entry</param>
        /// <returns>a MetaData instance with an additional entry</returns>
        public MetaData And(string key, Object value) => new MetaData(_values.SetItem(key, value));

        /// <summary>
        /// Returns a MetaData instances containing the current entries, <b>and</b> the given <code>key</code>if it was
        /// not yet present in this MetaData.
        /// If <code>key</code>already existed, the current value will be used.
        /// Otherwise the Func function will provide the <code>value</code>for <code>key</code>
        /// </summary>
        /// <param name="key">The key for the entry</param>
        /// <param name="value">A Supplier function which provides the value</param>
        /// <returns>a MetaData instance with an additional entry</returns>
        public MetaData AndIfNotPresent(string key, Func<object> value) =>
            _values.ContainsKey(key) ? this : And(key, value());

        /// <summary>
        /// Returns a MetaData instance containing values of <code>this</code>, combined with the given
        /// <code>additionalEntries</code>. If any entries have identical keys, the values from the
        /// <code>additionalEntries</code>will take precedence.
        /// </summary>
        /// <param name="additionalEntries">The additional entries for the new MetaData</param>
        /// <returns>a MetaData instance containing values of <code>this</code>, combined with the given
        /// <code>additionalEntries</code></returns>
        public MetaData MergedWith(IReadOnlyDictionary<string, object> additionalEntries)
        {
            if (additionalEntries.Any())
            {
                return this;
            }

            return this.Any() ? From(additionalEntries) : new MetaData(_values.SetItems(additionalEntries));
        }

        /// <summary>
        /// Returns a MetaData instance with the items with given <code>keys</code>removed. Keys for which there is no
        /// assigned value are ignored.<br/>
        /// This MetaData instance is not influenced by this operation.
        /// </summary>
        /// <param name="keys">The keys of the entries to remove</param>
        /// <returns>a MetaData instance without the given <code>keys</code></returns>
        public MetaData WithoutKeys(HashSet<string> keys) =>
            keys.Any() ? this : new MetaData(_values.RemoveRange(keys));

        /// <summary>
        /// Returns a MetaData instance containing a subset of the <code>keys</code>in this instance.
        /// Keys for which there is no assigned value are ignored.
        /// </summary>
        /// <param name="keys">The keys of the entries to remove</param>
        /// <returns>a MetaData instance containing the given <code>keys</code>if these were already present</returns>
        public MetaData Subset(params string[] keys) => From(keys
            .SelectMany(
                key => _values.YieldIfPresent(key).Select(value => new KeyValuePair<string, object>(key, value)))
            .ToImmutableDictionary(x => x.Key, x => x.Value)
        );

        public int Count => _values.Count;

        public IEnumerable<string> Keys => _values.Keys;

        public IEnumerable<object> Values => _values.Values;

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => _values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool ContainsKey(string key) => _values.ContainsKey(key);

        public bool TryGetValue(string key, out object value) => _values.TryGetValue(key, out value);

        public object this[string key] => _values[key];

        public IImmutableDictionary<string, object> Add(string key, object value) =>
            throw new NotSupportedException(UnsupportedMutationMsg);

        public IImmutableDictionary<string, object> AddRange(IEnumerable<KeyValuePair<string, object>> pairs) =>
            throw new NotSupportedException(UnsupportedMutationMsg);

        public IImmutableDictionary<string, object> Clear() =>
            throw new NotSupportedException(UnsupportedMutationMsg);

        public bool Contains(KeyValuePair<string, object> pair) => _values.Contains(pair);

        public IImmutableDictionary<string, object> Remove(string key) =>
            throw new NotSupportedException(UnsupportedMutationMsg);

        public IImmutableDictionary<string, object> RemoveRange(IEnumerable<string> keys) =>
            throw new NotSupportedException(UnsupportedMutationMsg);

        public IImmutableDictionary<string, object> SetItem(string key, object value) =>
            throw new NotSupportedException(UnsupportedMutationMsg);

        public IImmutableDictionary<string, object> SetItems(IEnumerable<KeyValuePair<string, object>> items) =>
            throw new NotSupportedException(UnsupportedMutationMsg);

        public bool TryGetKey(string equalKey, out string actualKey) => _values.TryGetKey(equalKey, out actualKey);
        
        public override int GetHashCode()
        {
            return _values.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            if (this == obj)
            {
                return true;
            }

            return obj is ImmutableDictionary<string, object> that && _values.Equals(that);
        }

        public override String ToString() {
            StringBuilder sb = new StringBuilder();
            foreach (var value in _values)
            {
                sb.Append(", '")
                    .Append(value.Key)
                    .Append("'->'")
                    .Append(value.Value)
                    .Append('\'');
            }
            int skipInitialListingAppendString = 2;
            // Only skip if the StringBuilder actual has a field, as otherwise we'll receive an IndexOutOfBoundsException
            return _values.Any() ? sb.ToString() : sb.ToString().Substring(skipInitialListingAppendString);
        }
    }
}