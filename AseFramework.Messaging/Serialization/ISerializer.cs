using System;

namespace Ase.Messaging.Serialization
{
    public interface ISerializer
    {
        ISerializedObject<T> Serialize<T>(object @object, Type expectedRepresentation);

        bool CanSerializeTo(Type expectedRepresentation);

        T Deserialize<S, T>(ISerializedObject<S> serializedObject);

        Type ClassForType(ISerializedType type);
        
        ISerializedType TypeForClass(Type type);
        
        IConverter GetConverter();

    }
}