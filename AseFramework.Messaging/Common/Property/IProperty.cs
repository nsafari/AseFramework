namespace Ase.Messaging.Common.Property
{
    /// <summary>
    /// Interface describing a mechanism that can read a predefined property from a given instance.
    /// </summary>
    /// <typeparam name="T">The type of object defining this property</typeparam>
    public interface IProperty<T>
    {
     
        /// <summary>
        /// Returns the value of the property on given {@code target}.
        /// </summary>
        /// <param name="target">The instance to get the property value from</param>
        /// <typeparam name="V">The type of value expected</typeparam>
        /// <returns>the property value on {@code target}</returns>
        V GetValue<V>(T target);

    }
}