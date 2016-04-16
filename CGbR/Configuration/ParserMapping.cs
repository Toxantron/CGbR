namespace CGbR.Configuration
{
    /// <summary>
    /// Mapping of file extension to parser
    /// </summary>
    public class ParserMapping
    {
        /// <summary>
        /// File extension
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// Name of the parser that shall parse files of this extension
        /// </summary>
        public string Parser { get; set; }
    }
}