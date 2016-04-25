using System.Collections.Generic;
using System.Linq;

namespace CGbR.Lib
{
    /// <summary>
    /// Helper class to serializer and deserialize objects to byte arrays
    /// </summary>
    public static class BinarySerializer
    {
        /// <summary>
        /// Serialize object to byte array
        /// </summary>
        /// <param name="instance">Instance to serialize</param>
        /// <returns>Bytes containing the object properties</returns>
        public static byte[] Serialize(IByteSerializable instance)
        {
            var index = 0;
            var bytes = new byte[instance.Size];
            instance.ToBytes(bytes, ref index);
            return bytes;
        }

        /// <summary>
        /// Serialize a collection of objects to byte array
        /// </summary>
        /// <param name="instances">Instance to serialize</param>
        /// <returns>Bytes containing the object properties</returns>
        public static byte[] SerializeMany(ICollection<IByteSerializable> instances)
        {
            var index = 0;
            var bytes = new byte[instances.Sum(i => i.Size)];
            foreach (var instance in instances)
            {
                instance.ToBytes(bytes, ref index);
            }
            return bytes;
        }

        /// <summary>
        /// Deserialize object from byte array
        /// </summary>
        /// <typeparam name="T">Type of object to deserialize</typeparam>
        /// <param name="bytes">Byte array containing property values</param>
        /// <returns>Object instance</returns>
        public static T Deserialize<T>(byte[] bytes)
            where T : IByteSerializable, new()
        {
            var index = 0;
            var instance = new T();
            instance.FromBytes(bytes, ref index);
            return instance;
        }

        /// <summary>
        /// Deserialize object from byte array
        /// </summary>
        /// <typeparam name="T">Type of object to deserialize</typeparam>
        /// <param name="bytes">Byte array containing property values</param>
        /// <returns>Object instance</returns>
        public static ICollection<T> DeserializeMany<T>(byte[] bytes)
            where T : IByteSerializable, new()
        {
            var index = 0;
            var instances = new List<T>();
            while (index < bytes.Length)
            {
                var instance = new T();
                instance.FromBytes(bytes, ref index);
                instances.Add(instance);
            }
            return instances;
        }
    }
}