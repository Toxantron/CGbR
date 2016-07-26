using NUnit.Framework;

namespace CGbR.Tests
{
    [TestFixture]
    public class GeneratorTests
    {
        [Test]
        public void GenerateJson()
        {
            // Arrange
            var model = new ClassModel("JsonDummy")
            {
                AccessModifier = AccessModifier.Public
            };
        }
    }
}
