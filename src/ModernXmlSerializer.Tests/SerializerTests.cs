using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModernXmlSerializer.Resources.RevChip;
using System;

namespace ModernXmlClient.Tests
{

    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class SerializerTests
    {

        /// <summary>
        /// 
        /// </summary>
        public TestContext TestContext { get; set; }

        #region ConnectRequest

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void ConnectRequest_Serialize()
        {
            var connect = new ConnectRequest("1234567890", "999-999-9999");
            var result = connect.SerializeToXml();
            result.Should().Be("<req><api>Connect</api><key>1234567890</key><posserial>999-999-9999</posserial></req>");
            var result2 = connect.SerializeToXml();
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void ConnectRequest_Deserialize()
        {
            var response = "<req><api>Connect</api><key>10987654321</key><posserial>111-111-1111</posserial></req>";
            var result = response.DeserializeFromXml<ConnectRequest>();
            result.Api.Should().Be("Connect");
            result.Key.Should().Be("10987654321");
            result.SerialNumber.Should().Be("111-111-1111");
        }

        #endregion

    }

}