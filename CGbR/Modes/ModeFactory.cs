using System;
using System.IO;
using System.Linq;

namespace CGbR
{
    /// <summary>
    /// Factory to resolve generator mode
    /// </summary>
    public class ModeFactory
    {
        /// <summary>
        /// Resolve mode for given path argument
        /// </summary>
        /// <param name="path">Path the generator shall operate on</param>
        /// <returns></returns>
        public static IGeneratorMode Resolve(string path)
        {
            if (File.Exists(path))
                return new FileMode();
            if (Directory.GetFiles(path).Any(f => Path.GetExtension(f) == ".csproj"))
                return new ProjectMode();
            if (Directory.GetFiles(path).Any(f => Path.GetExtension(f) == ".sln"))
                return null;

            return null;
        }
    }
}