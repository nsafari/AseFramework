namespace Ase.Messaging.Serialization
{

    /// <summary>
    /// Describes the type of a serialized object. This information is used to decide how to deserialize an object.
    /// </summary>
    public interface ISerializedType
    {
        /// <summary>
        /// Returns the type that represents an empty message, of undefined type. The type of such message is "empty" and
        /// always has a {@code null} revision.
        /// </summary>
        /// <returns>the type representing an empty message</returns>
        static ISerializedType EmptyType() {
            return SimpleSerializedType.EmptyType();
        }

        /// <summary>
        /// Check whether the {@code serializedType} equals {@link SerializedType#emptyType#getName()} and returns a
        /// corresponding {@code true} or {@code false} whether this is the case or not. The given {@code serializedType}
        /// should not be {@code null} as otherwise a {@link NullPointerException} will be thrown.
        /// </summary>
        /// <param name="serializedType">the type to check whether it equals {@link SerializedType#emptyType()}</param>
        /// <returns>{@code true} if the {@code serializedType} does equals the {@link SerializedType#emptyType()#getName()}
        /// and {@code false} if it does</returns>
        static bool IsEmptyType(ISerializedType serializedType) {
            return EmptyType().GetName() == serializedType.GetName();
        }
        
        /// <summary>
        /// Returns the name of the serialized type. This may be the class name of the serialized object, or an alias for
        /// that name.
        /// </summary>
        /// <returns>the name of the serialized type</returns>
        string GetName();

        /// <summary>
        /// Returns the revision identifier of the serialized object. This revision identifier is used by upcasters to
        /// decide how to transform serialized objects during deserialization.
        /// </summary>
        /// <returns>the revision identifier of the serialized object</returns>
        string GetRevision();

    }
}