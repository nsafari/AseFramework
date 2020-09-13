using Ase.Messaging.Common;

namespace Ase.Messaging.Serialization
{
    /// <summary>
    /// SerializedType implementation that takes its properties as constructor parameters.
    /// </summary>
    public class SimpleSerializedType: ISerializedType
    {
        private static readonly ISerializedType EMPTY_TYPE = new SimpleSerializedType("empty", null);
        private readonly string _type;
        private readonly string? _revisionId;

        /// <summary>
        /// Returns the type that represents an empty message, of undefined type. The type of such message is "empty" and
        /// always has a {@code null} revision.
        /// </summary>
        /// <returns>the type representing an empty message</returns>
        public static ISerializedType EmptyType() {
            return EMPTY_TYPE;
        }
        
        /// <summary>
        /// Initialize with given {@code objectType} and {@code revisionNumber}
        /// </summary>
        /// <param name="objectType">The description of the serialized object's type</param>
        /// <param name="revisionNumber">The revision of the serialized object's type</param>
        public SimpleSerializedType(string objectType, string? revisionNumber) {
            Assert.NotNull(objectType, () => "objectType cannot be null");
            _type = objectType;
            _revisionId = revisionNumber;
        }

        public string GetName() {
            return _type;
        }
        
        public string? GetRevision() {
            return _revisionId;
        }

        protected bool Equals(SimpleSerializedType other)
        {
            return _type == other._type && _revisionId == other._revisionId;
        }

        public override int GetHashCode()
        {
            return System.HashCode.Combine(_type, _revisionId);
        }

        public override string ToString() {
            return $"SimpleSerializedType[{_type}] (revision {_revisionId})";
        }



    }
}