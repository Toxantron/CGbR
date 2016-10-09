using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CGbR
{
    /// <summary>
    /// Factory to resolve generators
    /// </summary>
    internal static class GeneratorFactory
    {
        private static IGenerator[] _generators;

        /// <summary>
        /// Initialize the factory and load all generators from the app domain
        /// </summary>
        public static void Initialize(IEnumerable<Assembly> assemblies)
        {
            var generators = (from assembly in assemblies
                              from type in assembly.GetExportedTypes()
                              where type.IsClass && typeof(IGenerator).IsAssignableFrom(type)
                              select (IGenerator) Activator.CreateInstance(type));
            _generators = generators.ToArray();
        }

        /// <summary>
        /// Resolve generator by name
        /// </summary>
        /// <param name="name">Name of the generator</param>
        /// <returns>Generator instance</returns>
        public static IGenerator Resolve(string name)
        {
            var generator = _generators.FirstOrDefault(g => g.Name == name);

            if (generator == null)
                throw new ArgumentException($"No generator with name '{name}' found!", nameof(name));

            return generator;
        }
    }
}