using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Ase.Messaging.Common.Annotation
{
    public abstract class AnnotationUtils
    {
        private AnnotationUtils()
        {
            // utility class
        }

        private static Attribute? GetAnnotation(MemberInfo target, string annotationType) {
            foreach (Attribute annotation in target.GetCustomAttributes()) {
                if (annotationType.Equals(annotation.GetType().Name)) {
                    return annotation;
                }
            }
            return null;
        }

        
        private static void CollectAttributes<T>(T ann, Dictionary<string, object> attributes)
            where T : Attribute
        {
            MethodInfo[] methods = ann.annotationType().getDeclaredMethods();
            foreach (MethodInfo method in methods) {
                if (method.GetParameters().Length == 0 && method.ReturnType != typeof(Void)) {
                    try
                    {
                        object value = method.Invoke(ann);
                        attributes.put(resolveName(method), value);
                    }
                    catch (IllegalAccessException |

                    InvocationTargetException e) {
                        throw new AxonConfigurationException("Error while inspecting annotation values", e);
                    }
                }
            }
        }

        private static string? ResolveName(MethodInfo method)
        {
            if ("value".Equals(method.Name))
            {
                string simpleName = method.DeclaringType!.Name;
                return simpleName
                    .Substring(0, 1)
                    .ToLower(new CultureInfo("en-US", false))
                    .Concat(simpleName.Substring(1))
                    .ToString();
            }

            return method.Name;
        }
    }
}