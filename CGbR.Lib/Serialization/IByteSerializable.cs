using System.Security.Cryptography.X509Certificates;

namespace CGbR.Lib
{
    /// <summary>
    /// Instances of this class have generated methods for binary serialization and deserialization
    /// </summary>
    public interface IByteSerializable
    {
        /// <summary>
        /// Binary size of this object and its children
        /// </summary>
        int Size { get; }

        /// <summary>
        /// Write object to the given byte array
        /// </summary>
        /// <param name="bytes">Target array</param>
        /// <param name="index">Position in the array</param>
        /// <returns></returns>
        void ToBytes(byte[] bytes, ref int index);

        /// <summary>
        /// Read object properties from bytes
        /// </summary>
        /// <param name="bytes">Serialized array of bytes</param>
        /// <param name="index">Current position in array</param>
        void FromBytes(byte[] bytes, ref int index);
    }
}