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
    public partial class DifferentCollections : IByteSerializable
    {
        #region BinarySerializer

        /// <summary>
        /// Binary size of the object
        /// </summary>
        public int Size
        {
            get 
            { 
                var size = 14;
                // Add size for collections and strings
                size += Integers == null ? 0 : Integers.Count() * 4;
                size += Doubles == null ? 0 : Doubles.Count * 8;
                size += Longs == null ? 0 : Longs.Length * 8;
                size += MultiDimension == null ? 0 : MultiDimension.Length * 4;
                size += Times == null ? 0 : Times.Count * 8;
                size += Names == null ? 0 : Names.Sum(s => s.Length + 2);
  
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

            // Convert Integers
            // Two bytes length information for each dimension
            GeneratorByteConverter.Include((ushort)(Integers == null ? 0 : Integers.Count()), bytes, ref index);
            if (Integers != null)
            {
                foreach(var value in Integers)
                {
            		GeneratorByteConverter.Include(value, bytes, ref index);
                }
            }
            // Convert Doubles
            // Two bytes length information for each dimension
            GeneratorByteConverter.Include((ushort)(Doubles == null ? 0 : Doubles.Count), bytes, ref index);
            if (Doubles != null)
            {
                for(var i = 0; i < Doubles.Count; i++)
                {
                    var value = Doubles[i];
            		GeneratorByteConverter.Include(value, bytes, ref index);
                }
            }
            // Convert Longs
            // Two bytes length information for each dimension
            GeneratorByteConverter.Include((ushort)(Longs == null ? 0 : Longs.Length), bytes, ref index);
            if (Longs != null)
            {
                for(var i = 0; i < Longs.Length; i++)
                {
                    var value = Longs[i];
            		GeneratorByteConverter.Include(value, bytes, ref index);
                }
            }
            // Convert MultiDimension
            // Two bytes length information for each dimension
            GeneratorByteConverter.Include((ushort)(MultiDimension == null ? 0 : MultiDimension.GetLength(0)), bytes, ref index);
            GeneratorByteConverter.Include((ushort)(MultiDimension == null ? 0 : MultiDimension.GetLength(1)), bytes, ref index);
            if (MultiDimension != null)
            {
                for(var i = 0; i < MultiDimension.GetLength(0); i++)
                for(var j = 0; j < MultiDimension.GetLength(1); j++)
                {
                    var value = MultiDimension[i,j];
            		GeneratorByteConverter.Include(value, bytes, ref index);
                }
            }
            // Convert Times
            // Two bytes length information for each dimension
            GeneratorByteConverter.Include((ushort)(Times == null ? 0 : Times.Count), bytes, ref index);
            if (Times != null)
            {
                for(var i = 0; i < Times.Count; i++)
                {
                    var value = Times[i];
            		GeneratorByteConverter.Include(value.ToBinary(), bytes, ref index);
                }
            }
            // Convert Names
            // Two bytes length information for each dimension
            GeneratorByteConverter.Include((ushort)(Names == null ? 0 : Names.Count), bytes, ref index);
            if (Names != null)
            {
                foreach(var value in Names)
                {
            		GeneratorByteConverter.Include(value, bytes, ref index);
                }
            }
            return bytes;
        }

        /// <summary>
        /// Create object from byte array
        /// </summary>
        public DifferentCollections FromBytes(byte[] bytes)
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
        public DifferentCollections FromBytes(byte[] bytes, ref int index)
        {
            // Read Integers
            var integersLength = GeneratorByteConverter.ToUInt16(bytes, ref index);
            var tempIntegers = new List<int>(integersLength);
            for (var i = 0; i < integersLength; i++)
            {
            	var value = GeneratorByteConverter.ToInt32(bytes, ref index);
                tempIntegers.Add(value);
            }
            Integers = tempIntegers;
            // Read Doubles
            var doublesLength = GeneratorByteConverter.ToUInt16(bytes, ref index);
            var tempDoubles = new List<double>(doublesLength);
            for (var i = 0; i < doublesLength; i++)
            {
            	var value = GeneratorByteConverter.ToDouble(bytes, ref index);
                tempDoubles.Add(value);
            }
            Doubles = tempDoubles;
            // Read Longs
            var longsLength = GeneratorByteConverter.ToUInt16(bytes, ref index);
            var tempLongs = new long[longsLength];
            for (var i = 0; i < longsLength; i++)
            {
            	var value = GeneratorByteConverter.ToInt64(bytes, ref index);
                tempLongs[i] = value;
            }
            Longs = tempLongs;
            // Read MultiDimension
            var multidimensionLength = GeneratorByteConverter.ToUInt16(bytes, ref index);
            var multidimensionWidth = GeneratorByteConverter.ToUInt16(bytes, ref index);
            var tempMultiDimension = new uint[multidimensionLength,multidimensionWidth];
            for (var i = 0; i < multidimensionLength; i++)
            for (var j = 0; j < multidimensionWidth; j++)
            {
            	var value = GeneratorByteConverter.ToUInt32(bytes, ref index);
                tempMultiDimension[i,j] = value;
            }
            MultiDimension = tempMultiDimension;
            // Read Times
            var timesLength = GeneratorByteConverter.ToUInt16(bytes, ref index);
            var tempTimes = new List<DateTime>(timesLength);
            for (var i = 0; i < timesLength; i++)
            {
            	var value = DateTime.FromBinary(GeneratorByteConverter.ToInt64(bytes, ref index));
                tempTimes.Add(value);
            }
            Times = tempTimes;
            // Read Names
            var namesLength = GeneratorByteConverter.ToUInt16(bytes, ref index);
            var tempNames = new List<string>(namesLength);
            for (var i = 0; i < namesLength; i++)
            {
            	var value = GeneratorByteConverter.GetString(bytes, ref index);
                tempNames.Add(value);
            }
            Names = tempNames;

            return this;
        }

        
        #endregion
    }
}