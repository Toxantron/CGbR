using System;

namespace CGbR
{
    /// <summary>
    /// Factory to resolve a parser by given fileExtension
    /// </summary>
    public static class ParserFactory
    {
        /// <summary>
        /// Resolve parser by fileExtension
        /// </summary>
        /// <param name="fileExtension">Name of the parser</param>
        /// <returns>Parser instance</returns>
        public static IParser Resolve(string fileExtension)
        {
            switch (fileExtension)
            {
                case ".cs":
                    return new RegexParser();

                default:
                    throw new ArgumentException($"Parser named '{fileExtension}' not found!", nameof(fileExtension));
            }
        }
    }
}