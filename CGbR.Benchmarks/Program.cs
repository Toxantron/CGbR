using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CGbR.Benchmarks
{
    internal class Program
    {
        private static void Main()
        {
            var testObject = GenerateBigObject();

            BenchmarkJson(testObject);

            Console.ReadLine();
        }

        /// <summary>
        /// Generate a big object for the benchmark
        /// </summary>
        private static Root GenerateBigObject()
        {
            var root = new Root
            {
                Number = 1228612,
                Price = 691861841.12412,
                SmallNumber = 17,
                Description = "Höajäineväiownväwevni iwvöqwbvoibvowiv",
                PartialsArray = new Partial[30],
                PartialsList = new List<Partial>()
            };

            for (var i = 0; i < 30; i++)
            {
                var partial = new Partial
                {
                    Id = 67571274172,
                    Price = (float)660412.1212,
                    Name = "jLBVWUBVOWBVWEVBBVWEV",
                    DecimalNumbers = new List<double>()
                };
                var numbers = new List<ulong>();
                for (var j = 0; j < 10; j++)
                {
                    numbers.Add(818616126518619);
                    partial.DecimalNumbers.Add(1724182754.125125);
                }
                partial.SomeNumbers = numbers;

                root.PartialsArray[i] = partial;
                root.PartialsList.Add(partial);
            }

            return root;
        }

        private static void BenchmarkJson(Root testObject)
        {
            // Run once for the JIT
            var json = JsonConvert.SerializeObject(testObject);
            json = testObject.ToJson();
            var deserialized = JsonConvert.DeserializeObject<Root>(json);
            deserialized = new Root().FromJson(json);

            var watch = new Stopwatch();
            // Classic json
            Console.WriteLine("Reflection json");
            watch.Start();
            json = JsonConvert.SerializeObject(testObject);
            watch.Stop();
            Console.WriteLine("Serialize: {0:F3}ms", watch.Elapsed.TotalMilliseconds);

            watch.Restart();
            deserialized = JsonConvert.DeserializeObject<Root>(json);
            watch.Stop();
            Console.WriteLine("Deserialize: {0:F3}ms", watch.Elapsed.TotalMilliseconds);
            Console.WriteLine("{0}", deserialized.PartialsArray.Length);

            // Generated json
            Console.WriteLine("Generated json");
            watch.Restart();
            json = testObject.ToJson();
            watch.Stop();
            Console.WriteLine("Serialize: {0:F3}ms", watch.Elapsed.TotalMilliseconds);

            watch.Restart();
            deserialized =new Root().FromJson(json);
            watch.Stop();
            Console.WriteLine("Deserialize: {0:F3}ms", watch.Elapsed.TotalMilliseconds);
            Console.WriteLine("{0}", deserialized.PartialsArray.Length);
        }
    }
}
