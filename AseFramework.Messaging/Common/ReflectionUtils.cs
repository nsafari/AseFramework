using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ase.Messaging.Common
{
    public abstract class ReflectionUtils
    {
        private ReflectionUtils()
        {
            // utility class
        }

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
        public static Type? GetExactSuperType(Type type, Type searchClass)
        {
            if (type.IsGenericType || type.IsClass || type.HasElementType)
            {
                Type? clazz = Erase(type);

                if (searchClass == clazz)
                {
                    return type;
                }

                if (!searchClass.IsAssignableFrom(clazz))
                {
                    return null;
                }
            }

            IEnumerable<Type> exactDirectSuperTypes = GetExactDirectSuperTypes(type);
            foreach (Type superType in exactDirectSuperTypes)
            {
                Type? result = GetExactSuperType(superType, searchClass);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        /// <summary>
        /// Unwrap the given {@code type} if is wrapped by any of the given {@code wrapperTypes}. This method assumes that
        /// the {@code wrapperTypes} have a single generic argument, which identifies the type they wrap.
        /// <p/>
        /// For example, if invoked with {@code Future.class} and {@code Optional.class} as {@code wrapperTypes}:
        /// <ul>
        /// <li> {@code Task<String>} resolves to {@code String}</li>
        /// <li> {@code Task<List<String>>} resolves to {@code List<String>}</li>
        /// </ul>
        /// </summary>
        /// <param name="type">The type to unwrap, if wrapped</param>
        /// <param name="wrapperTypes">The wrapper types to unwrap</param>
        /// <returns>the unwrapped Type, or the original if it wasn't wrapped in any of the given wrapper types</returns>
        public static Type UnwrapIfType(Type type, params Type[] wrapperTypes)
        {
            foreach (Type wrapperType in wrapperTypes)
            {
                Type? wrapper = ReflectionUtils.GetExactSuperType(type, wrapperType);

                if (wrapper != null && wrapper.IsGenericType)
                {
                    Type[] actualTypeArguments = wrapper.GetGenericArguments();
                    if (actualTypeArguments.Length == 1)
                    {
                        return UnwrapIfType(actualTypeArguments[0], wrapperTypes);
                    }
                }
                else if (wrapperType == type)
                {
                    // the wrapper type doesn't declare what it wraps. In that case we just know it's an Object
                    return typeof(object);
                }
            }

            return type;
        }

        /// <summary>
        /// Returns the direct supertypes of the given type. Resolves type parameters.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private static Type[] GetExactDirectSuperTypes(Type? type)
        {
            if (type == null)
            {
                throw new ArgumentException("Cannot handle given Type of null");
            }

            if (type.IsGenericType)
            {
                return GetExactDirectSuperTypesOfParameterizedTypeOrClass(type);
            }
            else if (type.IsGenericTypeParameter)
            {
                return type.GetGenericParameterConstraints();
            }
            else if (type.HasElementType)
            {
                return GetExactDirectSuperTypes(type.GetElementType());
            }

            // logger.debug(
            //     type.getClass() + " is not supported for retrieving the exact direct super types from. Will by "
            //                     + "default return the type contained in an Type[]"
            // );
            return new Type[] {type};
        }

        private static Type[] GetExactDirectSuperTypesOfParameterizedTypeOrClass(Type type)
        {
            Type? clazz;
            Type[] result;
            int resultIndex;
            if (type.IsGenericType)
            {
                clazz = type.BaseType;
            }
            else
            {
                clazz = type;
                if (clazz.IsArray)
                {
                    Type? typeComponent = clazz.GetElementType();
                    Type[] componentSupertypes = GetExactDirectSuperTypes(typeComponent);
                    // TODO: what does 3 mean?
                    result = new Type[componentSupertypes.Length + 3];
                    for (resultIndex = 0; resultIndex < componentSupertypes.Length; resultIndex++)
                    {
                        result[resultIndex] = componentSupertypes[resultIndex].MakeArrayType();
                    }

                    return result;
                }
            }

            Type[]? superInterfaces = clazz?.GetInterfaces();
            Type? superClass = clazz?.BaseType;

            // The only supertype of an interface without superinterfaces is Object
            if (superClass == null && superInterfaces?.Length == 0 && (clazz?.IsInterface ?? false))
            {
                return new Type[] {typeof(object)};
            }

            if (superClass == null)
            {
                result = new Type[superInterfaces?.Length ?? 0];
                resultIndex = 0;
            }
            else
            {
                result = new Type[(superInterfaces?.Length ?? 0) + 1];
                resultIndex = 1;
                result[0] = MapTypeParameters(superClass, type);
            }

            if (superInterfaces == null)
            {
                return result;
            }

            foreach (Type superInterface in superInterfaces)
            {
                result[resultIndex++] = MapTypeParameters(superInterface, type);
            }

            return result;
        }

        /// <summary>
        /// Maps type parameters in a type to their values.
        /// </summary>
        /// <param name="toMapType">Type possibly containing type arguments</param>
        /// <param name="typeAndParams">must be either ParameterizedType, or (in case there are no type arguments, or it's a raw type) Class</param>
        /// <returns>toMapType, but with type parameters from typeAndParams replaced.</returns>
        private static Type MapTypeParameters(Type toMapType, Type typeAndParams)
        {
            return toMapType;
        }

        /// <summary>
        /// Checks if the given type is a class that is supposed to have type parameters, but doesn't.
        /// In other words, if it's a really raw type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static bool IsMissingTypeParameters(Type type)
        {
            for (Type? clazz = type; clazz != null; clazz = clazz.IsNested ? clazz.DeclaringType : null)
            {
                if (clazz.GetGenericArguments().Length != 0)
                {
                    return true;
                }
            }

            if (type.IsGenericType)
            {
                return false;
            }
// Log.lo
//             logger.debug(
//                 type.getClass() + " is not supported for checking if there are missing type parameters. "
//                                 + "Will by default return false."
//             );

            return false;
        }

        /// <summary>
        /// Returns the erasure of the given type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static Type? Erase(Type? type)
        {
            if (type == null)
            {
                return typeof(object);
            }

            if (type.IsGenericType)
            {
                var typeBaseType = type.BaseType;
                return typeBaseType == typeof(object) ? type.GetInterfaces()[0] : typeBaseType;
            }

            if (type.IsGenericParameter)
            {
                return type.GetGenericParameterConstraints().Length == 0
                    ? typeof(object)
                    : Erase(type.GetGenericParameterConstraints()[0]);
            }

            return type.HasElementType ? Erase(type.GetElementType())?.MakeArrayType() : typeof(object);

            // logger.debug(type.getClass() + " is not supported for type erasure. Will by default return Object.");
        }

       
        public static bool IsAccessible(MethodBase member)
        {
            return IsNonFinalPublicMember(member);
        }

        public static bool IsNonFinalPublicMember(MethodBase member)
        {
            return member.DeclaringType is { } && 
                   member.DeclaringType.IsPublic && !member.IsFinal;
        }
    }
}