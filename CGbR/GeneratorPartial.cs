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
        /// Create generator partial from generator
        /// </summary>
        public GeneratorPartial(IGenerator generator, string code)
        {
            GeneratorName = generator.Name;
            Usings = generator.Usings;
            Code = code;
        }

        /// <summary>
        /// Name of the generator that created it
        /// </summary>
        public string GeneratorName { get; private set; }

        /// <summary>
        /// Usings the generated code requires
        /// </summary>
        public string[] Usings { get; private set; }

        /// <summary>
        /// Generated code to insert
        /// </summary>
        public string Code { get; private set; }
    }
}
