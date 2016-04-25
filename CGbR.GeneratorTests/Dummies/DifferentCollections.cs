using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CGbR.GeneratorTests
{
    [DataContract]
    public partial class DifferentCollections
    {
        [DataMember]
        public IEnumerable<int> Integers { get; set; }

        [DataMember]
        public List<double> Doubles { get; set; }

        [DataMember]
        public long[] Longs { get; set; }

        [DataMember]
        public uint[,] MultiDimension { get; set; }

        [DataMember]
        public ICollection<string> Names { get; set; }
    }
}