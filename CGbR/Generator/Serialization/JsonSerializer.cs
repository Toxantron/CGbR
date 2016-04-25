using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CGbR
{
    /// <summary>
    /// Generator for JSON serialization
    /// </summary>
    internal class JsonSerializer : ILocalGenerator
    {
        /// <see cref="ILocalGenerator"/>
        public string Name { get; } = nameof(JsonSerializer);

        /// <see cref="ILocalGenerator"/>
        public string[] Usings { get; } = 
        {
            "System",
            "System.Collections.Generic",
            "System.IO",
            "System.Globalization",
            "System.Text",
            "Newtonsoft.Json"
        };

        /// <seealso cref="ILocalGenerator"/>
        public string[] Interfaces { get; } = new string[0];

        /// <see cref="ILocalGenerator"/>
        public bool CanExtend(ClassModel model)
        {
            return model.IsPartial && model.HasAttribute(nameof(DataContractAttribute));
        }

        /// <see cref="ILocalGenerator"/>
        public string Extend(ClassModel model)
        {
            var template = new JsonSerializerGenerator();
            template.Session = new Dictionary<string, object>
            {
                { "Model", model }
            };
            template.Initialize();
            return template.TransformText();
        }
    }
}