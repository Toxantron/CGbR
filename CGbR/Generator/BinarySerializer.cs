using System.Linq;
using System.Runtime.Serialization;
using CGbR.Generator;
using System.Collections.Generic;

namespace CGbR
{
    /// <summary>
    /// Serializer that writes to/reads from a byte array
    /// </summary>
    internal class BinarySerializer : ILocalGenerator
    {
        private readonly BinarySerializerGenerator _template = new BinarySerializerGenerator();

        /// <seealso cref="ILocalGenerator"/>
        public string Name { get; } = "BinarySerializer";

        /// <seealso cref="ILocalGenerator"/>
        public string[] Usings { get; } = new[]
        {
            "System",
        };

        /// <seealso cref="ILocalGenerator"/>
        public bool CanExtend(ClassModel model)
        {
            return model.Attributes.Any(att => att.Name + "Attribute" == nameof(DataContractAttribute));
        }

        /// <seealso cref="ILocalGenerator"/>
        public string Extend(ClassModel model)
        {
            _template.Session = new Dictionary<string, object>
            {
                { "Model", model }
            };
            _template.Initialize();
            return _template.TransformText();
        }
    }
}