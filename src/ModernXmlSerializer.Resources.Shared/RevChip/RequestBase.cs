using System.Xml.Serialization;

namespace ModernXmlSerializer.Resources.RevChip
{

    /// <summary>
    /// 
    /// </summary>
    public class RequestBase
    {

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        [XmlElement(ElementName = "api", Order = 0)]
        public string Api { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [XmlElement(ElementName = "key", Order = 1)]
        public string Key { get; set; }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public RequestBase()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public RequestBase(string key) : this()
        {
            Key = key;
        }

    }

}