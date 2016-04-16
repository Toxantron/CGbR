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
        /// <param name="args">Arguments passed to the generator</param>
        bool Initialize(string[] args);

        /// <summary>
        /// Execute the generator on given path
        /// </summary>
        /// <param name="path">Path of the generator</param>
        void Execute(string path);
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