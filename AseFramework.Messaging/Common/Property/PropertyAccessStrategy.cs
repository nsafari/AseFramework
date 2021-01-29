using System;
using System.Runtime.Loader;

namespace Ase.Messaging.Common.Property
{
    public class PropertyAccessStrategy: IComparable<PropertyAccessStrategy>
    {
        private IComparable<PropertyAccessStrategy> _comparableImplementation;
        public int CompareTo(PropertyAccessStrategy other)
        {
            return _comparableImplementation.CompareTo(other);
        }
    }
}