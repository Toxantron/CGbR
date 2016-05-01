using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGbR.Lib
{
    /// <summary>
    /// Byte converter for the generator only! All methods are unsafe and should
    /// not be used outside of generated code
    /// </summary>
    public static class GeneratorByteConverter
    {
        private static readonly Encoding Encoder = new UTF8Encoding();

        /// <summary>
        /// Writer property of type Int16 to bytes by using pointer opertations
        /// </summary>
        public static unsafe void Include(short value, byte[] bytes, ref int index)
        {
            fixed (byte* b = &bytes[index])
                *((short*)b) = value;
            index += 2;
        }
        /// <summary>
        /// Writer property of type UInt16 to bytes by using pointer opertations
        /// </summary>
        public static unsafe void Include(ushort value, byte[] bytes, ref int index)
        {
            fixed (byte* b = &bytes[index])
                *((ushort*)b) = value;
            index += 2;
        }
        /// <summary>
        /// Writer property of type Int32 to bytes by using pointer opertations
        /// </summary>
        public static unsafe void Include(int value, byte[] bytes, ref int index)
        {
            fixed (byte* b = &bytes[index])
                *((int*)b) = value;
            index += 4;
        }
        /// <summary>
        /// Writer property of type UInt32 to bytes by using pointer opertations
        /// </summary>
        public static unsafe void Include(uint value, byte[] bytes, ref int index)
        {
            fixed (byte* b = &bytes[index])
                *((uint*)b) = value;
            index += 4;
        }
        /// <summary>
        /// Writer property of type Single to bytes by using pointer opertations
        /// </summary>
        public static unsafe void Include(float value, byte[] bytes, ref int index)
        {
            fixed (byte* b = &bytes[index])
                *((float*)b) = value;
            index += 4;
        }
        /// <summary>
        /// Writer property of type Double to bytes by using pointer opertations
        /// </summary>
        public static unsafe void Include(double value, byte[] bytes, ref int index)
        {
            fixed (byte* b = &bytes[index])
                *((double*)b) = value;
            index += 8;
        }
        /// <summary>
        /// Writer property of type Int64 to bytes by using pointer opertations
        /// </summary>
        public static unsafe void Include(long value, byte[] bytes, ref int index)
        {
            fixed (byte* b = &bytes[index])
                *((long*)b) = value;
            index += 8;
        }
        /// <summary>
        /// Writer property of type UInt64 to bytes by using pointer opertations
        /// </summary>
        public static unsafe void Include(ulong value, byte[] bytes, ref int index)
        {
            fixed (byte* b = &bytes[index])
                *((ulong*)b) = value;
            index += 8;
        }

        /// <summary>
        /// Serialize string to byte array
        /// </summary>
        public static void Include(string value, byte[] bytes, ref int index)
        {
            Include((ushort)(value?.Length ?? 0), bytes, ref index);
            if (value == null)
                return;

            Buffer.BlockCopy(Encoder.GetBytes(value), 0, bytes, index, value.Length);
            index += value.Length;
        }
        
        /// <summary>
        /// Convert bytes at given position to Int16 and increment index
        /// </summary>
        public static short ToInt16(byte[] bytes, ref int index)
        {
            var value = BitConverter.ToInt16(bytes, index);
            index += 2;
            return value;
        }

        /// <summary>
        /// Convert bytes at given position to Int16 and increment index
        /// </summary>
        public static ushort ToUInt16(byte[] bytes, ref int index)
        {
            var value = BitConverter.ToUInt16(bytes, index);
            index += 2;
            return value;
        }

        /// <summary>
        /// Convert bytes at given position to Int16 and increment index
        /// </summary>
        public static int ToInt32(byte[] bytes, ref int index)
        {
            var value = BitConverter.ToInt32(bytes, index);
            index += 4;
            return value;
        }

        /// <summary>
        /// Convert bytes at given position to Int16 and increment index
        /// </summary>
        public static uint ToUInt32(byte[] bytes, ref int index)
        {
            var value = BitConverter.ToUInt32(bytes, index);
            index += 4;
            return value;
        }

        /// <summary>
        /// Convert bytes at given position to Int16 and increment index
        /// </summary>
        public static float ToSingle(byte[] bytes, ref int index)
        {
            var value = BitConverter.ToSingle(bytes, index);
            index += 4;
            return value;
        }

        /// <summary>
        /// Convert bytes at given position to Int16 and increment index
        /// </summary>
        public static long ToInt64(byte[] bytes, ref int index)
        {
            var value = BitConverter.ToInt64(bytes, index);
            index += 8;
            return value;
        }

        /// <summary>
        /// Convert bytes at given position to Int16 and increment index
        /// </summary>
        public static ulong ToUInt64(byte[] bytes, ref int index)
        {
            var value = BitConverter.ToUInt64(bytes, index);
            index += 8;
            return value;
        }

        /// <summary>
        /// Convert bytes at given position to Int16 and increment index
        /// </summary>
        public static double ToDouble(byte[] bytes, ref int index)
        {
            var value = BitConverter.ToDouble(bytes, index);
            index += 8;
            return value;
        }

        /// <summary>
        /// Deserialize string from byte array
        /// </summary>
        public static string GetString(byte[] bytes, ref int index)
        {
            var namesLength = BitConverter.ToUInt16(bytes, index);
            index += 2;
            if (namesLength == 0)
                return string.Empty;

            var value = Encoder.GetString(bytes, index, namesLength);
            index += namesLength;
            return value;
        }
    }
}
