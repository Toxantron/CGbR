namespace CGbR
{
    /// <summary>
    /// Mode the generator is used 
    /// </summary>
    internal interface IGeneratorMode
    {
        /// <summary>
        /// Mode of this generator
        /// </summary>
        GeneratorMode Mode { get; }

        /// <summary>
        /// Initialize the mode
        /// </summary>
        /// <param name="path">Path to run generator on</param>
        /// <param name="args">Arguments passed to the generator</param>
        bool Initialize(string path, string[] args);

        /// <summary>
        /// Execute the generator on given path
        /// </summary>
        void Execute();
    }

    /// <summary>
    /// Different generator modes
    /// </summary>
    internal enum GeneratorMode
    {
        /// <summary>
        /// Operate on a single file
        /// </summary>
        File,
        /// <summary>
        /// Operate on a project
        /// </summary>
        Project,
        /// <summary>
        /// Parse and generate for entire solution
        /// </summary>
        Solution
    }
}