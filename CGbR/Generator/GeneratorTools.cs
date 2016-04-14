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
            var shortName = name.Replace("Attribute", string.Empty);
            return element.HasAttributeShort(shortName);
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
            var shortName = name.Replace("Attribute", string.Empty);
            return elements.Where(e => e.HasAttributeShort(shortName));
        }

        /// <summary>
        /// Checks if an element has an attribute with given short name
        /// </summary>
        /// <param name="element">Code element to check</param>
        /// <param name="shortName">Name of the attribute</param>
        /// <returns>True if attribute is set</returns>
        private static bool HasAttributeShort(this CodeElementModel element, string shortName)
        {
            return element.Attributes.Any(att => att.Name.Replace("Attribute", string.Empty) == shortName);
        }


        /// <summary>
        /// Generate code that creates collection instance
        /// </summary>
        /// <param name="property">Property to generate</param>
        /// <param name="length">Optional length of the collection</param>
        /// <returns>Collection construtor string</returns>
        public static string CollectionConstructor(PropertyModel property, string length = "0")
        {
            switch (property.CollectionType)
            {
                case "Array":
                    return $"new {property.ElementType}[{length}]";
                default:
                    return $"new List<{property.ElementType}>({length})";
            }
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
        /// Code fragement that returns size of the collection
        /// </summary>
        /// <param name="property">Collection property</param>
        /// <param name="dimension">Optional dimension parameter</param>
        /// <returns>Code fragement</returns>
        public static string CollectionSize(PropertyModel property, int dimension = -1)
        {
            if (property.ValueType == ValueType.String)
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
                case "ICollection":
                case "Array":
                    return true;
                default:
                    return false;
            }
        }
    }
}