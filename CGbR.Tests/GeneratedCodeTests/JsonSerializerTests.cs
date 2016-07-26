//using System.Linq;
//using NUnit.Framework;

//namespace CGbR.GeneratorTests
//{
//    [TestFixture]
//    public class JsonSerializerTests
//    {
//        [Test]
//        public void Serialize()
//        {
//            // Arrange
//            var root = new Root(10)
//            {
//                Partials = new[]
//                {
//                    new Partial {Id = 1, Name="A"},
//                    new Partial {Id = 2, Name = "B"}
//                },
//                Numbers = new ulong[] { 10, 12, 16 }
//            };

//            // Act
//            var json = root.ToJson();

//            // Assert
//            const string expected = "{\"_number\":10,\"Partials\":[{\"Id\":1,\"Name\":\"A\"},{\"Id\":2,\"Name\":\"B\"},],\"Numbers\":[10,12,16,]}";
//            Assert.AreEqual(expected, json, "Failed to serialize string");
//        }

//        [Test]
//        public void SerializeNullArray()
//        {
//            // Arrange 
//            var root = new Root(12);

//            // Act
//            var json = root.ToJson();

//            // Assert
//            const string expected = "{\"_number\":12,\"Partials\":null,\"Numbers\":null}";
//            Assert.AreEqual(expected, json, "Serializer failed");
//        }

//        [Test]
//        public void SerializeEmptyArray()
//        {
//            // Arrange 
//            var root = new Root(15)
//            {
//                Partials = new Partial[0],
//                Numbers = new ulong[0]
//            };

//            // Act
//            var json = root.ToJson();

//            // Assert
//            const string expected = "{\"_number\":15,\"Partials\":[],\"Numbers\":[]}";
//            Assert.AreEqual(expected, json, "Serializer failed");
//        }

//        [Test]
//        public void Deserialize()
//        {
//            // Arrange
//            const string json = "{\"_number\":20,\"Partials\":[{\"Id\":1},{\"Id\":2}],\"Numbers\":[10,12,16]}";

//            // Act
//            var deserialized = new Root(0).FromJson(json);

//            // Assert
//            Assert.NotNull(deserialized);
//            Assert.AreEqual(20, deserialized.Number, "Failed to parse number");
//            Assert.AreEqual(2, deserialized.Partials.Length, "Failed to parse class array");
//            Assert.AreEqual(1, deserialized.Partials[0].Id);
//            Assert.AreEqual(2, deserialized.Partials[1].Id);
//            Assert.AreEqual(3, deserialized.Numbers.Count);
//            Assert.AreEqual(38, deserialized.Numbers.Sum(i => (int)i));
//        }

//        [Test]
//        public void DeserializeEmptyArray()
//        {
//            // Arrange
//            const string json = "{\"_number\":10,\"Partials\":[],\"Numbers\":[]}";

//            // Act
//            var deserialized = new Root(0).FromJson(json);

//            // Assert
//            Assert.NotNull(deserialized);
//            Assert.AreEqual(10, deserialized.Number);
//            Assert.AreEqual(0, deserialized.Partials.Length);
//            Assert.AreEqual(0, deserialized.Numbers.Count);
//        }

//        [Test]
//        public void DeserializeNullArray()
//        {
//            // Arrange
//            const string json = "{\"_number\":30,\"Partials\":null,\"Numbers\":null}";

//            // Act
//            var deserialized = new Root(0).FromJson(json);

//            // Assert
//            Assert.NotNull(deserialized);
//            Assert.AreEqual(30, deserialized.Number);
//            Assert.IsNull(deserialized.Partials);
//            Assert.IsNull(deserialized.Numbers);
//        }
//    }
//}