using System;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;

namespace CGbR.Benchmarks
{
    public class BigObjectBenchmark
    {
        internal static void Run()
        {
            var testObject = GenerateBigObject();

            // Run the JSON benchmark
            var sizeA = BenchmarkJson(testObject);
            var sizeB = BenchmarkBinary(testObject);

            if (sizeA == sizeB)
                Console.WriteLine("Cycle successful!");
        }

        /// <summary>
        /// Generate a big object for the benchmark
        /// </summary>
        private static Root GenerateBigObject()
        {
            var random = new Random();

            var root = new Root
            {
                Number = random.Next(),
                Price = random.Next() / (double)random.Next(),
                SmallNumber = (ushort)random.Next(ushort.MaxValue - 1),
                Description = " whägwbvwoibv    wivb  oovwwweenq ponqnv",
                PartialsArray = new Partial[30],
                PartialsList = new List<Partial>()
            };

            for (var i = 0; i < 30; i++)
            {
                var partial = new Partial
                {
                    Id = random.Next(),
                    Price = random.Next() / (float)random.Next(),
                    Name = "jLBVWUBVOWBVWEVB1zh7771h31p9BVWEV",
                    DecimalNumbers = new List<double>()
                };
                var numbers = new List<ulong>();
                for (var j = 0; j < 10; j++)
                {
                    numbers.Add((ushort)(random.Next() * 3));
                    partial.DecimalNumbers.Add(random.Next() / (double)random.Next());
                }
                partial.SomeNumbers = numbers;

                root.PartialsArray[i] = partial;
                root.PartialsList.Add(partial);
            }

            return root;
        }

        private static int BenchmarkJson(Root testObject)
        {
            // Run once for the JIT
            var json = JsonConvert.SerializeObject(testObject);
            var deserialized = JsonConvert.DeserializeObject<Root>(json);
            json = deserialized.ToJson();
            deserialized = new Root().FromJson(json);

            // Test size
            Console.WriteLine("JSON Benchmark:");
            Console.WriteLine("----------------------");
            Console.WriteLine("String size: {0}", json.Length);

            var watch = new Stopwatch();
            // Classic json
            Console.WriteLine("Reflection json");
            watch.Start();
            json = JsonConvert.SerializeObject(deserialized);
            watch.Stop();
            Console.WriteLine("Serialize: {0:F3}ms", watch.Elapsed.TotalMilliseconds);

            watch.Restart();
            deserialized = JsonConvert.DeserializeObject<Root>(json);
            watch.Stop();
            Console.WriteLine("Deserialize: {0:F3}ms", watch.Elapsed.TotalMilliseconds);

            // Generated json
            Console.WriteLine("Generated json");
            watch.Restart();
            json = deserialized.ToJson();
            watch.Stop();
            Console.WriteLine("Serialize: {0:F3}ms", watch.Elapsed.TotalMilliseconds);

            watch.Restart();
            deserialized = new Root().FromJson(json);
            watch.Stop();
            Console.WriteLine("Deserialize: {0:F3}ms", watch.Elapsed.TotalMilliseconds);

            Console.WriteLine();
            Console.WriteLine();

            return deserialized.PartialsArray.Length;
        }

        /// <summary>
        /// Benmark the speed of the binary serializer
        /// </summary>
        /// <param name="testObject"></param>
        private static int BenchmarkBinary(Root testObject)
        {
            // Run once for the JIT
            var bytes = testObject.ToBytes();
            var deserialized = new Root().FromBytes(bytes);

            // Run benchmark
            Console.WriteLine("Binary benchmark:");
            Console.WriteLine("----------------------");
            Console.WriteLine("Binary size: {0}", bytes.Length);

            var watch = new Stopwatch();
            // Serialize to bytes
            watch.Start();
            bytes = deserialized.ToBytes();
            watch.Stop();
            Console.WriteLine("Serialize: {0:F3}ms", watch.Elapsed.TotalMilliseconds);

            // Serialize from bytes
            watch.Restart();
            deserialized = deserialized.FromBytes(bytes);
            watch.Stop();
            Console.WriteLine("Deserialize: {0:F3}ms", watch.Elapsed.TotalMilliseconds);

            return deserialized.PartialsArray.Length;
        }
    }
}