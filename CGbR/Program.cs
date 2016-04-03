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
        /// Entry method for the application
        /// </summary>
        /// <param name="args"></param>
		public static void Main(string[] args)
        {
            // Determine mode from argument
            GeneratorMode mode;
            if(Path.GetExtension(args[0]) == ".cs")
                mode = GeneratorMode.File;
            else if (Directory.GetFiles(args[0]).Any(f => Path.GetExtension(f) == ".csproj"))
                mode = GeneratorMode.Project;
            else if (Directory.GetFiles(args[0]).Any(f => Path.GetExtension(f) == ".sln"))
                mode = GeneratorMode.Solution;
            else
                return;

            // Initialize local strategies
            var parser = ParserFactory.Resolve("Regex");
            var generators = GeneratorFactory.ResolveAll();

            // Prepare mode
            var generatorMode = ModeFactory.Resolve(mode);
            generatorMode.Initialize(parser, generators);

            // Execute mode
            generatorMode.Execute(args[0]);
        }
    }
}
