using System;

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
            switch (model.ValueType)
            {
                case ValueType.Char:
                case ValueType.Boolean:
                case ValueType.Byte:
                    return 1;
                case ValueType.Int16:
                case ValueType.UInt16:
                    return 2;
                case ValueType.Int32:
                case ValueType.UInt32:
                case ValueType.Single:
                    return 4;
                case ValueType.Double:
                case ValueType.Int64:
                case ValueType.UInt64:
                    return 8;
                case ValueType.String:
                case ValueType.Class:
                    return 0;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Calculate the binary size of a class instance
        /// </summary>
        /// <param name="model">Class model</param>
        /// <returns>Fixed size of an instance</returns>
        public static int OfClass(ClassModel model)
        {
            var sum = 0;
            foreach (var property in model.Properties)
            {
                if (property.IsCollection)
                    // Include 2 bytes length information for each dimension of flexible collections
                    sum += 2 * property.Dimensions;
                else if (property.ValueType == ValueType.String)
                    // 2 bytes length for strings as well
                    sum += 2;
                else
                    sum += OfProperty(property);
            }
            return sum;
        }
    }
}