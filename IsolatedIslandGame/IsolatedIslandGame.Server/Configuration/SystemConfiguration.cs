using System.IO;
using System.Xml;
using System.Xml.Serialization;
using IsolatedIslandGame.Library;

namespace IsolatedIslandGame.Server.Configuration
{
    public class SystemConfiguration
    {
        [XmlElement]
        public string ServerVersion { get; set; }
        [XmlElement]
        public string ClientVersion { get; set; }
        [XmlElement]
        public string DatabaseHostname { get; set; }
        [XmlElement]
        public string DatabaseUsername { get; set; }
        [XmlElement]
        public string DatabasePassword { get; set; }
        [XmlElement]
        public string Database { get; set; }

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
                    ServerVersion = "not set",
                    ClientVersion = "not set",
                    DatabaseHostname = "not set",
                    DatabaseUsername = "not set",
                    DatabasePassword = "not set",
                    Database = "not set"
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
