using System;

namespace CGbR
{
    /// <summary>
    /// Factory to resolve a parser by given name
    /// </summary>
    public static class ParserFactory
    {
        /// <summary>
        /// Resolve parser by name
        /// </summary>
        /// <param name="name">Name of the parser</param>
        /// <returns>Parser instance</returns>
        public static IParser Resolve(string name)
        {
            switch (name)
            {
                case "Regex": return new RegexParser();

                default: throw new ArgumentException($"Parser named '{name}' not found!", nameof(name));
            }
        }
    }
}