//using NUnit.Framework;

//namespace CGbR.GeneratorTests
//{
//    [TestFixture]
//    public class BinarySerializerTests
//    {
//        [Test]
//        public void EmptyToBytes()
//        {
//            // Arrange
//            var root = new Root(0);

//            // Act
//            var bytes = root.ToBytes();

//            // Assert
//            Assert.AreEqual(8, bytes.Length);
//            Assert.IsTrue(BytesEqual(bytes, new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 }));
//        }

//        [Test]
//        public void FieldOnly()
//        {
//            // Arrange
//            var root = new Root(10);

//            // Act
//            var bytes = root.ToBytes();

//            // Assert
//            Assert.AreEqual(8, bytes.Length);
//            Assert.IsTrue(BytesEqual(bytes, new byte[] { 10, 0, 0, 0, 0, 0, 0, 0 }));
//        }

//        private static bool BytesEqual(byte[] first, byte[] second)
//        {
//            if (first.Length != second.Length)
//                return false;

//            for (int i = 0; i < first.Length; i++)
//            {
//                if (first[i] != second[i])
//                    return false;
//            }
//            return true;
//        }
//    }
//}