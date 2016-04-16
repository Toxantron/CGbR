namespace CGbR.Configuration
{
    /// <summary>
    /// Config for local generators
    /// </summary>
    public class LocalGeneratorConfig
    {
        /// <summary>
        /// Name of the local generator
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Flag if this generator is enabled
        /// </summary>
        public bool IsEnabled { get; set; }
    }
}