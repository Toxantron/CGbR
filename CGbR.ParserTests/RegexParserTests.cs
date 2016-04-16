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
            var parser = ParserFactory.Resolve(".cs");

            // Assert
            Assert.IsNotNull(parser);
            Assert.AreEqual("Regex", parser.Name, "Not the right parser created");
        }

        [Test]
        public void ParsePlain()
        {
            // Arrange
            var parser = ParserFactory.Resolve(".cs");
            const string code =
@"namespace Test
{
    public partial class Plain
    {
    }
}
";
            File.WriteAllText("Plain.cs", code);

            // Act
            var model = parser.ParseFile("Plain.cs");

            // Assert
            Assert.IsNotNull(model);
            Assert.AreEqual("Test", model.Namespace, "Namespace not parsed!");
            Assert.AreEqual("Plain", model.Name, "Name not parsed!");
        }

        [Test]
        public void ParseComplex()
        {
            // Arrange
            var parser = ParserFactory.Resolve(".cs");
            const string code =
@"namespace Test.With.Dots
{
    public partial class Complex : Base, IInterface
    {
    }
}
";
            File.WriteAllText("Complex.cs", code);

            // Act
            var model = parser.ParseFile("Complex.cs");

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
            var parser = ParserFactory.Resolve(".cs");
            const string code =
@"namespace Test.Attributes
{
    [Test(1, Blub = 2)]
    public partial class Attributes
    {
    }
}
";
            File.WriteAllText("Attributes.cs", code);

            // Act
            var model = parser.ParseFile("Attributes.cs");

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
            var parser = ParserFactory.Resolve(".cs");
            const string code =
@"namespace Test
{
    public partial class Test
    {
        public int WithoutAtt { get; set; }

        [DataMember]
        public ushort WithAtt { get; set; }
    }
}
";
            File.WriteAllText("Test.cs", code);

            // Act
            var model = parser.ParseFile("Test.cs");

            // Assert
            Assert.AreEqual(2, model.Properties.Count, "Number of properties does not match!");
            var prop = model.Properties[0];
            Assert.AreEqual("WithoutAtt", prop.Name, "Name of first property does not match!");
            Assert.AreEqual("int", prop.ElementType, "Type of property does not match");
            Assert.AreEqual(0, prop.Attributes.Count, "First property does not have attributes");
            prop = model.Properties[1];
            Assert.AreEqual("WithAtt", prop.Name, "Name of first property does not match!");
            Assert.AreEqual("ushort", prop.ElementType, "Type of property does not match");
            Assert.AreEqual(1, prop.Attributes.Count, "Second property should have attributes");
        }

        [Test]
        public void ParseList()
        {
            // Arrange
            var parser = ParserFactory.Resolve(".cs");
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
            File.WriteAllText("List.cs", code);

            // Act
            var parsed = parser.ParseFile("List.cs");

            // Assert
            Assert.AreEqual(1, parsed.Properties.Count);
            var prop = parsed.Properties.First();
            Assert.AreEqual("Numbers", prop.Name);
            Assert.AreEqual("ushort", prop.ElementType);
            Assert.AreEqual("IList", prop.CollectionType);
        }
    }
}
