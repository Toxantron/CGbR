using System;
using System.Runtime.Serialization;

namespace CGbR.GeneratorTests
{
    [DataContract]
    public partial class DifferentTypes
    {
        [DataMember]
        public DateTime Time { get; set; }

    }
}