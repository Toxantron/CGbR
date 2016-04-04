using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CGbR.GeneratorTests
{
    [DataContract]
    public partial class Root
    {
        [DataMember]
        public int Number { get; set; }

        [DataMember]
        public Partial[] Partials { get; set; }

        [DataMember]
        public IList<ulong> Numbers { get; set; }
    }
}
