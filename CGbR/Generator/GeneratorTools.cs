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
    }
}