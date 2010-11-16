using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace TagMp3Saito.ReflectHelp
{
    internal class Serializer
    {
        /// <summary>
        /// To convert a Byte Array of Unicode values (UTF-8 encoded) to a complete String.
        /// </summary>
        /// <param name="characters">Unicode Byte Array to be converted to String</param>
        /// <returns>String converted from Unicode Byte Array</returns>
        private static string UTF8ByteArrayToString(byte[] characters)
        {
            var encoding = new UTF8Encoding();
            string constructedString = encoding.GetString(characters);
            return (constructedString);
        }

        /// <summary>
        /// Converts the String to UTF8 Byte array and is used in De serialization
        /// </summary>
        /// <param name="pXmlString"></param>
        /// <returns></returns>
        private static byte[] StringToUTF8ByteArray(string pXmlString)
        {
            var encoding = new UTF8Encoding();
            byte[] byteArray = encoding.GetBytes(pXmlString);
            return byteArray;
        }

        /// <summary>
        /// Serialize an object into an XML string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeObject<T>(T obj)
        {
            string xmlString = null;
            var memoryStream = new MemoryStream();
            var xs = new XmlSerializer(typeof (T));
            var xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
            xs.Serialize(xmlTextWriter, obj);
            memoryStream = (MemoryStream) xmlTextWriter.BaseStream;
            xmlString = UTF8ByteArrayToString(memoryStream.ToArray());
            return xmlString;
        }

        /// <summary>
        /// Reconstruct an object from an XML string
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T DeserializeObject<T>(string xml)
        {
            var xs = new XmlSerializer(typeof (T));
            var memoryStream = new MemoryStream(StringToUTF8ByteArray(xml));
            var xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
            return (T) xs.Deserialize(memoryStream);
        }
    }
}