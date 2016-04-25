using System.Runtime.Serialization;

namespace CGbR.Benchmarks
{
    [DataContract]
    public partial class SmallNumeric
    {
        [DataMember]
        public ushort Index { get; set; }

        [DataMember]
        public int Value { get; set; }
    }
}