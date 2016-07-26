using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace CGbR.Tests
{
    [TestFixture]
    public class GeneratorTests
    {
        [TestCase(nameof(JsonSerializer), new[]
        {
            "System",
            "System.Collections.Generic",
            "System.IO",
            "System.Globalization",
            "System.Text",
            "Newtonsoft.Json"
        })]
        [TestCase(nameof(BinarySerializer), new[]
        {
            "System",
            "System.Collections.Generic",
            "System.Linq",
            "System.Text",
            "CGbR.Lib"
        })]
        [TestCase(nameof(Cloneable), new[]
        {
            "System",
            "System.Collections.Generic",
        })]
        public void Usings(string generator, string[] expected)
        {
            // Arrange
            var gen = GetGenerator(generator);

            // Act
            var interfaces = gen.Usings;

            // Assert
            Assert.AreEqual(expected.Length, interfaces.Length);
            foreach (var @interface in expected)
            {
                Assert.IsTrue(interfaces.Contains(@interface));
            }
        }

        [TestCase(nameof(JsonSerializer), "JsonValue")]
        [TestCase(nameof(BinarySerializer), "BinaryValue")]
        [TestCase(nameof(Cloneable), "CloneValue")]
        public void TestValueTypes(string generator, string expectedCode)
        {
            // Arrange
            var model = GenerateDataContract(AccessModifier.Public);
            foreach (ModelValueType value in Enum.GetValues(typeof(ModelValueType)))
            {
                if(value == ModelValueType.Class)
                    continue;

                var prop = new PropertyModel($"{value}Prop")
                {
                    AccessModifier = AccessModifier.Internal,
                    ValueType = value,
                    ElementType = value.ToString("G"),
                };
                prop.Attributes.Add(new AttributeModel("DataMember"));
                model.Properties.Add(prop);
            }

            // Act
            var gen = GetGenerator(generator);
            var generated = gen.Extend(model);

            // Assert by comparing generated fragment with test oracle
            var expected = File.ReadAllText(Path.Combine("Expected", $"{expectedCode}.txt"));
            Assert.AreEqual(expected, generated);
        }

        [TestCase(nameof(JsonSerializer), "JsonCollection")]
        [TestCase(nameof(BinarySerializer), "BinaryCollection")]
        [TestCase(nameof(Cloneable), "CloneCollection")]
        public void TestCollection(string generator, string expectedCode)
        {
            // Arrange
            var model = GenerateDataContract(AccessModifier.Public);
            var index = 0;
            foreach (ModelValueType value in Enum.GetValues(typeof(ModelValueType)))
            {
                if (value == ModelValueType.Class)
                    continue;

                var colType = CollectionType(ref index);
                var prop = new PropertyModel($"{value}{colType}")
                {
                    AccessModifier = AccessModifier.Internal,
                    ValueType = value,
                    ElementType = value.ToString("G"),
                    Dimensions = 1,
                    IsCollection = true,
                    CollectionType = colType
                };
                prop.Attributes.Add(new AttributeModel("DataMember"));
                model.Properties.Add(prop);
            }

            // Act
            var gen = GetGenerator(generator);
            var generated = gen.Extend(model);

            // Assert by comparing generated fragment with test oracle
            var expected = File.ReadAllText(Path.Combine("Expected", $"{expectedCode}.txt"));
            Assert.AreEqual(expected, generated);
        }

        private static string CollectionType(ref int index)
        {
            index++;

            if (index%3 == 0)
                return "IEnumerable";

            if (index%3 == 1)
                return "List";

            return "Array";
        }

        /// <summary>
        /// Generate a class model to generate for
        /// </summary>
        private static ClassModel GenerateDataContract(AccessModifier access)
        {
            var model = new ClassModel("Dummy")
            {
                AccessModifier = access,
                IsPartial = true,
            };
            model.Attributes.Add(new AttributeModel("DataContract")); // Attributes for JSON and Binary
            model.Interfaces = new[] { "ICloneable" }; // Cloneable for CloneGenerator

            return model;
        }

        /// <summary>
        /// Fetch generator for different causes
        /// </summary>
        private static ILocalGenerator GetGenerator(string genName)
        {
            return (ILocalGenerator)GeneratorFactory.Resolve(genName);
        }
    }
}
