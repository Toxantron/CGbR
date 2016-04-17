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
            var path = args[0].Replace("\"", string.Empty);
            if (File.Exists(path))
                mode = GeneratorMode.File;
            else if (Directory.GetFiles(path).Any(f => Path.GetExtension(f) == ".csproj"))
                mode = GeneratorMode.Project;
            else if (Directory.GetFiles(path).Any(f => Path.GetExtension(f) == ".sln"))
                mode = GeneratorMode.Solution;
            else
                return;

            // Prepare mode
            var generatorMode = ModeFactory.Resolve(mode);
            if (!generatorMode.Initialize(path, args.Skip(1).ToArray()))
                return;

            // Execute mode
            generatorMode.Execute();
        }
    }
}
