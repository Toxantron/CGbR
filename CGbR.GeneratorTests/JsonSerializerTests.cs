using NUnit.Framework;

namespace CGbR.GeneratorTests
{
    [TestFixture]
    public class JsonSerializerTests
    {
        [Test]
        public void TestSerialize()
        {
            // Arrange
            var root = new Root
            {
                Number = 10,
                Partials = new[]
                {
                    new Partial {Id = 1},
                    new Partial {Id = 2}
                },
                Numbers = new[] {10, 12, 16}
            };

            // Act
            var json = root.ToJson();

            // Assert
            const string expected = "{\"Number\":10,\"Partials\":[{\"Id\":1},{\"Id\":2}],\"Numbers\":[10,12,16]}";
            Assert.AreEqual(expected, json, "Failed to serialize string");
        }

    }
}