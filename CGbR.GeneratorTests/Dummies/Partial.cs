using System.Runtime.Serialization;

namespace CGbR.GeneratorTests
{
[DataContract]
public partial class Partial
{
    [DataMember]
    public short Id { get; set; }
}
}