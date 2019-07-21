using System.Xml.Serialization;

namespace ModernXmlSerializer.Resources.RevChip
{

    /// <summary>
    /// Represents the data required to connect to a RevChip device.
    /// </summary>
    [XmlRoot("req", Namespace = "", IsNullable = false)]
    public class ConnectRequest : RequestBase
    {

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        [XmlElement(ElementName = "posserial", Order = 2)]
        public string SerialNumber { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public ConnectRequest()
        {
            Api = "Connect";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <param name="key"></param>
        public ConnectRequest(string key, string serialNumber) : this()
        {
            Key = key;
            SerialNumber = serialNumber;
        }

        #endregion

    }

}