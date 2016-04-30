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
            fixed (byte* b = bytes)
                *((short*)(b + index)) = value;
            index += 2;
        }
        /// <summary>
        /// Writer property of type UInt16 to bytes by using pointer opertations
        /// </summary>
        public static unsafe void Include(ushort value, byte[] bytes, ref int index)
        {
            fixed (byte* b = bytes)
                *((ushort*)(b + index)) = value;
            index += 2;
        }
        /// <summary>
        /// Writer property of type Int32 to bytes by using pointer opertations
        /// </summary>
        public static unsafe void Include(int value, byte[] bytes, ref int index)
        {
            fixed (byte* b = bytes)
                *((int*)(b + index)) = value;
            index += 4;
        }
        /// <summary>
        /// Writer property of type UInt32 to bytes by using pointer opertations
        /// </summary>
        public static unsafe void Include(uint value, byte[] bytes, ref int index)
        {
            fixed (byte* b = bytes)
                *((uint*)(b + index)) = value;
            index += 4;
        }
        /// <summary>
        /// Writer property of type Single to bytes by using pointer opertations
        /// </summary>
        public static unsafe void Include(float value, byte[] bytes, ref int index)
        {
            fixed (byte* b = bytes)
                *((float*)(b + index)) = value;
            index += 4;
        }
        /// <summary>
        /// Writer property of type Double to bytes by using pointer opertations
        /// </summary>
        public static unsafe void Include(double value, byte[] bytes, ref int index)
        {
            fixed (byte* b = bytes)
                *((double*)(b + index)) = value;
            index += 8;
        }
        /// <summary>
        /// Writer property of type Int64 to bytes by using pointer opertations
        /// </summary>
        public static unsafe void Include(long value, byte[] bytes, ref int index)
        {
            fixed (byte* b = bytes)
                *((long*)(b + index)) = value;
            index += 8;
        }
        /// <summary>
        /// Writer property of type UInt64 to bytes by using pointer opertations
        /// </summary>
        public static unsafe void Include(ulong value, byte[] bytes, ref int index)
        {
            fixed (byte* b = bytes)
                *((ulong*)(b + index)) = value;
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
