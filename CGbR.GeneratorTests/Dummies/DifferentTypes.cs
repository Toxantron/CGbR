using System;
using System.Runtime.Serialization;

namespace CGbR.GeneratorTests
{
    [DataContract]
    public partial class DifferentTypes : ICloneable
    {
        [DataMember]
        public DateTime Time { get; set; }

        [DataMember]
        public ShortEnum Short { get; set; }

        [DataMember]
        public SomeEnum Some { get; set; }

        [DataMember]
        public BigEnum Big { get; set; }

        public object Clone()
        {
            return Clone(true);
        }
    }
}