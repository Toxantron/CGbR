using System;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace CGbR.ParserTests
{
    [TestFixture]
    public class RegexParserTests
    {
        [Test]
        public void InstantiateParser()
        {
            // Act
            var parser = ParserFactory.Resolve("Regex");

            // Assert
            Assert.IsNotNull(parser);
            Assert.AreEqual("Regex", parser.Name, "Not the right parser created");
        }

        [Test]
        public void ParsePlain()
        {
            // Arrange
            var parser = ParserFactory.Resolve("Regex");
            const string code =
@"namespace Test
{
    public class Plain
    {
    }
}
";
            File.WriteAllText("PlainRegex", code);

            // Act
            var model = parser.ParseFile("PlainRegex") as ClassModel;

            // Assert
            Assert.IsNotNull(model);
            Assert.AreEqual("Test", model.Namespace, "Namespace not parsed!");
            Assert.AreEqual("Plain", model.Name, "Name not parsed!");
            Assert.IsFalse(model.IsPartial, "Class is not partial!");
            Assert.AreEqual(AccessModifier.Public, model.AccessModifier, "Failed to parse access modifier");
        }

        [Test]
        public void ParsePartial()
        {
            // Arrange
            var parser = ParserFactory.Resolve("Regex");
            const string code =
@"namespace Test
{
    public partial class Plain
    {
    }
}
";
            File.WriteAllText("PlainRegex", code);

            // Act
            var model = parser.ParseFile("PlainRegex") as ClassModel;

            // Assert
            Assert.IsNotNull(model);
            Assert.AreEqual("Test", model.Namespace, "Namespace not parsed!");
            Assert.AreEqual("Plain", model.Name, "Name not parsed!");
            Assert.IsTrue(model.IsPartial, "Class is partial!");
            Assert.AreEqual(AccessModifier.Public, model.AccessModifier, "Failed to parse access modifier");
        }

        [Test]
        public void ParseInternal()
        {
            // Arrange
            var parser = ParserFactory.Resolve("Regex");
            const string code =
@"namespace Test
{
    internal class Plain
    {
    }
}
";
            File.WriteAllText("PlainRegex", code);

            // Act
            var model = parser.ParseFile("PlainRegex") as ClassModel;

            // Assert
            Assert.IsNotNull(model);
            Assert.AreEqual("Test", model.Namespace, "Namespace not parsed!");
            Assert.AreEqual("Plain", model.Name, "Name not parsed!");
            Assert.IsFalse(model.IsPartial, "Class is not partial!");
            Assert.AreEqual(AccessModifier.Internal, model.AccessModifier, "Failed to parse access modifier");
        }

        [Test]
        public void ParseComplex()
        {
            // Arrange
            var parser = ParserFactory.Resolve("Regex");
            const string code =
@"namespace Test.With.Dots
{
    public class Complex : Base, IInterface
    {
    }
}
";
            File.WriteAllText("ComplexRegex", code);

            // Act
            var model = parser.ParseFile("ComplexRegex") as ClassModel;

            // Assert
            Assert.IsNotNull(model);
            Assert.AreEqual("Test.With.Dots", model.Namespace, "Namespace not parsed!");
            Assert.AreEqual("Complex", model.Name, "Name not parsed!");
            Assert.AreEqual("Base", model.BaseClass, "Base class not parsed");
            Assert.AreEqual(1, model.Interfaces.Length, "Number of interfaces does not match");
            Assert.AreEqual("IInterface", model.Interfaces[0], "Failed to parse interface");
        }

        [Test]
        public void ParseAttribute()
        {
            // Arrange
            var parser = ParserFactory.Resolve("Regex");
            const string code =
@"namespace Test.Attributes
{
    [Test(1, Blub = 2)]
    public partial class Attributes
    {
    }
}
";
            File.WriteAllText("AttributesRegex", code);

            // Act
            var model = parser.ParseFile("AttributesRegex");

            // Assert
            Assert.IsNotNull(model);
            Assert.AreEqual("Test.Attributes", model.Namespace, "Namespace not parsed!");
            Assert.AreEqual("Attributes", model.Name, "Name not parsed!");
            Assert.AreEqual(1, model.Attributes.Count, "Number of attributes does not match!");
            var attribute = model.Attributes[0];
            Assert.AreEqual("Test", attribute.Name);
            Assert.AreEqual(1,attribute.Parameters.Count, "Number of parameters does not match!");
            Assert.AreEqual("1", attribute.Parameters[0], "Parameter does not match!");
            Assert.AreEqual(1, attribute.Properties.Count, "Number of properties does not match");
            var prop = attribute.Properties[0];
            Assert.AreEqual("Blub", prop.Name, "Name of the property not parsed!");
            Assert.AreEqual("2", prop.Value, "Value of properties does not match");
        }

        [Test]
        public void ParseProperties()
        {
            // Arrange
            var parser = ParserFactory.Resolve("Regex");
            const string code =
@"namespace Test
{
    public partial class Test
    {
        internal int WithoutAtt { get; set; }

        [DataMember]
        public ushort WithAtt { get; set; }
    }
}
";
            File.WriteAllText("TestRegex", code);

            // Act
            var model = parser.ParseFile("TestRegex") as ClassModel;

            // Assert
            Assert.NotNull(model);
            Assert.AreEqual(2, model.Properties.Count, "Number of properties does not match!");
            var prop = model.Properties[0];
            Assert.AreEqual("WithoutAtt", prop.Name, "Name of first property does not match!");
            Assert.AreEqual("int", prop.ElementType, "Type of property does not match");
            Assert.AreEqual(AccessModifier.Internal, prop.AccessModifier);
            Assert.AreEqual(0, prop.Attributes.Count, "First property does not have attributes");
            prop = model.Properties[1];
            Assert.AreEqual("WithAtt", prop.Name, "Name of first property does not match!");
            Assert.AreEqual("ushort", prop.ElementType, "Type of property does not match");
            Assert.AreEqual(AccessModifier.Public, prop.AccessModifier);
            Assert.AreEqual(1, prop.Attributes.Count, "Second property should have attributes");
        }

        [Test]
        public void ParseList()
        {
            // Arrange
            var parser = ParserFactory.Resolve("Regex");
            const string code =
@"namespace Test
{
    public partial class Test
    {
        [DataMember]
        public IList<ushort> Numbers { get; set; }
    }
}
";
            File.WriteAllText("ListRegex", code);

            // Act
            var parsed = parser.ParseFile("ListRegex") as ClassModel;

            // Assert
            Assert.NotNull(parsed);
            Assert.AreEqual(1, parsed.Properties.Count);
            var prop = parsed.Properties.First();
            Assert.AreEqual("Numbers", prop.Name);
            Assert.AreEqual("ushort", prop.ElementType);
            Assert.AreEqual(ModelValueType.UInt16, prop.ValueType);
            Assert.AreEqual("IList", prop.CollectionType);
            Assert.AreEqual(AccessModifier.Public, parsed.AccessModifier);
        }

        [Test]
        public void ParseField()
        {
            // Arrange
            var parser = ParserFactory.Resolve("Regex");
            const string code =
@"namespace Test
{
    public class Test
    {
        private long _number;
    }
}
";
            File.WriteAllText("FieldRegex", code);

            // Act
            var parsed = parser.ParseFile("FieldRegex") as ClassModel;

            // Assert
            Assert.NotNull(parsed);
            Assert.AreEqual(1, parsed.Properties.Count);
            var prop = parsed.Properties.First();
            Assert.AreEqual("_number", prop.Name);
            Assert.AreEqual("long", prop.ElementType);
            Assert.AreEqual(ModelValueType.Int64, prop.ValueType);
        }

        [Test]
        public void ParseEnum()
        {
            // Arrange
            var parser = ParserFactory.Resolve("Regex");
            const string code =
@"namespace Test
{
    internal enum Plain
    {
    }
}
";
            File.WriteAllText("Enum", code);

            // Act
            var model = parser.ParseFile("Enum") as EnumModel;

            // Assert
            Assert.NotNull(model);
            Assert.AreEqual("Plain", model.Name);
            Assert.AreEqual(ModelValueType.Int32, model.BaseType);
            Assert.AreEqual(AccessModifier.Internal, model.AccessModifier);
        }

        [Test]
        public void ParseEnumBaseType()
        {
            // Arrange
            var parser = ParserFactory.Resolve("Regex");
            const string code =
@"namespace Test
{
    public enum Short : short
    {
    }
}
";
            File.WriteAllText("Enum", code);

            // Act
            var model = parser.ParseFile("Enum") as EnumModel;

            // Assert
            Assert.NotNull(model);
            Assert.AreEqual("Short", model.Name);
            Assert.AreEqual(ModelValueType.Int16, model.BaseType);
            Assert.AreEqual(AccessModifier.Public, model.AccessModifier);
        }

        [Test]
        public void ParseEnumMembers()
        {
            // Arrange
            var parser = ParserFactory.Resolve("Regex");
            const string code =
@"namespace Test
{
    internal enum Plain
    {
        A,
        B
    }
}
";
            File.WriteAllText("Enum", code);

            // Act
            var model = parser.ParseFile("Enum") as EnumModel;

            // Assert
            Assert.NotNull(model);
            Assert.AreEqual("Plain", model.Name);
            Assert.AreEqual(ModelValueType.Int32, model.BaseType);
            Assert.AreEqual(AccessModifier.Internal, model.AccessModifier);
            Assert.AreEqual(2, model.Members.Count);
            var first = model.Members[0];
            Assert.AreEqual("A", first.Name);
            Assert.AreEqual(1, first.Value);
            var second = model.Members[1];
            Assert.AreEqual("B", second.Name);
            Assert.AreEqual(2, second.Value);
        }

        [Test]
        public void ParseNumberedMembers()
        {
            // Arrange
            var parser = ParserFactory.Resolve("Regex");
            const string code =
@"namespace Test
{
    internal enum Plain
    {
        A = 2,
        B = 8
    }
}
";
            File.WriteAllText("Enum", code);

            // Act
            var model = parser.ParseFile("Enum") as EnumModel;

            // Assert
            Assert.NotNull(model);
            Assert.AreEqual("Plain", model.Name);
            Assert.AreEqual(ModelValueType.Int32, model.BaseType);
            Assert.AreEqual(AccessModifier.Internal, model.AccessModifier);
            Assert.AreEqual(2, model.Members.Count);
            var first = model.Members[0];
            Assert.AreEqual("A", first.Name);
            Assert.AreEqual(2, first.Value);
            var second = model.Members[1];
            Assert.AreEqual("B", second.Name);
            Assert.AreEqual(8, second.Value);
        }
    }
}
