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
                        ServerAddress = "140.113.123.134",
                        ServerPort = 4532,
                        ServerVersion = "Development 0",
                        ClientVersion = "Development 0"
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
