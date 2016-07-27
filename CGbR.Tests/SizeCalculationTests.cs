using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace CGbR.GeneratorTests
{
    [TestFixture]
    public class SizeCalculationTests
    {
        [TestCase(ModelValueType.Byte, 1)]
        [TestCase(ModelValueType.Boolean, 1)]
        [TestCase(ModelValueType.Char, 2)]
        [TestCase(ModelValueType.Int16, 2)]
        [TestCase(ModelValueType.UInt16, 2)]
        [TestCase(ModelValueType.Int32,4)]
        [TestCase(ModelValueType.UInt32, 4)]
        [TestCase(ModelValueType.Single, 4)]
        [TestCase(ModelValueType.Int64, 8)]
        [TestCase(ModelValueType.UInt64, 8)]
        [TestCase(ModelValueType.Double, 8)]
        public void ValueTypeSize(ModelValueType type, int expectedSize)
        {
            // Arrange 
            var model = new ClassModel("Dummy");
            model.Properties.Add(new PropertyModel("Property")
            {
                ValueType = type,
                ElementType = type.ToString("G"),
                Attributes = new List<AttributeModel> { new AttributeModel("DataMember") }
            });

            // Act
            var size = BinarySize.OfClass(model, new BinarySerializer());

            // Assert
            Assert.AreEqual(expectedSize, size);
        }

        [Test]
        public void CollectionSizes()
        {
            // Arrange
            var model = new ClassModel("Dummy");
            model.Properties.Add(new PropertyModel("Dim1")
            {
                IsCollection = true,
                CollectionType = "List",
                Dimensions = 1,
                Attributes = new List<AttributeModel> { new AttributeModel("DataMember") }
            });
            model.Properties.Add(new PropertyModel("Dim2")
            {
                IsCollection = true,
                CollectionType = "Array",
                Dimensions = 2,
                Attributes = new List<AttributeModel> { new AttributeModel("DataMember") }
            });

            // Act
            var size = BinarySize.OfClass(model, new BinarySerializer());

            // Assert
            Assert.AreEqual(6, size);
        }

        [TestCase(ModelValueType.Byte, 1)]
        [TestCase(ModelValueType.Int16, 2)]
        [TestCase(ModelValueType.Int32, 4)]
        [TestCase(ModelValueType.Int64, 8)]
        public void EnumSize(ModelValueType baseType, int expectedSize)
        {
            // Arrange
            var model = new ClassModel("Dummy");
            model.Properties.Add(new PropertyModel("Enum")
            {
                ElementType = "SomeEnum",
                ValueType = ModelValueType.Class,
                Attributes = new List<AttributeModel> { new AttributeModel("DataMember") }
            });
            model.References = new List<CodeElementModel> {new EnumModel("SomeEnum")
            {
                BaseType = baseType
            }};

            // Act
            var size = BinarySize.OfClass(model, new BinarySerializer());

            // Assert
            Assert.AreEqual(expectedSize, size);
        }
    }
}