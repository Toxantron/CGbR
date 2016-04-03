namespace CGbR
{
    /// <summary>
    /// Mode that operates on a single input file
    /// </summary>
    internal class FileMode : ModeBase
    {
        /// <see cref="IGeneratorMode"/>
        public override GeneratorMode Mode { get; } = GeneratorMode.File;

        /// <see cref="IGeneratorMode"/>
        public override void Execute(string path)
        {
            // Parse file
            var file = Parse(path);
            GenerateLocalPartial(file);
        }
    }
}