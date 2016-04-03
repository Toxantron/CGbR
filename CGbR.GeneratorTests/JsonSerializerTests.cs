using System.Linq;
using NUnit.Framework;

namespace CGbR.GeneratorTests
{
    [TestFixture]
    public class JsonSerializerTests
    {
        [Test]
        public void Serialize()
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

        [Test]
        public void Deserialize()
        {
            // Arrange
            const string json = "{\"Number\":10,\"Partials\":[{\"Id\":1},{\"Id\":2}],\"Numbers\":[10,12,16]}";
            
            // Act
            var deserialized = new Root().FromJson(json);

            // Assert
            Assert.NotNull(deserialized);
            Assert.AreEqual(10, deserialized.Number, "Failed to parse number");
            Assert.AreEqual(2, deserialized.Partials.Length, "Failed to parse class array");
            Assert.AreEqual(1, deserialized.Partials[0].Id);
            Assert.AreEqual(2, deserialized.Partials[1].Id);
            //Assert.AreEqual(3, deserialized.Numbers.Length);
            //Assert.AreEqual(38, deserialized.Numbers.Sum());
        }
    }
}