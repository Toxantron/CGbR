using System;
using System.Runtime.Serialization;

namespace CGbR.GeneratorTests
{
    [DataContract]
    public partial class Partial : ICloneable
    {
        [DataMember]
        public short Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}