using System;
using System.Collections.Generic;

namespace Ase.Messaging.Common
{

    public abstract class ReflectionUtils
    {
        /// <summary>
        /// Finds the most specific supertype of <tt>type</tt> whose erasure is <tt>searchClass</tt>.
        /// In other words, returns a type representing the class <tt>searchClass</tt> plus its exact type parameters in
        /// <tt>type</tt>.
        /// <p>
        /// <ul>
        /// <li>Returns an instance of {@link ParameterizedType} if <tt>searchClass</tt> is a real class or interface and
        ///     <tt>type</tt> has parameters for it</li>
        /// <li>Returns an instance of {@link GenericArrayType} if <tt>searchClass</tt> is an array type, and <tt>type</tt>
        /// has type parameters for it</li>
        /// <li>Returns an instance of {@link Class} if <tt>type</tt> is a raw type, or has no type parameters for
        /// <tt>searchClass</tt></li>
        /// <li>Returns null if <tt>searchClass</tt> is not a superclass of type.</li>
        /// </ul>
        /// <p>
        /// <p>For example, with <tt>class StringList implements List&lt;String&gt;</tt>,
        /// <tt>getExactSuperType(StringList.class, Collection.class)</tt>
        /// returns a {@link ParameterizedType} representing <tt>Collection&lt;String&gt;</tt>.
        /// </p>
        /// </summary>
        /// <param name="type"></param>
        /// <param name="searchClass"></param>
        /// <returns></returns>
        public static Type GetExactSuperType(Type type, Type searchClass)
        {
            return null;
        }


        /// <summary>
        /// Returns the erasure of the given type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static Type Erase(Type type)
        {
            if (!type.IsGenericType)
            {
                return type;
            }
            return type.ReflectedType ?? typeof(object);
        }
    }
}