using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public static bool IsAnnotationPresent<T>(MemberInfo element)
            where T : Attribute
        {
            return IsAnnotationPresent(element, typeof(T).Name);
        }

        public static bool IsAnnotationPresent(MemberInfo element, string annotationType)
        {
            return FindAnnotationAttributes(element, annotationType)!.Count > 0;
        }

        public static IDictionary<string, object?>? FindAnnotationAttributes(MemberInfo element, string annotationName)
        {
            IDictionary<string, object?> attributes = new Dictionary<string, object?>();
            CustomAttributeData? attributeData = GetAnnotation(element, annotationName);
            bool found = false;
            if (attributeData != null)
            {
                CollectAttributes(attributeData, attributes);
                found = true;
            }
            else
            {
                HashSet<string> visited = new HashSet<string>();
                foreach (CustomAttributeData customAttributeData in CustomAttributeData.GetCustomAttributes(element))
                {
                    if (CollectAnnotationAttributes(customAttributeData.AttributeType, annotationName, visited,
                        attributes))
                    {
                        found = true;
                        CollectAttributes(customAttributeData, attributes);
                    }
                }
            }

            return found ? attributes : null;
        }


        public static IDictionary<string, object?>? FindAnnotationAttributes<T>(MemberInfo element)
            where T : Attribute
        {
            return FindAnnotationAttributes(element, typeof(T).Name);
        }

        private static bool CollectAnnotationAttributes(MemberInfo target, string annotationType,
            HashSet<string> visited, IDictionary<string, object?> attributes)
        {
            CustomAttributeData? ann = GetAnnotation(target, annotationType);
            if (ann == null && visited.Add(target.Name))
            {
                foreach (CustomAttributeData metaAnn in CustomAttributeData.GetCustomAttributes(target))
                {
                    if (!CollectAnnotationAttributes(metaAnn.AttributeType, annotationType, visited, attributes))
                        continue;
                    CollectAttributes(metaAnn, attributes);
                    return true;
                }
            }
            else if (ann != null)
            {
                CollectAttributes(ann, attributes);
                return true;
            }

            return false;
        }

        private static CustomAttributeData? GetAnnotation(MemberInfo target, string annotationType)
        {
            foreach (CustomAttributeData attributeData in CustomAttributeData.GetCustomAttributes(target))
            {
                if (annotationType.Equals(attributeData.GetType().Name))
                {
                    return attributeData;
                }
            }

            return null;
        }


        private static void CollectAttributes<T>(T attributeData, IDictionary<string, object?> attributes)
            where T : CustomAttributeData
        {
            var customAttributeTypedArguments =
                attributeData.ConstructorArguments.Concat(
                    attributeData.NamedArguments
                        .Select(argument => argument.TypedValue)
                        .ToList()
                );

            foreach (CustomAttributeTypedArgument customAttributeTypedArgument in customAttributeTypedArguments)
            {
                if (customAttributeTypedArgument.Value?.GetType() ==
                    typeof(ReadOnlyCollection<CustomAttributeTypedArgument>))
                {
                    foreach (CustomAttributeTypedArgument customAttribute in
                        (ReadOnlyCollection<CustomAttributeTypedArgument>) customAttributeTypedArgument.Value)
                    {
                        attributes.Add(customAttribute.ArgumentType.Name, customAttribute.Value);
                    }
                }
                else
                {
                    attributes.Add(customAttributeTypedArgument.ArgumentType.Name, customAttributeTypedArgument.Value);
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