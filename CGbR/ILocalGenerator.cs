namespace CGbR
{
    /// <summary>
    /// Generators that only generate partial code for a single class
    /// </summary>
    public interface ILocalGenerator : IGenerator
    {
        /// <summary>
        /// Check if this generator has any additions to this class
        /// </summary>
        /// <returns><c>true</c> if this instance can extend the specified model; otherwise, <c>false</c>.</returns>
        /// <param name="model">Class model that shall be extended</param>
        bool CanExtend(ClassModel model);

        /// <summary>
        /// Generate extension code for the given class
        /// </summary>
        /// <param name="model">Model of the class that shall be extended</param>
        /// <returns>Fragment of generated code</returns>
        string Extend(ClassModel model);
    }
}