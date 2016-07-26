//using System.Collections.Generic;
//using NUnit.Framework;

//namespace CGbR.GeneratorTests
//{
//    [TestFixture]
//    public class SizeCalculationTests
//    {
//        [TestCase("", 4)]
//        [TestCase("Hello world!", 16)]
//        public void PartialSize(string name, int expectedSize)
//        {
//            // Arrange 
//            var partial = new Partial {Name = name};

//            // Act
//            var size = partial.Size;

//            // Assert
//            Assert.AreEqual(expectedSize, size);
//        }

//        [Test]
//        public void RootSize()
//        {
//            // Arrange 
//            var root = new Root(10)
//            {
//                Numbers = new List<ulong> {10, 11, 12},
//                Partials = new[]
//                {
//                    new Partial {Id = 2, Name = "Test"},
//                    new Partial {Id = 10, Name = "Bob"}
//                }
//            };

//            // Act
//            var size = root.Size;

//            // Assert
//            var expectedSize = 8;
//            expectedSize += 24;
//            expectedSize += 8;
//            expectedSize += 7;
//            Assert.AreEqual(expectedSize, size);
//        }
//    }
//}