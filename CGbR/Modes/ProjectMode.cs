using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CGbR.Configuration;
using Newtonsoft.Json;

namespace CGbR
{
    /// <summary>
    /// Usage mode that operates on an entire project
    /// </summary>
    internal class ProjectMode : ModeBase
    {
        /// <summary>
        /// Directory the generated is executed on
        /// </summary>
        private string _directory;

        /// <summary>
        /// Root namespace of the project
        /// </summary>
        private string _namespace;

        /// <see cref="IGeneratorMode"/>
        public override GeneratorMode Mode { get; } = GeneratorMode.Project;

        /// <see cref="IGeneratorMode"/>
        public override bool Initialize(string path, string[] args)
        {
            _directory = path;

            // Look for the config
            var configPath = Path.Combine(_directory, "cgbr.json");
            if (!File.Exists(configPath))
            {
                Console.WriteLine("Project mode requires a config.");
                return false;
            }

            // Read and parse config
            var configText = File.ReadAllText(configPath);
            var config = JsonConvert.DeserializeObject<CgbrConfiguration>(configText);

            // Parser mapppings
            foreach (var mapping in config.Mappings)
            {
                Parsers[mapping.Extension] = ParserFactory.Resolve(mapping.Parser);
            }

            // Generators
            foreach (var localGenerator in config.LocalGenerators.Where(gen => gen.IsEnabled))
            {
                Generators.Add(GeneratorFactory.Resolve(localGenerator.Name));
            }
            foreach (var globalGenerator in config.GlobalGenerators.Where(gen => gen.IsEnabled))
            {
                Generators.Add(GeneratorFactory.Resolve(globalGenerator.Name));
            }

            return true;
        }

        /// <see cref="IGeneratorMode"/>
        public override void Execute()
        {
            _namespace = "Test";

            // Parse all files in directory recursive
            var files = new List<ParsedFile>();
            ParseFilesInDirectory(_directory, files);

            // Link classes of this project
            LinkReferences(files);

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
        private void ParseFilesInDirectory(string directory, ICollection<ParsedFile> files)
        {
            // Parse all files
            foreach (var file in Directory.GetFiles(directory).Where(f => Path.GetExtension(f) == ".cs")
                                                              .Where(f => !f.Contains(".Generated.")))
            {
                var parsed = Parse(file);
                if (parsed != null)
                    files.Add(parsed);
            }

            // Dive into subdirectories
            foreach (var subdirectory in Directory.GetDirectories(directory))
            {
                ParseFilesInDirectory(subdirectory, files);
            }
        }

        /// <summary>
        /// Link class references
        /// </summary>
        /// <param name="files">Parsed files</param>
        private static void LinkReferences(ICollection<ParsedFile> files)
        {
            foreach (var file in files)
            {
                // Find references classes in list of all classes
                var references = (from model in files.Select(f => f.ClassModel)
                                  from reference in file.ClassModel.References
                                  where reference.Name == model.Name
                                  select model);

                // Overwrite flat references with real instances
                file.ClassModel.References = references.ToList();
            }
        }

        /// <summary>
        /// Generating partial classes for individual class models
        /// </summary>
        /// <param name="files">Input for the generators</param>
        private void GenerateLocalPartials(IEnumerable<ParsedFile> files)
        {
            // Iterate over the files and generate the partial class
            foreach (var file in files)
            {
                var model = file.ClassModel;

                // Find all matching generators and collect their code fragments
                var fragments = (from gen in Generators.OfType<ILocalGenerator>()
                                 where gen.CanExtend(model)
                                 select new GeneratorPartial(gen, gen.Extend(model)));

                // Initialize and execute the class skeleton template
                var code = GenerateClass(model.Name, model.Namespace, fragments.ToArray());

                // Write file
                var fileName = Path.GetFileNameWithoutExtension(file.Name) + ".Generated.cs";
                fileName = Path.Combine(Path.GetDirectoryName(file.Name), fileName);
                File.WriteAllText(fileName, code);
            }
        }

        /// <summary>
        /// Generate the global classes
        /// </summary>
        /// <param name="files"></param>
        private void GenerateGlobalClasses(IEnumerable<ParsedFile> files)
        {
            var models = files.Select(f => f.ClassModel).ToArray();
            // Determine all global classes and generate their code
            var globalClasses = (from generator in Generators.OfType<IGlobalGenerator>()
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
                File.WriteAllText(fileName, code);
            }
        }
    }
}