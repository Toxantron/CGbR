using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CGbR.GeneratorTests
{
    [DataContract]
    public partial class Root : ICloneable
    {
        public Root(int number)
        {
            _number = number;
        }

        [DataMember]
        private int _number;

        [DataMember]
        public Partial[] Partials { get; set; }

        [DataMember]
        public IList<ulong> Numbers { get; set; }

        public object Clone()
        {
            return Clone(true);
        }

        private Root()
        {
        }




        public int Number { get { return _number; } set { _number = value; } }
    }
}
