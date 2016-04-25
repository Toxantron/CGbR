using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGbR.Lib;
using Newtonsoft.Json;

namespace CGbR.Benchmarks
{
    internal class Program
    {
        private static void Main()
        {
            BigObjectBenchmark.Run();

            var smallNumerics = new List<SmallNumeric>();
            for (var i = 0; i < 1000; i++)
            {
                smallNumerics.Add(new SmallNumeric {Index = (ushort)i, Value = i * 100 + i % 25});
            }

            var watch = new Stopwatch();
            watch.Start();
            var bytes = BinarySerializer.SerializeMany(smallNumerics);
            watch.Stop();

            Console.WriteLine("Serialize 1000 objects: {0}ms", watch.ElapsedMilliseconds);

            Console.ReadLine();
        }
    }
}
