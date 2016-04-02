using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGbR
{
    /// <summary>
    /// Class representing a partial fragment of generated code
    /// </summary>
    internal class GeneratorPartial
    {
        /// <summary>
        /// Name of the generator that created it
        /// </summary>
        public string GeneratorName { get; set; }

        /// <summary>
        /// Usings the generated code requires
        /// </summary>
        public string[] Usings { get; set; }

        /// <summary>
        /// Generated code to insert
        /// </summary>
        public string Code { get; set; }
    }
}
