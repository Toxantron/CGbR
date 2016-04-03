namespace CGbR
{
    /// <summary>
    /// Type representation of payload fields
    /// </summary>
    public enum ValueType
    {
        /// <summary>
        /// Single character
        /// </summary>
        [CodeRepresentation("char", "Char")]
        Char,
        /// <summary>
        /// Single byte interpreted as boolean
        /// </summary>
        [CodeRepresentation("bool", "Boolean")]
        Boolean,
        /// <summary>
        /// Single byte
        /// </summary>
        [CodeRepresentation("byte", "Byte")]
        Byte,
        /// <summary>
        /// Signed 16-bit integer
        /// </summary>
        [CodeRepresentation("short", "Int16")]
        Int16,
        /// <summary>
        /// Unsigned 16-bit integer
        /// </summary>
        [CodeRepresentation("ushort", "UInt16")]
        UInt16,
        /// <summary>
        /// Signed 32-bit integer
        /// </summary>
        [CodeRepresentation("int", "Int32")]
        Int32,
        /// <summary>
        /// Unsigned 64-bit integer
        /// </summary>
        [CodeRepresentation("uint", "UInt32")]
        UInt32,
        /// <summary>
        /// 32-bit floating point field
        /// </summary>
        [CodeRepresentation("float", "Single")]
        Single,
        /// <summary>
        /// Signed Double
        /// </summary>
        [CodeRepresentation("double", "Double")]
        Double,
        /// <summary>
        /// Signed 64-bit integer
        /// </summary>
        [CodeRepresentation("long", "Int64")]
        Int64,
        /// <summary>
        /// Signed 64-bit integer
        /// </summary>
        [CodeRepresentation("ulong", "UInt64")]
        UInt64,
        /// <summary>
        /// ASCII or UTF8 encoded string
        /// </summary>
        [CodeRepresentation("string", "String")]
        String,
        /// <summary>
        /// Complex sub type
        /// </summary>
        Class,
    }
}