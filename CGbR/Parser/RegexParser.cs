using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace CGbR
{
    /// <summary>
    /// <see cref="IParser"/> implementation that uses only regex to extract the most basic syntax elements
    /// like properties or attributes
    /// </summary>
    internal class RegexParser : IParser
    {
        // Regex used to parse source file
        private readonly Regex _namespaceRegex = new Regex(@"namespace (?<namespace>(?:\w\.?)*)");
        private readonly Regex _attributeRegex = new Regex(@" \[(?<attributeName>\w+)\(?(?:(?<parameter>\d+),? ?)*(?:(?<property>\w+) ?= ?(?<value>\d+),? ?)*");

        private readonly Regex _classRegex = new Regex(@" (?<accessModifier>(?:public|internal))(?<isPartial> partial)? class (?<className>\w+)(?: : )?(?<baseType>[A-Z][a-z]\w+)?(?:, )?(?:(?<interface>I\w+)(?:, )?)*");
        private readonly Regex _enumRegex = new Regex(@" (?<accessModifier>(?:public|internal)) enum (?<enumName>\w+)(?: : )?(?<type>\w+)?");

        private readonly Regex _propRegex = new Regex(@" (?<accessModifier>(?:public|internal|private)) (?:(?<collectionType>\w+)<)?(?<type>\w+)(?<isArray>\[(?<dimensions>, ?)*\])?>? (?<name>_?\w+) ");
        private readonly Regex _memberRegex = new Regex(@" (?<name>\w+)(?: ?= ?)?(?<value>\d+)?");


        /// <seealso cref="IParser"/>
        public string Name { get; } = "Regex";

        /// <seealso cref="IParser"/>
        public CodeElementModel ParseFile(string filePath)
        {
            // Read the file
            var file = File.ReadAllLines(filePath);

            CodeElementModel model = null;
            var @namespace = string.Empty;
            for (var i = 0; i < file.Length; i++)
            {
                var line = file[i];

                // First find the namespace
                if (string.IsNullOrEmpty(@namespace))
                {
                    var match = _namespaceRegex.Match(line);
                    if (match.Success)
                        @namespace = match.Groups["namespace"].Value;
                }

                // First we must find the class definition
                if (model == null)
                {
                    model = ParseClass(file, i, @namespace) ?? ParseEnum(file, i, @namespace);
                    continue;
                }

                // Try to parse properties or members
                if (model is ClassModel)
                    ParseProperty(file, i, (ClassModel) model);
                else if (model is EnumModel)
                    ParseMember(file, i, (EnumModel) model);

            }

            return model;
        }

        /// <summary>
        /// Try parsing the class definition and its attributes
        /// </summary>
        /// <param name="file">Text lines of the file</param>
        /// <param name="index">Index in the file</param>
        /// <param name="namespace">Namespace of the class</param>
        private CodeElementModel ParseClass(string[] file, int index, string @namespace)
        {
            ClassModel model = null;

            // Try to match current line as class definition
            var match = _classRegex.Match(file[index]);
            if (!match.Success)
                return model;

            // Get base type and interfaces
            var baseGroup = match.Groups["baseType"];
            var baseType = baseGroup.Success ? baseGroup.Value : string.Empty;

            var interfaceGroup = match.Groups["interface"];
            var interfaces = new string[interfaceGroup.Captures.Count];
            for (var i = 0; i < interfaceGroup.Captures.Count; i++)
            {
                interfaces[i] = interfaceGroup.Captures[i].Value;
            }

            // Create class model
            model = new ClassModel(match.Groups["className"].Value)
            {
                Namespace = @namespace,
                BaseClass = baseType,
                Interfaces = interfaces,
                IsPartial = match.Groups["isPartial"].Success,
                AccessModifier = ParseAccessModifier(match.Groups["accessModifier"].Value)
            };

            // Add attributes by moving up the lines
            ParseAttributes(file, index, model);

            return model;
        }

        /// <summary>
        /// Try to parse enum from file
        /// </summary>
        /// <param name="file">Text lines of the file</param>
        /// <param name="index">Index in the file</param>
        /// <param name="namespace">Namespace of the class</param>
        private CodeElementModel ParseEnum(string[] file, int index, string @namespace)
        {
            EnumModel model = null;

            // Try to match current line as class definition
            var match = _enumRegex.Match(file[index]);
            if (!match.Success)
                return model;

            // Create class model
            var typeMatch = match.Groups["type"];
            model = new EnumModel(match.Groups["enumName"].Value)
            {
                BaseType = typeMatch.Success ? DetermineType(typeMatch.Value) : ModelValueType.Int32,
                AccessModifier = ParseAccessModifier(match.Groups["accessModifier"].Value)
            };

            // Add attributes by moving up the lines
            ParseAttributes(file, index, model);

            return model;
        }

        /// <summary>
        /// Try parsing a property from the current line
        /// </summary>
        /// <param name="file">All text lines of the file</param>
        /// <param name="index">Current index in the file</param>
        /// <param name="model">Class model</param>
        private void ParseProperty(string[] file, int index, ClassModel model)
        {
            // Try parsing property
            var match = _propRegex.Match(file[index]);
            if (!match.Success)
                return;

            // Create property model
            var type = match.Groups["type"].Value;
            var collGroup = match.Groups["collectionType"];
            var arrayGroup = match.Groups["isArray"];
            var property = new PropertyModel(match.Groups["name"].Value)
            {
                ElementType = type,
                ValueType = DetermineType(type),
                AccessModifier = ParseAccessModifier(match.Groups["accessModifier"].Value),
                IsCollection = collGroup.Success | arrayGroup.Success,
                CollectionType = arrayGroup.Success ? "Array" : collGroup.Value,
                Dimensions = match.Groups["dimensions"].Captures.Count + 1
            };

            // Add references
            if (property.ValueType == ModelValueType.Class && model.References.All(r => r.Name != type))
                model.References.Add(new ClassModel(type));

            // Add attributes by moving up the lines
            ParseAttributes(file, index, property);

            model.Properties.Add(property);
        }

        /// <summary>
        /// Parse enum member and add to the root model
        /// </summary>
        /// <param name="file">All text lines of the file</param>
        /// <param name="index">Current index in the file</param>
        /// <param name="model">Enum model</param>
        private void ParseMember(string[] file, int index, EnumModel model)
        {
            // Try parsing property
            var match = _memberRegex.Match(file[index]);
            if (!match.Success)
                return;

            // Create property model
            var valueMatch = match.Groups["value"];
            var member = new EnumMember(match.Groups["name"].Value)
            {
                Value = valueMatch.Success ? int.Parse(valueMatch.Value) : model.Members.Count + 1
            };

            // Add attributes by moving up the lines
            ParseAttributes(file, index, member);

            model.Members.Add(member);
        }

        /// <summary>
        /// Parse modifier string to enum
        /// </summary>
        /// <param name="modifier"></param>
        /// <returns></returns>
        private static AccessModifier ParseAccessModifier(string modifier)
        {
            switch (modifier)
            {
                case "private":
                    return AccessModifier.Private;
                case "internal":
                    return AccessModifier.Internal;
                case "public":
                    return AccessModifier.Public;
                default:
                    throw new ArgumentOutOfRangeException(nameof(modifier), "I have no idea how you managed this");
            }
        }

        /// <summary>
        /// Parse the enum representation of a type
        /// </summary>
        /// <param name="typeString">Type to parse</param>
        private static ModelValueType DetermineType(string typeString)
        {
            var enumType = typeof(ModelValueType);
            var values = Enum.GetNames(enumType);
            foreach (var value in values)
            {
                var member = enumType.GetMember(value);
                var attribute = member[0].GetCustomAttribute<TypeAliasAttribute>();
                if (attribute != null && attribute.Aliases.Contains(typeString))
                    return (ModelValueType)Enum.Parse(typeof(ModelValueType), value);
            }
            return ModelValueType.Class;
        }

        /// <summary>
        /// Look for attributes in the previous lines
        /// </summary>
        /// <param name="file">All lines of the fie</param>
        /// <param name="index">Current index in file</param>
        /// <param name="model">Model to add attributes to</param>
        private void ParseAttributes(string[] file, int index, CodeElementModel model)
        {
            // Keep going back in the file to look for attributes
            for (var line = index - 1; line >= 0; line--)
            {
                var match = _attributeRegex.Match(file[line]);
                if (!match.Success)
                    break;

                // Create attribute
                var attribute = new AttributeModel(match.Groups["attributeName"].Value);
                model.Attributes.Add(attribute);

                // Set parameters
                var parameterGroup = match.Groups["parameter"];
                for (int i = 0; i < parameterGroup.Captures.Count; i++)
                {
                    attribute.Parameters.Add(parameterGroup.Captures[i].Value);
                }

                // Set properties
                var propertygroup = match.Groups["property"];
                var valueGroup = match.Groups["value"];
                for (int i = 0; i < parameterGroup.Captures.Count; i++)
                {
                    var property = new PropertyModel(propertygroup.Captures[i].Value)
                    {
                        Value = valueGroup.Captures[i].Value
                    };
                    attribute.Properties.Add(property);
                }
            }
        }
    }
}
