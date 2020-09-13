using System;

namespace Ase.Messaging.Serialization
{
    /// <summary>
    /// Interface describing the structure of a serialized object.
    /// </summary>
    /// <typeparam name="T">The data type representing the serialized object</typeparam>
    public interface ISerializedObject<T>
    {
        /// <summary>
        /// Returns the type of this representation's data.
        /// </summary>
        /// <returns>the type of this representation's data</returns>
        Type GetContentType();

        /// <summary>
        /// Returns the description of the type of object contained in the data.
        /// </summary>
        /// <returns>the description of the type of object contained in the data</returns>
        ISerializedType GetType();

        /// <summary>
        /// The actual data of the serialized object.
        /// </summary>
        /// <returns>the actual data of the serialized object</returns>
        T GetData();
        
    }
}