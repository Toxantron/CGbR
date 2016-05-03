/*
 * This code was generated by the CGbR generator on 03.05.2016. Any manual changes will be lost on the next build.
 * 
 * For questions or bug reports please refer to https://github.com/Toxantron/CGbR or contact the distributor of the
 * 3rd party generator target.
 */
using CGbR.Lib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace CGbR.GeneratorTests
{
    /// <summary>
    /// Auto generated class by CGbR project
    /// </summary>
    public partial class DifferentTypes : IByteSerializable
    {
        #region BinarySerializer

        /// <summary>
        /// Binary size of the object
        /// </summary>
        public int Size
        {
            get 
            { 
                var size = 21;
                // Add size for collections and strings
  
                return size;              
            }
        }

        /// <summary>
        /// Convert object to bytes
        /// </summary>
        public byte[] ToBytes()
        {
            var index = 0;
            var bytes = new byte[Size];

            return ToBytes(bytes, ref index);
        }

        /// <summary>
        /// Convert object to bytes within object tree
        /// </summary>
        void IByteSerializable.ToBytes(byte[] bytes, ref int index)
        {
            ToBytes(bytes, ref index);
        }

        /// <summary>
        /// Convert object to bytes within object tree
        /// </summary>
        public byte[] ToBytes(byte[] bytes, ref int index)
        {
            if (index + Size > bytes.Length)
                throw new ArgumentOutOfRangeException("index", "Object does not fit in array");

            // Convert Time
            GeneratorByteConverter.Include(Time.ToBinary(), bytes, ref index);
            // Convert Short
            GeneratorByteConverter.Include((Byte)Short, bytes, ref index);
            // Convert Some
            GeneratorByteConverter.Include((Int32)Some, bytes, ref index);
            // Convert Big
            GeneratorByteConverter.Include((Int64)Big, bytes, ref index);
            return bytes;
        }

        /// <summary>
        /// Create object from byte array
        /// </summary>
        public DifferentTypes FromBytes(byte[] bytes)
        {
            var index = 0;            
            return FromBytes(bytes, ref index); 
        }

        /// <summary>
        /// Create object from segment in byte array
        /// </summary>
        void IByteSerializable.FromBytes(byte[] bytes, ref int index)
        {
            FromBytes(bytes, ref index);
        }

        /// <summary>
        /// Create object from segment in byte array
        /// </summary>
        public DifferentTypes FromBytes(byte[] bytes, ref int index)
        {
            // Read Time
            Time = DateTime.FromBinary(GeneratorByteConverter.ToInt64(bytes, ref index));
            // Read Short
            Short = (ShortEnum) bytes[index++];
            // Read Some
            Some = (SomeEnum) GeneratorByteConverter.ToInt32(bytes, ref index);
            // Read Big
            Big = (BigEnum) GeneratorByteConverter.ToInt64(bytes, ref index);

            return this;
        }

        
        #endregion
    }
}