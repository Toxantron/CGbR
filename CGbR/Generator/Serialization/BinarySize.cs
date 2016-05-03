using System;
using System.Linq;
using System.Runtime.Serialization;

namespace CGbR
{
    /// <summary>
    /// Helper to determine binary size of properties and classes
    /// </summary>
    internal static class BinarySize
    {
        /// <summary>
        /// Get the binary size of a property
        /// </summary>
        /// <param name="model">Property model</param>
        /// <returns>Size of the propety in bytes</returns>
        public static int OfProperty(PropertyModel model)
        {
            return OfType(model.ValueType);
        }

        /// <summary>
        /// Get the binary size of a given value type
        /// </summary>
        public static int OfType(ModelValueType type)
        {
            switch (type)
            {
                case ModelValueType.Boolean:
                case ModelValueType.Byte:
                    return 1;
                case ModelValueType.Char:
                case ModelValueType.Int16:
                case ModelValueType.UInt16:
                    return 2;
                case ModelValueType.Int32:
                case ModelValueType.UInt32:
                case ModelValueType.Single:
                    return 4;
                case ModelValueType.Double:
                case ModelValueType.Int64:
                case ModelValueType.UInt64:
                    return 8;
                case ModelValueType.String:
                case ModelValueType.Class:
                    return 0;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Calculate the binary size of a class instance
        /// </summary>
        /// <param name="model">Class model</param>
        /// <param name="tools">Tools strategy to handle classes</param>
        /// <returns>Fixed size of an instance</returns>
        public static int OfClass(ClassModel model, IClassSerializationTools tools)
        {
            var sum = 0;
            foreach (var property in model.Properties.Where(p => p.HasAttribute(nameof(DataMemberAttribute))))
            {
                if (property.IsCollection)
                    // Include 2 bytes length information for each dimension of flexible collections
                    sum += 2 * property.Dimensions;
                else if (property.ValueType == ModelValueType.String)
                    // 2 bytes length for strings as well
                    sum += 2;
                else if (property.ValueType == ModelValueType.Class)
                    // Check if the class reference declares a fixed size
                    sum += tools.FixedSize(model, property);
                else
                    sum += OfProperty(property);
            }
            return sum;
        }

        /// <summary>
        /// Check if the binary size of a property is fixed or variable
        /// </summary>
        /// <param name="property">Property that shall be checked</param>
        /// <returns>True if property is of variable size</returns>
        public static bool IsVariable(PropertyModel property)
        {
            // All collection and strings are variable
            if (property.IsCollection || property.ValueType == ModelValueType.String)
                return true;

            // Same applies for classes
            if (property.ValueType == ModelValueType.Class)
                return true;

            // Everything else is static
            return false;
        }

        /// <summary>
        /// Check if the binary size of a class is fixed or variable
        /// </summary>
        /// <param name="model">Class that shall be checked</param>
        /// <returns>True if class is of variable size</returns>
        public static bool IsVariable(ClassModel model)
        {
            return model.Properties.Any(IsVariable);
        }
    }
}