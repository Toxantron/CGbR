using System;
using System.Collections.Generic;

namespace CGbR
{
    /// <summary>
    /// Generators that create code targeting multiple classes and are therefore project global
    /// </summary>
    public interface IGlobalGenerator : IGenerator
    {
        /// <summary>
        /// Name of the generated global class
        /// </summary>
        string ClassName { get; }

        /// <summary>
        /// Filter to find classes that can be extended by this generator
        /// </summary>
        Func<ClassModel, bool> ClassFilter { get; }

        /// <summary>
        /// Generate code to extend given classes
        /// </summary>
        /// <param name="models">Models matching the filter</param>
        /// <returns>Code partial</returns>
        string Extend(IEnumerable<ClassModel> models);
    }
}