using Ben.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace System
{

    /// <summary>
    /// 
    /// </summary>
    public static class ObjectExtensions
    {

        #region Static Private Members

        static readonly TypeDictionary<IOrderedEnumerable<PropertyDetails>> propertyDetails;
        static readonly TypeDictionary<string> typeNames;

        #endregion

        #region Static Constructor

        static ObjectExtensions()
        {
            propertyDetails = new TypeDictionary<IOrderedEnumerable<PropertyDetails>>();
            typeNames = new TypeDictionary<string>();
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Materializes an object from an XML string, honoring any <see cref="XmlElementAttribute"/> decorations present.
        /// </summary>
        /// <typeparam name="T">The type to deserialize into.</typeparam>
        /// <param name="objectData">The serialized XML payload.</param>
        /// <returns>An instance of <typeparamref name="T"/> populated with the values from <paramref name="objectData"/>.</returns>
        public static T DeserializeFromXml<T>(this string objectData) where T : class
        {
            if (string.IsNullOrWhiteSpace(objectData)) return null;

            var result = Activator.CreateInstance<T>();
            var objectType = typeof(T);
            IOrderedEnumerable<PropertyDetails> properties;

            if (!propertyDetails.ContainsKey(objectType))
            {
               properties = objectType
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                    .Where(prop => Attribute.IsDefined(prop, typeof(XmlElementAttribute)))
                    .Select(c => new PropertyDetails { Property = c, Attribute = c.GetCustomAttribute(typeof(XmlElementAttribute)) as XmlElementAttribute })
                    .OrderBy(c => c.Attribute.Order);

                propertyDetails[objectType] = properties;
            }
            else
            {
                properties = propertyDetails[objectType];
            }

            using (var memoryStream = new MemoryStream(Encoding.Unicode.GetBytes(objectData)))
            {
                var doc = XDocument.Load(memoryStream);
                foreach (var element in doc.Root.Elements())
                {
                    if (string.IsNullOrWhiteSpace(element.Value)) continue;

                    var propertyInfo = properties.FirstOrDefault(c => c.Attribute.ElementName == element.Name.LocalName || c.Property.Name == element.Name.LocalName);
                    if (propertyInfo == null)
                    {
                        Debug.WriteLine($"ModernXmlSerializer: Could not find expected ElementName or PropertyName '{element.Name.LocalName}'");
                        continue;
                    }
                    propertyInfo.Property.SetValue(result, TypeDescriptor.GetConverter(propertyInfo.Property.PropertyType).ConvertFromInvariantString(element.Value));
                }
            }
            return result;
        }

        /// <summary>
        /// Serializes an object into XML, honoring any <see cref="XmlElementAttribute.ElementName"/>, and applying <see cref="XmlElementAttribute.Order"/> all the wsy
        /// down the inheritance hierarchy.
        /// </summary>
        /// <param name="objectInstance">The object to serialize.</param>
        /// <returns>A string containing an XML version of the object.</returns>
        public static string SerializeToXml(this object objectInstance)
        {
            if (objectInstance == null) return string.Empty;

            var objectType = objectInstance.GetType();
            IOrderedEnumerable<PropertyDetails> properties;
            string objectName = null;

            if (!propertyDetails.ContainsKey(objectType))
            {
                properties = objectType
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                    .Where(prop => Attribute.IsDefined(prop, typeof(XmlElementAttribute)))
                    .Select(c => new PropertyDetails { Property = c, Attribute = c.GetCustomAttribute(typeof(XmlElementAttribute)) as XmlElementAttribute })
                    .OrderBy(c => c.Attribute.Order);

                propertyDetails[objectType] = properties;
            }
            else
            {
                properties = propertyDetails[objectType];
            }

            if (!typeNames.ContainsKey(objectType))
            {
                objectName = objectType.GetCustomAttribute(typeof(XmlRootAttribute)) is XmlRootAttribute rootNodeAttribute ? rootNodeAttribute.ElementName : objectType.Name;
                typeNames[objectType] = objectName;
            }
            else
            {
                objectName = typeNames[objectType];
            }

            // RWM: We can't use regular XML Serialization here, as it doesn't respect XmlElementAttribute.Order down the inheritance chain.
            //      Let's fix that, because it's 2019, and we can.

            var assembliesNode = new XElement(objectName,
                from PropertyDetails prop in properties
                select new XElement(prop.Attribute.ElementName, prop.Property.GetValue(objectInstance)));

            return assembliesNode.ToString(SaveOptions.DisableFormatting);
        }

        #endregion

        internal class PropertyDetails
        {

            internal PropertyInfo Property { get; set; }

            internal XmlElementAttribute Attribute { get; set; }

        }

    }

}