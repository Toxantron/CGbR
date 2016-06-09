# CGbR - Code dynamic, run static

[![Join the chat at https://gitter.im/Toxantron/CGbR](https://badges.gitter.im/Toxantron/CGbR.svg)](https://gitter.im/Toxantron/CGbR?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)
[![NuGet](https://img.shields.io/nuget/v/CGbR.svg)](https://www.nuget.org/packages/CGbR/)
[![Build status](https://ci.appveyor.com/api/projects/status/9lbxnkji7ifaws2k?svg=true)](https://ci.appveyor.com/project/Toxantron/cgbr)
[![Coverage Status](https://coveralls.io/repos/github/Toxantron/CGbR/badge.svg?branch=master)](https://coveralls.io/github/Toxantron/CGbR?branch=master)
[![license](https://img.shields.io/github/license/mashape/apistatus.svg?maxAge=2592000)](https://github.com/Toxantron/CGbR/blob/master/LICENSE)

A lot of frameworks and libraries in .NET use reflection to find classes, look for attributes, read properties etc. 
and with no doubt reflection is a powerful tool. It does however come at the price of performance. On the other hand
static C# performs incredibly well when it can be fully compiled but no one really wants to take the time of writing
fast boilerplate code over and over again. 

The idea of CGbR (**C**ode **G**enerator **b**eats **R**eflection) is to combine these two offering a reflection similar API but 
performing dynamic parsing operations in the pre-build stage. Using C# partial classes it generates a file _<file_name>.Generated.cs_ 
that contains performance optimized non-dynamic C# methods and properties based on attributes and interfaces defined in the original
class. This makes it the perfect choice for performance critical applications, limited hardware capabilties and embedded projects with AOT compilation.

**Quick links:**

1. [Modes of operation](#modes-of-operation)
2. [Serialization](#serialization)
  * [Binary](#binary-datacontract-serializer)
  * [JSON](#json-datacontract-serializer)
3. [Cloneable](#cloneable)
4. [Dependency Injection](#dependency-injection)
5. [Generated UI](#generated-ui)
6. [Developers](#developers)

## Modes of operations
The tool supports 3 modes of operation. It can run on a single file or a project/solution directory. The first one is meant
to be used within VisualStudio as a custom tool for a single file and the others are used as pre-build events or build
targets. Choice is made automatically based on the first argument. For filepathes the file mode is chosen and the others look for a 
".csproj" or ".sln" file in the given path.

### File Mode
In the File Mode CGbR operators on a single file only. Instead of a configuration it requires a couple of arguments. The first
argument is obviously the file. Next cames the name of the parser and all following arguments are interpreted as generator names.
This might look like this: `$ cgbr.exe Messages/MyMessage.cs Regex BinarySerializer`

### Project mode
In the Project Mode CGbR operates on the entire directory recursively. Parsers and Generators are selected by a `cgbr.json` config file with the following structure. This is the default mode activated by adding the nuget package. With each build the generated files are created and must be included into the project.

```json
{
  "Enabled": true,
  "Mappings": [
    {
      "Extension": ".cs",
      "Parser": "Regex"
    }
  ],
  "LocalGenerators": [
    {
      "Name": "BinarySerializer",
      "IsEnabled": true
    },
    {
      "Name": "JsonSerializer",
      "IsEnabled": true
    }
  ],
  "GlobalGenerators": [

  ]
}
```

## Serialization
The perfect usage scenario and actually the origin of CGbR is serializing and deserializing objects. In the original
project performance gains from generated static code over the original reflection API were somewhere between factor of
100 and 700.
Sample code can be found in the [Generator tests](https://github.com/Toxantron/CGbR/tree/master/CGbR.GeneratorTests)
and you will also find [benchmarks](https://github.com/Toxantron/CGbR-Benchmarks) comparing the different serializers.

### Binary DataContract Serializer
The binary DataContract serializer target generates code that maps single objects or object structure onto byte arrays.
It has literally zero overhead by using the class definition as a scheme to determine which byte represents which property.
The code was optimized over several iterations and will create serialize/deserialize objects of binary size of around 1500 
bytes in a matter of less then 30 micro seconds.

**Concept:**
Consider the following classes input for the serializer
```c#
[DataContract]
public partial class Root
{
	[DataMember]
	public int Id { get; set; }
	
	[DataMember]
	public ushort Number { get; set; }
	
	[DataMember]
	public Partial[] Partials { get; set; }
	
	
	[DataMember]
	public double Price { get; set; }
}

[DataContract]
public partial class Partial
{
	[DataMember]
	public byte Index { get; set; }
	
	[DataMember]
	public long BigValue { get; set; }
}
```

The resulting array would look like this:

| Position | Property    | Length |
| -------- | ----------- |--------|
| 0 - 3    | Id     | 4 |
| 4 - 5    | Number | 2 |
| 6 - 7    | Partials.Length | 2 |
| 8        | Partial[0].Index | 1 |
| 9 - 16   | Partial[0].BigValue | 8 |
| 17       | Partial[1].Index | 1 |
| 18 - 25  | Partial[1].BigValue | 8 |
| ...      | ... | |
| 8 + n*9  | Price | 8 |

### JSON DataContract Serializer
Another serializer is the JSON serializer. It is not build from scratch but rather builds on the popular [Json.NET](http://www.newtonsoft.com/json)
from Newtonsoft. While writing JSON is done directly it uses JsonReader classes to parse the string. It replaces the reflection 
serializer classes with generated serialize and deserialize methods. Please refer to the [sample code](https://github.com/Toxantron/CGbR/tree/master/CGbR.GeneratorTests)
and [benchmarks](https://github.com/Toxantron/CGbR-Benchmarks) for further information.

## Cloneable
CGbR can generate methods to create a deep or shallow copy of an object. After adding the nuget package you need the following config:

```json
{
  "Enabled": true,
  "Mappings": [
    {
      "Extension": ".cs",
      "Parser": "Regex"
    }
  ],
  "LocalGenerators": [
    {
      "Name": "Cloneable",
      "IsEnabled": true
    }
  ],
  "GlobalGenerators": [
  ]
}
```

For every partial class that implements `ICloneable` interface a partial class is generated with a `Clone(bool deep)` method. In order to work you class needs an empty default constructor. Because the partial class has full access, it can be a private constructor.

```c#
public partial class Root : ICloneable
{
    public Root(int number)
    {
        _number = number;
    }
    private int _number;

    public Partial[] Partials { get; set; }

    public IList<ulong> Numbers { get; set; }

    public object Clone()
    {
        return Clone(true);
    }

    private Root()
    {
    }
} 

public partial class Root
{
    public Root Clone(bool deep)
    {
        var copy = new Root();
        // All value types can be simply copied
        copy._number = _number; 
        if (deep)
        {
            // In a deep clone the references are cloned 
            var tempPartials = new Partial[Partials.Length];
            for (var i = 0; i < Partials.Length; i++)
            {
                var value = Partials[i];
                value = value.Clone(true);
                tempPartials[i] = value;
            }
            copy.Partials = tempPartials;
            var tempNumbers = new List<ulong>(Numbers.Count);
            for (var i = 0; i < Numbers.Count; i++)
            {
                var value = Numbers[i];
                tempNumbers.Add(value);
            }
            copy.Numbers = tempNumbers;
        }
        else
        {
            // In a shallow clone only references are copied
            copy.Partials = Partials; 
            copy.Numbers = Numbers; 
        }
        return copy;
    }
}
```

## Dependency Injection
CGbR can also be used to generate dependency injection.

## Generated UI
CGbR could also be used to generate XAML or Forms based on class definitions.

## Developers
Detailed guides on how to write custom generators follow soon.
