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
		public static void Main (string[] args)
		{
            // Only argument is the project directory. Everyting else shall be in the config
            _directory = args[0];

            // Read configuration for this project

            // Initialize local strategies
            _parser = null;
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
            foreach (var file in Directory.GetFiles(directory))
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
                                 select new GeneratorPartial
                                 {
                                     GeneratorName = gen.Name,
                                     Usings = gen.Usings,
                                     Code = gen.Extend(model)
                                 }).ToArray();
                
                // Initialize and execute the class skeleton template
                var template = new ClassSkeleton();
                template.Session = new Dictionary<string, object>
                {
                    { "ClassName", model.Name },
                    { "Namespace", model.Namespace },
                    { "Fragments", fragments }
                };
                var code = template.TransformText();

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
