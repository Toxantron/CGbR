namespace CGbR
{
    /// <summary>
    /// Type representation of property types
    /// </summary>
    public enum ModelValueType
    {
        /// <summary>
        /// Single character
        /// </summary>
        [TypeAlias("char", "Char")]
        Char,
        /// <summary>
        /// Single byte interpreted as boolean
        /// </summary>
        [TypeAlias("bool", "Boolean")]
        Boolean,
        /// <summary>
        /// Single byte
        /// </summary>
        [TypeAlias("byte", "Byte")]
        Byte,
        /// <summary>
        /// Signed 16-bit integer
        /// </summary>
        [TypeAlias("short", "Int16")]
        Int16,
        /// <summary>
        /// Unsigned 16-bit integer
        /// </summary>
        [TypeAlias("ushort", "UInt16")]
        UInt16,
        /// <summary>
        /// Signed 32-bit integer
        /// </summary>
        [TypeAlias("int", "Int32")]
        Int32,
        /// <summary>
        /// Unsigned 64-bit integer
        /// </summary>
        [TypeAlias("uint", "UInt32")]
        UInt32,
        /// <summary>
        /// 32-bit floating point field
        /// </summary>
        [TypeAlias("float", "Single")]
        Single,
        /// <summary>
        /// Signed Double
        /// </summary>
        [TypeAlias("double", "Double")]
        Double,
        /// <summary>
        /// Signed 64-bit integer
        /// </summary>
        [TypeAlias("long", "Int64")]
        Int64,
        /// <summary>
        /// Signed 64-bit integer
        /// </summary>
        [TypeAlias("ulong", "UInt64")]
        UInt64,
        /// <summary>
        /// ASCII or UTF8 encoded string
        /// </summary>
        [TypeAlias("string", "String")]
        String,
        /// <summary>
        /// Complex sub type
        /// </summary>
        Class,
    }
}