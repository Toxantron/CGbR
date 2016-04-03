using System;
using System.Linq;

namespace CGbR
{
    /// <summary>
    /// Factory to resolve generators
    /// </summary>
    public static class GeneratorFactory
    {
        private static readonly IGenerator[] Generators = {
            new BinarySerializer(), 
        };

        /// <summary>
        /// Resolve all properties
        /// </summary>
        /// <returns>All available properties</returns>
        public static IGenerator[] ResolveAll()
        {
            return Generators;
        }

        /// <summary>
        /// Resolve generator by name
        /// </summary>
        /// <param name="name">Name of the generator</param>
        /// <returns>Generator instance</returns>
        public static IGenerator Resolve(string name)
        {
            var generator = Generators.FirstOrDefault(g => g.Name == name);

            if (generator == null)
                throw new ArgumentException($"No generator with name '{name}' found!", nameof(name));

            return generator;
        }
    }
}