namespace CGbR
{
    /// <summary>
    /// Mode the generator is used 
    /// </summary>
    public interface IGeneratorMode
    {
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
}