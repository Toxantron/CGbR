using System;
using System.Collections.Generic;
using System.Linq;

namespace CGbR
{
    /// <summary>
    /// Tools of the different generators
    /// </summary>
    internal static class GeneratorTools
    {
        /// <summary>
        /// Check if an attribute is defined on an element
        /// </summary>
        /// <param name="element">Code element to check</param>
        /// <param name="name">Name of the attribute</param>
        /// <returns>True if attribute is set</returns>
        public static bool HasAttribute(this CodeElementModel element, string name)
        {
            return element.Attributes.Any(att => AttributeCompare(att, name));
        }

        /// <summary>
        /// Filter collection by attribute
        /// </summary>
        /// <param name="elements">Code elements to filter</param>
        /// <param name="name">Name of the attribute</param>
        /// <returns>True if attribute is set</returns>
        public static IEnumerable<T> WhereAttribute<T>(this IEnumerable<T> elements, string name)
            where T : CodeElementModel
        {
            return elements.Where(e => e.HasAttribute(name));
        }

        /// <summary>
        /// Get attribute with this name or null
        /// </summary>
        public static AttributeModel GetAttributeOrNull(this CodeElementModel element, string attributeName)
        {
            return element.Attributes.FirstOrDefault(att => AttributeCompare(att, attributeName));
        }

        /// <summary>
        /// Compare different attributes
        /// </summary>
        private static bool AttributeCompare(AttributeModel attribute, string searchName)
        {
            const string suffix = nameof(Attribute);
            return attribute.Name.Replace(suffix, string.Empty) == searchName.Replace(suffix, string.Empty);
        }

        /// <summary>
        /// Get reference with given name
        /// </summary>
        public static CodeElementModel GetReference(this ClassModel model, string name)
        {
            return model.References.First(r => r.Name == name);
        }

        /// <summary>
        /// Generate code that creates collection instance
        /// </summary>
        /// <param name="property">Property to generate</param>
        /// <param name="length">Optional length of the collection</param>
        /// <returns>Collection constructor string</returns>
        public static string CollectionConstructor(PropertyModel property, string length = null)
        {
            if (property.CollectionType == "Array")
            {
                return $"new {property.ElementType}[{length ?? "0"}]";
            }
            if (property.CollectionType == "List" || property.CollectionType.StartsWith("I"))
            {
                return $"new List<{property.ElementType}>({length ?? string.Empty})";
            }

            return $"new {property.CollectionType}<{property.ElementType}>()";
        }

        /// <summary>
        /// Linq extension to transform IEnumerable to target type
        /// </summary>
        /// <param name="property">Property to get extension for</param>
        /// <returns></returns>
        public static string ToCollection(PropertyModel property)
        {
            switch (property.CollectionType)
            {
                case "Array":
                    return ".ToArray()";
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Code fragment that returns size of the collection
        /// </summary>
        /// <param name="property">Collection property</param>
        /// <param name="dimension">Optional dimension parameter</param>
        /// <returns>Code fragment</returns>
        public static string CollectionSize(PropertyModel property, int dimension = -1)
        {
            if (!property.IsCollection && property.ValueType == ModelValueType.String)
                return "Length";

            switch (property.CollectionType)
            {
                case "Array":
                    return dimension < 0 ? "Length" : $"GetLength({dimension})";
                case "ICollection":
                case "IList":
                case "List":
                    return "Count";
                default:
                    return "Count()";
            }
        }

        /// <summary>
        /// Check of this collection supports for-loop and indexed acces
        /// </summary>
        /// <param name="property">Collection that should be checked</param>
        /// <returns>True if for loop is supported.Otherwise false</returns>
        public static bool SupportsForLoop(PropertyModel property)
        {
            switch (property.CollectionType)
            {
                case "List":
                case "IList":
                case "Array":
                    return true;
                default:
                    return false;
            }
        }
    }
}