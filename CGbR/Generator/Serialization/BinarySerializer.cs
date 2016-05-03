using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace CGbR
{
    /// <summary>
    /// Serializer that writes to/reads from a byte array
    /// </summary>
    internal class BinarySerializer : ILocalGenerator, IClassSerializationTools
    {
        /// <seealso cref="ILocalGenerator"/>
        public string Name { get; } = nameof(BinarySerializer);

        /// <seealso cref="ILocalGenerator"/>
        public string[] Usings { get; } =
        {
            "System",
            "System.Collections.Generic",
            "System.Linq",
            "System.Text",
            "CGbR.Lib"
        };

        /// <seealso cref="ILocalGenerator"/>
        public string[] Interfaces { get; } = new[]
        {
            "IByteSerializable"
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
                { "Model", model },
                { "Tools", this }
            };
            template.Initialize();
            return template.TransformText();
        }

        /// <see cref="IClassSerializationTools"/>
        int IClassSerializationTools.FixedSize(ClassModel model, PropertyModel property)
        {
            var child = GetChild(model, property);
            if (child != null)
            {
                if (child is ClassModel && !BinarySize.IsVariable((ClassModel)child))
                    return BinarySize.OfClass((ClassModel)child, this);
                if (child is EnumModel)
                    return BinarySize.OfType(((EnumModel) child).BaseType);
            }

            // Fall back to supported types
            switch (property.ElementType)
            {
                case nameof(DateTime):
                    return 8;
                default:
                    return 0;
            }
        }

        /// <see cref="IClassSerializationTools"/>
        string IClassSerializationTools.ReferenceSize(ClassModel model, PropertyModel property)
        {
            var child = GetChild(model, property);

            // Check if the class was parsed or comes from another assembly
            if (child == null)
            {
                // For now we just switch supported type names
                switch (property.ElementType)
                {
                    case nameof(DateTime):
                        return property.IsCollection ? $"{GeneratorTools.CollectionSize(property)} * 8" : null;
                    default:
                        return null;
                }
            }

            string entrySize = null;
            if (child is ClassModel)
            {
                var classChild = (ClassModel)child;
                if (property.IsCollection)
                    entrySize = BinarySize.IsVariable(classChild) ? "Sum(entry => entry.Size)" : $"{GeneratorTools.CollectionSize(property)} * {BinarySize.OfClass(classChild, this)}";
                else if (BinarySize.IsVariable(property))
                    entrySize = "Size";
                else
                    return null;
            }
            else if (child is EnumModel)
            {
                var enumChild = (EnumModel)child;
                if (property.IsCollection)
                    entrySize = $"{GeneratorTools.CollectionSize(property)} * {BinarySize.OfType(enumChild.BaseType)}";
                else
                    return null;
            }


            return entrySize;
        }

        /// <see cref="IClassSerializationTools"/>
        string IClassSerializationTools.ClassToBytes(ClassModel model, PropertyModel property)
        {
            var target = property.IsCollection ? "value" : property.Name;
            var child = GetChild(model, property);

            // Class is auto generated as well
            if (child != null)
            {
                return $"{target}.ToBytes(bytes, ref index)";
            }

            // If we couldn't find the child we are bound to the classes we support
            switch (property.ElementType)
            {
                case nameof(DateTime):
                    return $"GeneratorByteConverter.Include({target}.ToBinary(), bytes, ref index)";
                default:
                    return $"{target}.ToBytes(bytes, ref index)";
            }
        }

        /// <see cref="IClassSerializationTools"/>
        string IClassSerializationTools.ClassFromBytes(ClassModel model, PropertyModel property)
        {
            var child = GetChild(model, property);

            // Class is auto generated as well
            if (child != null)
            {
                return $"new {property.ElementType}().FromBytes(bytes, ref index)";
            }

            // If we couldn't find the child we are bound to the classes we support
            switch (property.ElementType)
            {
                case nameof(DateTime):
                    return $"DateTime.FromBinary(GeneratorByteConverter.ToInt64(bytes, ref index))";
                default:
                    return string.Empty;
            }
        }

        private static CodeElementModel GetChild(ClassModel model, PropertyModel property)
        {
            return model.References.FirstOrDefault(r => r.Name == property.ElementType);
        }
    }
}
