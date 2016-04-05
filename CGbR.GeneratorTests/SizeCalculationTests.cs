using NUnit.Framework;
using NUnit.Framework.Internal;

namespace CGbR.GeneratorTests
{
    [TestFixture]
    public class SizeCalculationTests
    {
        [TestCase("", 4)]
        [TestCase("Hello world!", 16)]
        public void PartialSize(string name, int expectedSize)
        {
            // Arrange 
            var partial = new Partial {Name = name};

            // Act
            var size = partial.Size;

            // Assert
            Assert.AreEqual(expectedSize, size);
        }
    }
}