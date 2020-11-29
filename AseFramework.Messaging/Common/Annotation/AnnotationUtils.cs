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

        public static bool IsAnnotationPresent<T>(MemberInfo element, T annotationType)
            where T : Attribute
        {
            return IsAnnotationPresent(element, annotationType.GetType().Name);
        }
        
        public static bool IsAnnotationPresent(MemberInfo element, string annotationType) {
            return FindAnnotationAttributes(element, annotationType).isPresent();
        }

        public static IDictionary<string, object> FindAnnotationAttributes(MemberInfo element, string annotationName) {
            IDictionary<string, object> attributes = new Dictionary<string, object>();
            Attribute? ann = GetAnnotation(element, annotationName);
            bool found = false;
            if (ann != null) {
                CollectAttributes(ann, attributes);
                found = true;
            } else {
                HashSet<String> visited = new HashSet<>();
                for (Annotation metaAnn : element.getAnnotations()) {
                    if (collectAnnotationAttributes(metaAnn.annotationType(), annotationName, visited, attributes)) {
                        found = true;
                        collectAttributes(metaAnn, attributes);
                    }
                }
            }
            return found ? Optional.of(attributes) : Optional.empty();
        }
        
        private static Attribute? GetAnnotation(MemberInfo target, string annotationType)
        {
            foreach (Attribute annotation in target.GetCustomAttributes())
            {
                if (annotationType.Equals(annotation.GetType().Name))
                {
                    return annotation;
                }
            }

            return null;
        }


        private static void CollectAttributes<T>(T ann, Dictionary<string, object> attributes)
            where T : Attribute
        {
            ann.
            ann.
                MemberInfo[] methods = ann.NameGetType().GetDefaultMembers();
            foreach (var memberInfo in methods.Where(method => method is MethodInfo).ToList())
            {
                var method = (MethodInfo) memberInfo;
                if (method.GetParameters().Length == 0 && method.ReturnType != typeof(Void))
                {
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