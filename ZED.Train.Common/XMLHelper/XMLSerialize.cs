using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ZED.Train.Common
{   
    /// <summary>
    /// XML序列化类
    /// </summary>
    public class XMLSerialize<T>
    {
        private static XmlSerializer xml = null;

        public static string Serialize(T obj)
        {
            if (xml == null)
            {
                xml = new XmlSerializer(typeof(T));
            }
            using (StringWriter reader = new StringWriter())
            {
                xml.Serialize(reader, obj);
                return reader.ToString();
            }
        }

        public static string SerializeAndSave(T obj, string filePath)
        {
            if (xml == null)
            {
                xml = new XmlSerializer(typeof(T));
            }
            using (TextWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                xml.Serialize(writer, obj);
                return writer.ToString();
            }
        }

        public static T DeSerialize(string xmlstr)
        {
            if (xml == null)
            {
                xml = new XmlSerializer(typeof(T));
            }
            using (StringReader reader = new StringReader(xmlstr))
            {
                return (T)xml.Deserialize(reader);
            }
        }

        public static T DeSerializeAndRead(string filePath)
        {
            if (xml == null)
            {
                xml = new XmlSerializer(typeof(T));
            }
            using (TextReader reader = new StreamReader(filePath,Encoding.UTF8))
            {
                return (T)xml.Deserialize(reader);
            }
        }
    }
}
