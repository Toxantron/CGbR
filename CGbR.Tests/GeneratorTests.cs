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
                    Attributes = DataMember
                };
                model.Properties.Add(prop);
            }

            // Act
            var gen = GetGenerator(generator);
            var generated = gen.Extend(model);

            // Assert by comparing generated fragment with test oracle
            var expected = File.ReadAllText(Path.Combine("Expected", $"{expectedCode}.txt"));
            CodeCompare(expected, generated);
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
                    CollectionType = colType,
                    Attributes = DataMember
                };
                model.Properties.Add(prop);
            }

            // Act
            var gen = GetGenerator(generator);
            var generated = gen.Extend(model);

            // Assert by comparing generated fragment with test oracle
            var expected = File.ReadAllText(Path.Combine("Expected", $"{expectedCode}.txt"));
            CodeCompare(expected, generated);
        }

        [TestCase(nameof(JsonSerializer), "JsonClasses")]
        [TestCase(nameof(BinarySerializer), "BinaryClasses")]
        [TestCase(nameof(Cloneable), "CloneClasses")]
        public void ClassReference(string generator, string expectedCode)
        {
            // Arrange
            var model = GenerateDataContract(AccessModifier.Internal);
            model.Properties.Add(new PropertyModel("FixedSize")
            {
                ElementType = "FixedClass",
                ValueType = ModelValueType.Class,
                Attributes = DataMember
            });
            model.Properties.Add(new PropertyModel("FlexSize")
            {
                ElementType = "FlexClass",
                ValueType = ModelValueType.Class,
                Attributes = DataMember
            });
            model.Properties.Add(new PropertyModel("FixCollection")
            {
                IsCollection = true, 
                ElementType = "FixedClass",
                ValueType = ModelValueType.Class,
                Attributes = DataMember,
                Dimensions = 1,
                CollectionType = "List"
            });
            model.Properties.Add(new PropertyModel("FlexCollection")
            {
                IsCollection = true,
                ElementType = "FlexClass",
                ValueType = ModelValueType.Class,
                Attributes = DataMember,
                Dimensions = 1,
                CollectionType = "Array"
            });
            model.References.Add(GenerateSub(true));
            model.References.Add(GenerateSub(false));

            // Act
            var gen = GetGenerator(generator);
            var code = gen.Extend(model);

            // Assert by comparing generated fragment with test oracle
            var expected = File.ReadAllText(Path.Combine("Expected", $"{expectedCode}.txt"));
            CodeCompare(expected, code);
        }

        private static ClassModel GenerateSub(bool fixedSize)
        {
            var model = GenerateDataContract(AccessModifier.Public, fixedSize ? "FixedClass" : "FlexClass");

            // Add a few fixed properties
            model.Properties.Add(new PropertyModel("Integer")
            {
                ElementType = "int",
                ValueType = ModelValueType.Int32,
                Attributes = DataMember
            });
            model.Properties.Add(new PropertyModel("Double")
            {
                ElementType = "double",
                ValueType = ModelValueType.Double,
                Attributes = DataMember
            });

            // Add a collection and string for flex size only
            if (fixedSize)
                return model;

            model.Properties.Add(new PropertyModel("Integers")
            {
                IsCollection = true,
                ElementType = "int",
                ValueType = ModelValueType.UInt32,
                CollectionType = "Array",
                Dimensions = 1,
                Attributes = DataMember
            });
            model.Properties.Add(new PropertyModel("Text")
            {
                ElementType = "string",
                ValueType = ModelValueType.String,
                Attributes = DataMember
            });

            return model;
        }

        private static void CodeCompare(string expected, string value)
        {
            // Normalize all line endings
            var expectedClean = expected.Replace("\r\n", "\n");
            var valueClean = value.Replace("\r\n", "\n");

            Assert.AreEqual(expectedClean, valueClean);
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
        /// Data member definition for properties
        /// </summary>
        private static readonly List<AttributeModel> DataMember = new List<AttributeModel> {new AttributeModel("DataMember")};

        /// <summary>
        /// Generate a class model to generate for
        /// </summary>
        private static ClassModel GenerateDataContract(AccessModifier access, string name = "Dummy")
        {
            var model = new ClassModel(name)
            {
                AccessModifier = access,
                IsPartial = true,
                Attributes = new List<AttributeModel> {new AttributeModel("DataContract")}, // Attributes for JSON and Binary
                Interfaces = new[] {"ICloneable" } // Cloneable for CloneGenerator
            };

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
