using System;

namespace CGbR
{
    /// <summary>
    /// Attribute used to decorate enum members with possible type aliases that represent them
    /// </summary>
    internal class TypeAliasAttribute : Attribute
    {
        /// <summary>
        /// Code fragements that represent this enum
        /// </summary>
        public string[] Aliases { get; private set; }

        /// <summary>
        /// Initialize a new instance of the attribute
        /// </summary>
        /// <param name="aliases"></param>
        public TypeAliasAttribute(params string[] aliases)
        {
            Aliases = aliases;
        }
    }
}