using System.IO;
using System.Xml;
using System.Xml.Serialization;
using IsolatedIslandGame.Library;

namespace IsolatedIslandGame.Server.Configuration
{
    public class SystemConfiguration
    {
        private static SystemConfiguration instance;
        public static SystemConfiguration Instance { get { return instance; } }

        public static void InitialConfiguration(SystemConfiguration configuration)
        {
            instance = configuration;
        }

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
        [XmlElement]
        public string AdministratorPassword { get; set; }

        public SystemConfiguration() { }
        public static bool Load(string filePath, out SystemConfiguration configuration)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SystemConfiguration));
            if (File.Exists(filePath))
            {
                using (XmlReader reader = XmlReader.Create(filePath))
                {
                    if (serializer.CanDeserialize(reader))
                    {
                        configuration = (SystemConfiguration)serializer.Deserialize(reader);
                        return true;
                    }
                    else
                    {
                        configuration = null;
                        LogService.Fatal("version configuration can't be serialized!");
                        return false;
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
                    Database = "not set",
                    AdministratorPassword = "not set"
                };
                using (XmlWriter writer = XmlWriter.Create(filePath))
                {
                    serializer.Serialize(writer, versionConfiguration);
                }
                configuration = versionConfiguration;
                return true;
            }
        }
    }
}
