using IsolatedIslandGame.Library;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace IsolatedIslandGame.Client
{
    public class SystemConfiguration
    {
        [XmlElement]
        public string ServerName { get; set; }
        [XmlElement]
        public string ServerAddress { get; set; }
        [XmlElement]
        public int ServerPort { get; set; }
        [XmlElement]
        public string ServerVersion { get; set; }
        [XmlElement]
        public string ClientVersion { get; set; }

        public SystemConfiguration() { }
        public static SystemConfiguration Load(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SystemConfiguration));
            if (File.Exists(filePath))
            {
                using (XmlReader reader = XmlReader.Create(filePath))
                {
                    if (serializer.CanDeserialize(reader))
                    {
                        return (SystemConfiguration)serializer.Deserialize(reader);
                    }
                    else
                    {
                        LogService.Fatal("version configuration can't be serialized!");
                        return null;
                    }
                }
            }
            else
            {
                SystemConfiguration versionConfiguration = new SystemConfiguration
                {
                    ServerName = "IsolatedIsland.TestServer",
                    ServerAddress = "140.113.123.134",
                    ServerPort = 4531,
                    ServerVersion = "Development 0",
                    ClientVersion = "Development 0"
                };
                using (XmlWriter writer = XmlWriter.Create(filePath))
                {
                    serializer.Serialize(writer, versionConfiguration);
                }
                return versionConfiguration;
            }
        }
    }
}
