using System;

namespace CGbR
{
    /// <summary>
    /// Attribute used to decorate enum members with possible code fragements that represent them
    /// </summary>
    internal class CodeRepresentationAttribute : Attribute
    {
        /// <summary>
        /// Code fragements that represent this enum
        /// </summary>
        public string[] Representations { get; private set; }

        /// <summary>
        /// Initialize a new instance of the attribute
        /// </summary>
        /// <param name="representations"></param>
        public CodeRepresentationAttribute(params string[] representations)
        {
            Representations = representations;
        }
    }
}