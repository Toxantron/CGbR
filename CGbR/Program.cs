using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace CGbR
{
    /// <summary>
    /// Application entry point
    /// </summary>
	public class MainClass
    {

        /// <summary>
        /// Directory the generated is executed on
        /// </summary>
        private static string _directory;

        /// <summary>
        /// Outer frame for generated classes
        /// </summary>
        private static ClassSkeleton _skeleton;

        /// <summary>
        /// Project namespace
        /// </summary>
        private static string _namespace;

        /// <summary>
        /// Parse used to extract class models from files
        /// </summary>
        private static IParser _parser;

        /// <summary>
        /// All avaible and configured generators
        /// </summary>
        private static IGenerator[] _generators;

        /// <summary>
        /// Entry method for the application
        /// </summary>
        /// <param name="args"></param>
		public static void Main(string[] args)
        {
            // Only argument is the project directory. Everyting else shall be in the config
            _directory = args[0];
            _skeleton = new ClassSkeleton();

            // Read configuration for this project
            _namespace = "Test";

            // Initialize local strategies
            _parser = new RegexParser();
            _generators = new IGenerator[0];

            // Parse all files in directory recursive
            var files = new List<ParsedFile>();
            ParseFilesInDirectory(_directory, files);

            // Generate all local partials
            GenerateLocalPartials(files);

            // Generate global classes that build on multiple files
            GenerateGlobalClasses(files);
        }

        /// <summary>
        /// Parse all files in a directory and then recursivly go through the sub directories
        /// </summary>
        /// <param name="directory">Directory to search for class definitions</param>
        /// <param name="files">Total list of parsed files</param>
        private static void ParseFilesInDirectory(string directory, ICollection<ParsedFile> files)
        {
            // Parse all files
            foreach (var file in Directory.GetFiles(directory).Where(f => Path.GetExtension(f) == ".cs")
                                                              .Where(f => !f.Contains(".Generated.")))
            {
                var model = _parser.ParseFile(file);
                files.Add(new ParsedFile
                {
                    Name = file,
                    ClassModel = model
                });
            }

            // Dive into subdirectories
            foreach (var subdirectory in Directory.GetDirectories(directory))
            {
                ParseFilesInDirectory(subdirectory, files);
            }
        }

        /// <summary>
        /// Generating partial classes for individual class models
        /// </summary>
        /// <param name="files">Input for the generators</param>
        private static void GenerateLocalPartials(IEnumerable<ParsedFile> files)
        {
            // Iterate over the files and generate the partial class
            foreach (var file in files)
            {
                var model = file.ClassModel;

                // Find all matching generators and collect their code fragments
                var fragments = (from gen in _generators.OfType<ILocalGenerator>()
                                 where gen.CanExtend(model)
                                 select new GeneratorPartial(gen, gen.Extend(model)));

                // Initialize and execute the class skeleton template
                var code = GenerateClass(model.Name, model.Namespace, fragments.ToArray());

                // Write file
                var fileName = Path.GetFileNameWithoutExtension(file.Name);
                fileName = Path.Combine(fileName, ".Generated.cs");
                File.WriteAllText(fileName, code);
            }
        }

        /// <summary>
        /// Generate the global classes
        /// </summary>
        /// <param name="files"></param>
        private static void GenerateGlobalClasses(IEnumerable<ParsedFile> files)
        {
            var models = files.Select(f => f.ClassModel).ToArray();
            // Determine all global classes and generate their code
            var globalClasses = (from generator in _generators.OfType<IGlobalGenerator>()
                                 let matchingClasses = models.Where(generator.ClassFilter).ToList()
                                 where matchingClasses.Count > 0
                                 let globalPartial = new GeneratorPartial(generator, generator.Extend(matchingClasses))
                                 group globalPartial by generator.ClassName into globalClass
                                 select globalClass);

            // Generate code for all global classes
            foreach (var globalClass in globalClasses)
            {
                var className = globalClass.Key;

                // Execute code generator
                var code = GenerateClass(className, _namespace, globalClass.ToArray());
                
                // Write to file on root level
                var fileName = Path.Combine(_directory, className, ".Generated.cs");
            }
        }

        /// <summary>
        /// Generate class code from name, namespace and fragements of code from the different generators
        /// </summary>
        /// <param name="className">Name of the class to generate</param>
        /// <param name="namespace">Namespace the class should be in</param>
        /// <param name="fragments">Fragments of code from the different generators</param>
        /// <returns>Source code of the generated class</returns>
        private static string GenerateClass(string className, string @namespace, GeneratorPartial[] fragments)
        {
            // Initialize new session for the template
            _skeleton.Session = new Dictionary<string, object>
            {
                { "ClassName", className },
                { "Namespace", @namespace },
                { "Fragments", fragments }
            };

            return _skeleton.TransformText();
        }

        /// <summary>
        /// Representation of a file that was allready parsed
        /// </summary>
        private class ParsedFile
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
