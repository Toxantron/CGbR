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

            Console.WriteLine($"Starting CGbR on {args[0]}");

            // Prepare mode
            var path = args[0].Replace("\"", string.Empty);
            var generatorMode = ModeFactory.Resolve(path);
            if (!generatorMode.Initialize(path, args.Skip(1).ToArray()))
                return;

            // Execute mode
            generatorMode.Execute();
        }
    }
}
