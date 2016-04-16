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
            // Check for minimum requirements
            if (args.Length < 1)
            {
                Console.WriteLine("Insufficient number for arguments. File or directory required");
                return;
            }

            // Determine mode from argument
            GeneratorMode mode;
            if (File.Exists(args[0]))
                mode = GeneratorMode.File;
            else if (Directory.GetFiles(args[0]).Any(f => Path.GetExtension(f) == ".csproj"))
                mode = GeneratorMode.Project;
            else if (Directory.GetFiles(args[0]).Any(f => Path.GetExtension(f) == ".sln"))
                mode = GeneratorMode.Solution;
            else
                return;

            // Prepare mode
            var generatorMode = ModeFactory.Resolve(mode);
            if (!generatorMode.Initialize(args))
                return;

            // Execute mode
            generatorMode.Execute(args[0]);
        }
    }
}
