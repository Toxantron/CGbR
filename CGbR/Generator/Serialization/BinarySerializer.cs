using System.Linq;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace CGbR
{
    /// <summary>
    /// Serializer that writes to/reads from a byte array
    /// </summary>
    internal class BinarySerializer : ILocalGenerator
    {
        /// <seealso cref="ILocalGenerator"/>
        public string Name { get; } = nameof(BinarySerializer);

        /// <seealso cref="ILocalGenerator"/>
        public string[] Usings { get; } = 
        {
            "System",
            "System.Linq",
            "CGbR.Lib"
        };

        /// <seealso cref="ILocalGenerator"/>
        public bool CanExtend(ClassModel model)
        {
            return model.IsPartial && model.HasAttribute(nameof(DataContractAttribute));
        }

        /// <seealso cref="ILocalGenerator"/>
        public string Extend(ClassModel model)
        {
            var template = new BinarySerializerGenerator();
            template.Session = new Dictionary<string, object>
            {
                { "Model", model }
            };
            template.Initialize();
            return template.TransformText();
        }
    }
}