using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CGbR.GeneratorTests
{
    [DataContract]
    public partial class Test
    {
        [DataMember]
        public int Number { get; set; }
    }
}
