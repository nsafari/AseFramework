using Ase.Messaging.Common;

namespace AseFramework.Modeling.Saga
{
    /// <summary>
    /// A combination of key and value by which a Saga can be found. Sagas are triggered by events that have a property that
    /// the saga is associated with. A single Association Value can lead to multiple Sagas, and a single Saga can be
    /// associated with multiple Association Values.
    /// <p/>
    /// Two association values are considered equal when both their key and value are equal. For example, a Saga managing
    /// Orders could have a AssociationValue with key &quot;orderId&quot; and the order identifier as value.
    /// </summary>
    public class AssociationValue
    {
        private readonly string _propertyKey;
        private readonly string? _propertyValue;

        /// <summary>
        /// Creates a Association Value instance with the given {@code key} and {@code value}.
        /// </summary>
        /// <param name="key">The key of the Association Value. Usually indicates where the value comes from.</param>
        /// <param name="value">The value corresponding to the key of the association. It is highly recommended to only use</param>
        public AssociationValue(string key, string? value) {
            Assert.NotNull(key, () => "Cannot associate a Saga with a null key");
            _propertyKey = key;
            _propertyValue = value;
        }
        
        /// <summary>
        /// Returns the key of this association value. The key usually indicates where the property's value comes from.
        /// </summary>
        /// <returns>the key of this association value</returns>
        public string GetKey() {
            return _propertyKey;
        }
        
        /// <summary>
        /// Returns the value of this association.
        /// </summary>
        /// <returns>the value of this association. Never {@code null}.</returns>
        public string? GetValue() {
            return _propertyValue;
        }

        public override bool Equals(object? obj)
        {
            if (this == obj) {
                return true;
            }
            if (obj == null || GetType() != obj.GetType()) {
                return false;
            }
            AssociationValue that = (AssociationValue) obj;
            return Equals(_propertyKey, that._propertyKey) && 
                   Equals(_propertyValue, that._propertyValue);

        }

        public override int GetHashCode()
        {
            return System.HashCode.Combine(_propertyKey, _propertyValue);
        }
    }
}