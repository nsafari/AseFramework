using System;

namespace Ase.Messaging.Serialization
{
    public interface IConverter
    {
        bool CanConvert(Type sourceType, Type targetType);

        T Convert<T>(object original, Type targetType)
        {
            return Convert<T>(original, original.GetType(), targetType);
        }

        T Convert<T>(object original, Type sourceType, Type targetType);

        ISerializedObject<T> Convert<T>(ISerializedObject<object> original, Type targetType)
        {
            if (original.GetContentType() == targetType)
            {
                return (ISerializedObject<T>) original;
            }

            return new SimpleSerializedObject<T>(
                Convert<T>(original.GetData(), original.GetContentType(), targetType),
                targetType,
                original.Type()
            );
        }
    }
}