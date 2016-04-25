using System.Collections.Generic;
using System.IO;

namespace CGbR
{
    /// <summary>
    /// Mode that operates on a single input file
    /// </summary>
    internal class FileMode : ModeBase
    {
        private string _path;

        /// <see cref="IGeneratorMode"/>
        public override bool Initialize(string path, string[] args)
        {
            // File mode requires a minimum of 3 arguments
            _path = path;
            if (args.Length < 2)
            {
                return false;
            }

            // Second argument is parser
            var ext = Path.GetExtension(_path);
            Parsers[ext] = ParserFactory.Resolve(args[0]);

            // All following arguments are the active generators
            for (var i = 1; i < args.Length; i++)
            {
                Generators.Add(GeneratorFactory.Resolve(args[i]));
            }

            return true;
        }

        /// <see cref="IGeneratorMode"/>
        public override void Execute()
        {
            // Parse file
            var file = Parse(_path);
            GenerateLocalPartial(file);
        }
    }
}