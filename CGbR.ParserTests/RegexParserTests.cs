using System;
using System.IO;
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

        [TestCase("Plain.cs", "Plain", "Test", "", new string[0], Description = "Parse plain class")]
        public void ParseClass(string file, string className, string @namespace, string baseType, string[] interfaces)
        {
            // Arrange
            var parser = ParserFactory.Resolve("Regex");

            // Act
            var model = parser.ParseFile(Path.Combine("DummyClasses", file));

            // Assert
            Assert.IsNotNull(model);
            Assert.AreEqual(@namespace, model.Namespace, "Namespace not parsed!");
            Assert.AreEqual(className, model.Name, "Name not parsed!");
        }
    }
}
