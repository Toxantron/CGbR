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
        public static unsafe void Include(short value, byte[] bytes, int index)
        {
            fixed (byte* b = &bytes[index])
                *((short*)b) = value;
        }
        /// <summary>
        /// Writer property of type UInt16 to bytes by using pointer opertations
        /// </summary>
        public static unsafe void Include(ushort value, byte[] bytes, int index)
        {
            fixed (byte* b = &bytes[index])
                *((ushort*)b) = value;
        }
        /// <summary>
        /// Writer property of type Int32 to bytes by using pointer opertations
        /// </summary>
        public static unsafe void Include(int value, byte[] bytes, int index)
        {
            fixed (byte* b = &bytes[index])
                *((int*)b) = value;
        }
        /// <summary>
        /// Writer property of type UInt32 to bytes by using pointer opertations
        /// </summary>
        public static unsafe void Include(uint value, byte[] bytes, int index)
        {
            fixed (byte* b = &bytes[index])
                *((uint*)b) = value;
        }
        /// <summary>
        /// Writer property of type Single to bytes by using pointer opertations
        /// </summary>
        public static unsafe void Include(float value, byte[] bytes, int index)
        {
            fixed (byte* b = &bytes[index])
                *((float*)b) = value;
        }
        /// <summary>
        /// Writer property of type Double to bytes by using pointer opertations
        /// </summary>
        public static unsafe void Include(double value, byte[] bytes, int index)
        {
            fixed (byte* b = &bytes[index])
                *((double*)b) = value;
        }
        /// <summary>
        /// Writer property of type Int64 to bytes by using pointer opertations
        /// </summary>
        public static unsafe void Include(long value, byte[] bytes, int index)
        {
            fixed (byte* b = &bytes[index])
                *((long*)b) = value;
        }
        /// <summary>
        /// Writer property of type UInt64 to bytes by using pointer opertations
        /// </summary>
        public static unsafe void Include(ulong value, byte[] bytes, int index)
        {
            fixed (byte* b = &bytes[index])
                *((ulong*)b) = value;
        }

        /// <summary>
        /// Serialize string to byte array
        /// </summary>
        public static void Include(string value, byte[] bytes, ref int index)
        {
            Include((ushort)(value?.Length ?? 0), bytes, index);
            index += 2;
            if (value == null)
                return;

            Buffer.BlockCopy(Encoder.GetBytes(value), 0, bytes, index, value.Length);
            index += value.Length;
        }
        
        /// <summary>
        /// Convert bytes at given position to Int16 and increment index
        /// <summary>
        public static unsafe short ToInt16(byte[] value, ref int index) 
        {
            fixed( byte * pbyte = &value[index]) 
            {
                if( index % 2 == 0) 
                { // data is aligned 
                    return *((short *) pbyte);
                }
                else 
                {
                    if( IsLittleEndian) 
                    { 
                        return (short)((*pbyte) | (*(pbyte + 1) << 8)) ;
                    }
                    else 
                    {
                        return (short)((*pbyte << 8) | (*(pbyte + 1)));                        
                    }
                }
            }
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
