using BenchmarkDotNet.Running;
using System;

namespace ModernXmlSerializer.Benchmarks
{

    /// <summary>
    /// 
    /// </summary>
    class Program
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
#pragma warning disable CA1801 // Remove unused parameter
        static void Main(string[] args)
#pragma warning restore CA1801 // Remove unused parameter
        {
            var summary = BenchmarkRunner.Run<SerializeBenchmarks>();
            //var summary2 = BenchmarkRunner.Run<DeserializeBenchmarks>();

        }
    }
}
