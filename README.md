# CGbR - Code dynamic, run static
A lot of frameworks and libraries in .NET use reflection to find classes, look for attributes, read properties etc. 
and with no doubt reflection is a powerful tool. It does however come at the price of performance. On the other hand
static C# performs incredibly well when it can be fully compiled but no one really wants to take the time of writing
fast boilerplate code over and over again. 

The idea of CGbR (**C**ode **G**enerator **b**eats **R**eflection) is to combine these two offering a reflection similar API but 
performing dynamic parsing operations in the pre-build stage. Using C# partial classes it generates a file _<file_name>.Generated.cs_ 
that contains performance optimized non-dynamic C# methods and properties based on attributes and interfaces defined in the original
class. 

**Quick links:**

1. [Modes of operation](#modes-of-operation)
2. [Serialization](#serialization)
  * [Binary](#binary-datacontract-serializer)
  * [JSON](#json-datacontract-serializer)
3. [Dependency Injection](#dependency-injection)
4. [Developers](#developers)

## Modes of operations
The tool supports 3 modes of operation. It can run on a single file or a project/solution directory. The first one is meant
to be used within VisualStudio as a custom tool for a single file and the others are used as pre-build events or build
targets. Choice is made automatically based on the first argument. For files of type ".cs" and the others look for a 
".csproj" or ".sln" file in the given path.

## Serialization
The perfect usage scenario and actually the origin of CGbR is serializing and deserializing objects. In the original
project performance gains from generated static code over the original reflection API were somewhere between factor of
100 and 700.

### Binary DataContract Serializer
The binary DataContract serializer target generates code that maps single objects or object structure onto byte arrays.
It has literally zero overhead by using the class definition as a scheme to determine which byte represents which property.
The code was optimized over several iterations and will create serialize/deserialize objects of binary size of around 1500 
bytes in a matter of less then 30 micro seconds.

**Concept:**
Consider the following classes input for the serializer
```c#
[DataContract]
public class Root
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
public class Partial
{
	[DataMember]
	public byte Index { get; set; }
	
	[DataMember]
	public long BigValue { get; set; }
}
```

The resulting array would look like this:

| Position | Property    |
| -------- | ----------- |
| 0 - 3    | Id     |
| 4 - 5    | Number |
| 6 - 7    | Partials.Length |
| 8        | Partial[0].Index |
| 9 - 16   | Partial[0].BigValue |
| 17       | Partial[1].Index |
| 18 - 25  | Partial[1].BigValue |
| ...      | ... |
| 8 + n*9  | Price |

### JSON DataContract Serializer
Another serializer is the JSON serializer. It is not build from scratch but rather builds on the popular [Json.NET](http://www.newtonsoft.com/json)
from Newtonsoft. It uses the internal JsonWriter and JsonReader classes to write the string.It just replaces the reflection 
serializer classes with generated serialize and deserialize methods.

Class definition with DataMember as usual:
```c#
[DataContract]
public partial class Root
{
    [DataMember]
    public int Number { get; set; }

    [DataMember]
    public Partial[] Partials { get; set; }

    [DataMember]
    public ulong[] Numbers { get; set; }
}

[DataContract]
public partial class Partial
{
    [DataMember]
    public short Id { get; set; }
}
```

Generates the following code:
```c#
// In Root.Generated.cs
public void IncludeJson(JsonWriter writer)
{
    writer.WriteStartObject();

    writer.WritePropertyName("Number");
    writer.WriteValue(Number);
    
    writer.WritePropertyName("Partials");
    if (Partials == null)
        writer.WriteNull();
    else
    {
        writer.WriteStartArray();
        for (var i = 0; i < Partials.Length; i++)
        {
            Partials[i].IncludeJson(writer);
        }
        writer.WriteEndArray();
    }
    
    writer.WritePropertyName("Numbers");
    if (Numbers == null)
        writer.WriteNull();
    else
    {
        writer.WriteStartArray();
        for (var i = 0; i < Numbers.Length; i++)
        {
            writer.WriteValue(Numbers[i]);
        }
        writer.WriteEndArray();
    }
    
    writer.WriteEndObject();
}
public Root FromJson(JsonReader reader)
{
    while (reader.Read())
    {
        // Break on EndObject
        if (reader.TokenType == JsonToken.EndObject)
            break;

        // Only look for properties
        if (reader.TokenType != JsonToken.PropertyName)
            continue;

        switch ((string) reader.Value)
        {
            case "Number":
                reader.Read();
                Number = Convert.ToInt32(reader.Value);
                break;

            case "Partials":
				reader.Read(); // Read token where array should begin
                if (reader.TokenType == JsonToken.Null)
                    break;
                var partials = new List<Partial>();
                while (reader.Read() && reader.TokenType == JsonToken.StartObject)
                    partials.Add(new Partial().FromJson(reader));
                Partials = partials.ToArray();
                break;

            case "Numbers":
				reader.Read(); // Read token where array should begin
                if (reader.TokenType == JsonToken.Null)
                    break;
                var numbers = new List<ulong>();
                while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                    numbers.Add(Convert.ToUInt64(reader.Value));
                Numbers = numbers.ToArray();
                break;

        }
    }

    return this;
}

// In Partial.Generated.cs
public void IncludeJson(JsonWriter writer)
{
    writer.WriteStartObject();

    writer.WritePropertyName("Id");
    writer.WriteValue(Id);
    
    writer.WriteEndObject();
}
public Partial FromJson(JsonReader reader)
{
    while (reader.Read())
    {
        // Break on EndObject
        if (reader.TokenType == JsonToken.EndObject)
            break;

        // Only look for properties
        if (reader.TokenType != JsonToken.PropertyName)
            continue;

        switch ((string) reader.Value)
        {
            case "Id":
                reader.Read();
                Id = Convert.ToInt16(reader.Value);
                break;

        }
    }

    return this;
}
```

## Dependency Injection
CGbR can also be used to generate dependency injection.

## Developers
Detailed guides on how to write custom generators follow soon.
