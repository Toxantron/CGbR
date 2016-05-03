using System.Collections.Generic;

namespace CGbR
{
    /// <summary>
    /// Model representing an enum definition
    /// </summary>
    public class EnumModel : CodeElementModel
    {
        /// <summary>
        /// Initialize a new instance of the enum model
        /// </summary>
        public EnumModel(string name) : base(name)
        {
            Members = new List<EnumMember>();
        }

        /// <summary>
        /// Base type for the members
        /// </summary>
        public ModelValueType BaseType { get; set; }

        /// <summary>
        /// Access modifier of the enum
        /// </summary>
        public AccessModifier AccessModifier { get; set; }

        /// <summary>
        /// Members of the enum
        /// </summary>
        public IList<EnumMember> Members { get; set; }
    }

    /// <summary>
    /// Class representing the 
    /// </summary>
    public class EnumMember : CodeElementModel
    {
        /// <summary>
        /// Create member instance
        /// </summary>
        public EnumMember(string name) : base(name)
        {
        }

        /// <summary>
        /// Value the member represents
        /// </summary>
        public int Value { get; set; }
    }
}