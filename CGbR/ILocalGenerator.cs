namespace CGbR
{
    /// <summary>
    /// Generators that only generate partial code for a single class
    /// </summary>
    public interface ILocalGenerator : IGenerator
    {
        /// <summary>
        /// Generate extension code for the given class
        /// </summary>
        /// <param name="model">Model of the class that shall be extended</param>
        /// <returns>Fragment of generated code</returns>
        string Extend(ClassModel model);
    }
}