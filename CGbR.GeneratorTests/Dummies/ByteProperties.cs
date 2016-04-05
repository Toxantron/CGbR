using System.Runtime.Serialization;

namespace CGbR.GeneratorTests
{
    [DataContract]
    public partial class ByteProperties
    {
        [DataMember]
        public byte SingleByte { get; set; }

        [DataMember]
        public byte[] Bytes { get; set; }
    }
}