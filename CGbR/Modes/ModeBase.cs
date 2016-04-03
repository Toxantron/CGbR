using System.Collections.Generic;

namespace CGbR
{
    /// <summary>
    /// Base class for all modes
    /// </summary>
    internal abstract class ModeBase : IGeneratorMode
    {
        /// <summary>
        /// Parser instance to use
        /// </summary>
        private IParser _parser;

        /// <summary>
        /// All configured generators
        /// </summary>
        protected IGenerator[] Generators { get; private set; }

        /// <summary>
        /// Outer generator template
        /// </summary>
        private readonly ClassSkeleton _skeleton = new ClassSkeleton();

        /// <see cref="IGeneratorMode"/>
        public abstract GeneratorMode Mode { get; }

        /// <see cref="IGeneratorMode"/>
        public void Initialize(IParser parser, IGenerator[] generators)
        {
            _parser = parser;
            Generators = generators;
        }

        /// <see cref="IGeneratorMode"/>
        public abstract void Execute(string path);

        /// <summary>
        /// Parse classes in file at this path
        /// </summary>
        /// <param name="filePath">Path of the file</param>
        /// <returns>Parse file or null</returns>
        protected ParsedFile Parse(string filePath)
        {
            var model = _parser.ParseFile(filePath);
            if (model == null)
                return null;

            return new ParsedFile
            {
                Name = filePath,
                ClassModel = model
            };
        }

        /// <summary>
        /// Generate class code from name, namespace and fragements of code from the different generators
        /// </summary>
        /// <param name="className">Name of the class to generate</param>
        /// <param name="namespace">Namespace the class should be in</param>
        /// <param name="fragments">Fragments of code from the different generators</param>
        /// <returns>Source code of the generated class</returns>
        protected string GenerateClass(string className, string @namespace, GeneratorPartial[] fragments)
        {
            // Initialize new session for the template
            _skeleton.Session = new Dictionary<string, object>
            {
                { "ClassName", className },
                { "Namespace", @namespace },
                { "Fragments", fragments }
            };
            _skeleton.Initialize();

            return _skeleton.TransformText();
        }

        /// <summary>
        /// Representation of a file that was allready parsed
        /// </summary>
        protected class ParsedFile
        {
            /// <summary>
            /// Name of the file
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Classes that were found
            /// </summary>
            public ClassModel ClassModel { get; set; }
        }
    }
}