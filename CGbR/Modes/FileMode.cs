using System.Collections.Generic;
using System.IO;

namespace CGbR
{
    /// <summary>
    /// Mode that operates on a single input file
    /// </summary>
    internal class FileMode : ModeBase
    {
        /// <see cref="IGeneratorMode"/>
        public override GeneratorMode Mode { get; } = GeneratorMode.File;

        /// <see cref="IGeneratorMode"/>
        public override bool Initialize(string[] args)
        {
            // File mode requires a minimum of 3 arguments
            if (args.Length < 3)
            {
                return false;
            }

            // Second argument is parser
            var ext = Path.GetExtension(args[0]);
            Parsers[ext] = ParserFactory.Resolve(args[1]);

            // All following arguments are the active generators
            for (var i = 2; i < args.Length; i++)
            {
                Generators.Add(GeneratorFactory.Resolve(args[i]));
            }

            return true;
        }

        /// <see cref="IGeneratorMode"/>
        public override void Execute(string path)
        {
            // Parse file
            var file = Parse(path);
            GenerateLocalPartial(file);
        }
    }
}