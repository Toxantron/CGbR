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
            "System.IO",
            "System.Text",
            "Newtonsoft.Json"
        };

        /// <see cref="ILocalGenerator"/>
        public bool CanExtend(ClassModel model)
        {
            return model.HasAttribute(nameof(DataContractAttribute));
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