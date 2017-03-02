namespace IsolatedIslandGame.Client
{
    public class SystemConfiguration
    {
        private static SystemConfiguration instance;
        public static SystemConfiguration Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new SystemConfiguration
                    {
                        ServerName = "IsolatedIsland.ReleaseServer",
                        //ServerName = "IsolatedIsland.TestServer",
                        ServerAddress = "140.113.123.134",
                        ServerPort = 4532,
                        //ServerPort = 4531,
                        ServerVersion = "Release0.2",
                        ClientVersion = "Release0.2"
                    };
                }
                return instance;
            }
        }

        public string ServerName { get; set; }
        public string ServerAddress { get; set; }
        public int ServerPort { get; set; }
        public string ServerVersion { get; set; }
        public string ClientVersion { get; set; }

        public SystemConfiguration() { }
    }
}
