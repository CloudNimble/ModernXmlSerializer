using BenchmarkDotNet.Attributes;
using System;
using System.Security.Cryptography;

namespace ModernXmlSerializer.Benchmarks
{

    /// <summary>
    /// 
    /// </summary>
    public class DeserializeBenchmarks
    {
        private const int N = 10000;
        private readonly byte[] data;

        private readonly SHA256 sha256 = SHA256.Create();
        private readonly MD5 md5 = MD5.Create();

        /// <summary>
        /// 
        /// </summary>
        public DeserializeBenchmarks()
        {
            data = new byte[N];
            new Random(42).NextBytes(data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Benchmark]
        public byte[] Sha256() => sha256.ComputeHash(data);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Benchmark]
        public byte[] Md5() => md5.ComputeHash(data);

    }

}