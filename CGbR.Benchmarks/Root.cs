using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CGbR.Benchmarks
{
    [DataContract]
    public partial class Root
    {
        [DataMember]
        public int Number { get; set; }  
        
        [DataMember]
        public double Price { get; set; }  

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public List<Partial> PartialsList { get; set; }

        [DataMember]
        public Partial[] PartialsArray { get; set; }

        [DataMember]
        public ushort SmallNumber { get; set; }
    }
}