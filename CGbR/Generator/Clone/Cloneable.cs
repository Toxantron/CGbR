using System;
using System.Collections.Generic;
using System.Linq;

namespace CGbR
{
    /// <summary>
    /// Local generator that generates clone method
    /// </summary>
    internal class Cloneable : ILocalGenerator
    {
        /// <summary>
        /// Name of the generator
        /// </summary>
        public string Name { get; } = nameof(Cloneable);

        /// <summary>
        /// Using the code requires
        /// </summary>
        public string[] Usings { get; } = {
            "System"
        };

        /// <summary>
        /// Interfaces this generates
        /// </summary>
        public string[] Interfaces { get; } = {};

        /// <summary>
        /// CHeck if the generator is meant for this model
        /// </summary>
        public bool CanExtend(ClassModel model)
        {
            return model.Interfaces.Any(i => i == nameof(ICloneable));
        }

        /// <summary>
        /// Generated extension code
        /// </summary>
        public string Extend(ClassModel model)
        {
            var template = new CloneGenerator();
            template.Session = new Dictionary<string, object>()
            {
                ["Model"] = model
            };
            template.Initialize();
            return template.TransformText();
        }
    }
}