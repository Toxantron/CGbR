using System;

namespace CGbR
{
    /// <summary>
    /// Factory to resolve generator mode
    /// </summary>
    internal class ModeFactory
    {
        public static IGeneratorMode Resolve(GeneratorMode mode)
        {
            switch (mode)
            {
                case GeneratorMode.File:
                    return new FileMode();
                case GeneratorMode.Project:
                    return new ProjectMode();
                case GeneratorMode.Solution:
                    return null;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }
    }
}