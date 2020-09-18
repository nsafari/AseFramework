using System;
using Ase.Messaging.Messaging;

namespace Ase.Messaging.Serialization
{
    /// <summary>
    /// Represents the serialized form of a {@link MetaData} instance.
    /// </summary>
    /// <typeparam name="T">The data type representing the serialized object</typeparam>
    public class SerializedMetaData<T>: ISerializedObject<T>
    {
        private static readonly string MetadataClassName = nameof(MetaData);

        private readonly SimpleSerializedObject<T> _delegate;

        /// <summary>
        /// Construct an instance with given {@code bytes} representing the serialized form of a {@link MetaData}
        /// instance.
        /// </summary>
        /// <param name="data">data representing the serialized form of a {@link MetaData} instance.</param>
        /// <param name="dataType">The type of data</param>
        public SerializedMetaData(T data, Type dataType) {
            _delegate = new SimpleSerializedObject<T>(data, dataType, MetadataClassName, null);
        }

        /// <summary>
        /// Indicates whether the given {@code serializedObject} represents a serialized form of a MetaData object,
        /// such as the ones created by this class (see {@link #SerializedMetaData(Object, Class)}.
        /// </summary>
        /// <param name="serializedObject">The object to check for Meta Data</param>
        /// <returns>{@code true} if the serialized objects represents serialized meta data, otherwise
        /// {@code false}.</returns>
        public static bool IsSerializedMetaData(ISerializedObject<object>? serializedObject) {
            return serializedObject?.Type() != null && MetadataClassName.Equals(serializedObject.Type()!.GetName());
        }
        
        public T GetData() {
            return _delegate.GetData();
        }
        
        public Type GetContentType() {
            return _delegate.GetContentType();
        }

        public ISerializedType Type() {
            return _delegate.Type();
        }

        public override bool Equals(object? obj)
        {
            if (this == obj) {
                return true;
            }
            if (obj == null || GetType() != obj.GetType()) {
                return false;
            }
            SerializedMetaData<object> that = (SerializedMetaData<object>) obj;
            return Equals(_delegate, that._delegate);

        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_delegate);
        }
    }
}