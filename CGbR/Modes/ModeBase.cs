using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using static System.Reflection.Assembly;

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
        protected IDictionary<string, IParser> Parsers { get; } = new Dictionary<string, IParser>();

        /// <summary>
        /// All configured generators
        /// </summary>
        protected IList<IGenerator> Generators { get; } = new List<IGenerator>();

        /// <see cref="IGeneratorMode"/>
        public abstract bool Initialize(string path, string[] args);


        /// <summary>
        /// Resolve assemblies from path including our own assembly
        /// </summary>
        protected static IEnumerable<Assembly> ResolveAssemblies(IEnumerable<string> paths)
        {
            var assembly = GetExecutingAssembly();
            yield return assembly;

            foreach (var path in paths)
            {
                try
                {
                    var resolved = Environment.ExpandEnvironmentVariables(path);
                    assembly = LoadFile(resolved);
                }
                catch (Exception)
                {
                    Console.WriteLine($"Failed to load assembly from path: {path}");
                }
                yield return assembly;
            }
        }

        /// <see cref="IGeneratorMode"/>
        public abstract void Execute();

        /// <summary>
        /// Parse classes in file at this path
        /// </summary>
        /// <param name="filePath">Path of the file</param>
        /// <returns>Parse file or null</returns>
        protected ParsedFile Parse(string filePath)
        {
            var ext = Path.GetExtension(filePath);

            var model = Parsers[ext].ParseFile(filePath);
            if (model == null)
                return null;

            return new ParsedFile
            {
                Name = filePath,
                Model = model
            };
        }

        /// <summary>
        /// Generate local partial of a file
        /// </summary>
        /// <param name="file">File to generate for</param>
        protected void GenerateLocalPartial(ParsedFile file)
        {
            var model = file.Model as ClassModel;
            if (model == null)
                return;

            // Find all matching generators and collect their code fragments
            var fragments = (from gen in Generators.OfType<ILocalGenerator>()
                             where gen.CanExtend(model)
                             select new GeneratorPartial(gen, gen.Extend(model)));

            // Initialize and execute the class skeleton template
            var code = GenerateClass(model.Name, model.AccessModifier, model.Namespace, fragments.ToArray());

            // Write file
            var fileName = Path.GetFileNameWithoutExtension(file.Name) + ".Generated.cs";
            fileName = Path.Combine(Path.GetDirectoryName(file.Name), fileName);
            File.WriteAllText(fileName, code);
        }

        /// <summary>
        /// Generate class code from name, namespace and fragements of code from the different generators
        /// </summary>
        /// <param name="className">Name of the class to generate</param>
        /// <param name="modifier">Modifier of the class</param>
        /// <param name="namespace">Namespace the class should be in</param>
        /// <param name="fragments">Fragments of code from the different generators</param>
        /// <returns>Source code of the generated class</returns>
        protected string GenerateClass(string className, AccessModifier modifier, string @namespace, GeneratorPartial[] fragments)
        {
            // Initialize new session for the template
            var skeleton = new ClassSkeleton();
            skeleton.Session = new Dictionary<string, object>
            {
                { "ClassName", className },
                { "Modifier", modifier },
                { "Namespace", @namespace },
                { "BaseClasses", string.Join(", ", fragments.SelectMany(f => f.Interfaces).Distinct()) },
                { "Fragments", fragments }
            };
            skeleton.Initialize();

            return skeleton.TransformText();
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
            public CodeElementModel Model { get; set; }

            /// <summary>
            /// String override for better debugging
            /// </summary>
            public override string ToString() => $"{Model.Namespace} at {Name}";
        }
    }
}