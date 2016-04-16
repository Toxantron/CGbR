namespace CGbR.Configuration
{
    /// <summary>
    /// Config for a global generator
    /// </summary>
    public class GlobalGeneratorConfig
    {
        /// <summary>
        /// Name of the generator
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Flag if this generator is enabled
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Output directory for this global generator
        /// </summary>
        public string OutputDir { get; set; }
    }
}