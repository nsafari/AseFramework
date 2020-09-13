using System;
using Ase.Messaging.Common;

namespace Ase.Messaging.Serialization
{
    public class SimpleSerializedObject<T> : ISerializedObject<T>
    {
        private readonly T _data;
        private readonly ISerializedType _type;
        private readonly Type _dataType;

        public SimpleSerializedObject(T data, Type dataType, ISerializedType serializedType)
        {
            Assert.NotNull(data, () => "Data for a serialized object cannot be null");
            Assert.NotNull(serializedType, () => "The type identifier of the serialized object");
            _data = data;
            _dataType = dataType;
            _type = serializedType;
        }

        public SimpleSerializedObject(T data, Type dataType, String type, String revision)
            : this(data, dataType, new SimpleSerializedType(type, revision))
        {
        }
        
        public T GetData() {
            return _data;
        }
        
        public Type GetContentType() {
            return _dataType;
        }
        
        public ISerializedType Type() {
            return _type;
        }
        
        public override bool Equals(object? obj) {
            if (this == obj) {
                return true;
            }
            if (obj == null || GetType() != obj.GetType()) {
                return false;
            }
            
            SimpleSerializedObject<T> that = (obj as SimpleSerializedObject<T>)!;
            return _data!.Equals(that._data) && 
                   _type.Equals(that._type) &&
                   _dataType == that._dataType;
        }

        public override int GetHashCode() {
            return HashCode.Combine(_data, _type, _dataType);
        }

        public override string ToString() {
            return $"SimpleSerializedObject [{_type}]";
        }
    }
}