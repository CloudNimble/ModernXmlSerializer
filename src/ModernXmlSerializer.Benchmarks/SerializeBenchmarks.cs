using BenchmarkDotNet.Attributes;
using ModernXmlSerializer.Resources.RevChip;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace ModernXmlSerializer.Benchmarks
{

    /// <summary>
    /// 
    /// </summary>
    [ClrJob(baseline: true), CoreJob]
    [RPlotExporter, RankColumn]
    public class SerializeBenchmarks
    {

        private ConnectRequest request;

        /// <summary>
        /// 
        /// </summary>
        public SerializeBenchmarks()
        {
            request = new ConnectRequest("Testing12345", "Testing678910");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Benchmark]
        public string XmlSerializer()
        {
            using var stream = new MemoryStream();
            var xmlSerializer = new XmlSerializer(typeof(ConnectRequest));
            xmlSerializer.Serialize(stream, request);
            return Encoding.UTF8.GetString(stream.ToArray(), 0, (int)stream.Length);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Benchmark]
        public string DataContractSerializer()
        {
            using var stream = new MemoryStream();
            var dataContractSerializer = new DataContractSerializer(typeof(ConnectRequest));
            dataContractSerializer.WriteObject(stream, request);
            return Encoding.UTF8.GetString(stream.ToArray(), 0, (int)stream.Length);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Benchmark(Baseline = true)]
        public string ModernXmlSerializer() => request.SerializeToXml();

    }

}