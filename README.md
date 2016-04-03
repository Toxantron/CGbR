# CGbR - Code dynamic, run static
A lot of frameworks and libraries in .NET use reflection to find classes, look for attributes, read properties etc. 
and with no doubt reflection is a powerful tool. It does however come at the price of performance. On the other hand
static C# performs incredibly well when it can be fully compiled but no one really wants to take the time of writing
fast boilerplate code over and over again. 

The idea of CGbR (*C*ode *G*enerator *b*eats *R*eflectin) is to combine these two offering a reflection similar API but 
performing dynamic parsing operations in the pre-build stage. Using C# partial classes it generates a file _<file_name>.Generated.cs_ 
that contains performance optimized non-dynamic C# methods and properties based on attributes and interfaces defined in the original
class. 

## Serialization
The perfect usage scenario and actually the origin of CGbR is serializing and deserializing objects. In the original
project performance gains from generated static code over the original reflection API were somewhere between factor of
100 and 700.

### Binary DataContract Serializer
The binary DataContract serializer target generates code that maps single objects or object structure onto byte arrays.
It has literally zero overhead by using the class definition as a scheme to determine which byte represents which property.
The code was optimized over several iterations and will create serialize/deserialize objects of binary size of around 1500 
bytes in a matter of less then 30 micro seconds.