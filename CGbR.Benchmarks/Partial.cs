using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CGbR.Benchmarks
{
    [DataContract]
    public partial class Partial
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public float Price { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public List<double> DecimalNumbers { get; set; }

        [DataMember]
        public IEnumerable<ulong> SomeNumbers { get; set; }  
    }
}