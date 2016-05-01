using System.Collections.Generic;

namespace CGbR.Configuration
{
    /// <summary>
    /// Configuration for the generator in a certain directory
    /// </summary>
    public class CgbrConfiguration
    {
        /// <summary>
        /// Flag if GGbR is enabled for this project
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Mappings of extension to parser
        /// </summary>
        public ParserMapping[] Mappings { get; set; }

        /// <summary>
        /// Path to extension libraries
        /// </summary>
        public string[] Extensions { get; set; }

        /// <summary>
        /// Local generators
        /// </summary>
        public LocalGeneratorConfig[] LocalGenerators { get; set; }

        /// <summary>
        /// Configuration of global generators
        /// </summary>
        public GlobalGeneratorConfig[] GlobalGenerators { get; set; } 
    }
}