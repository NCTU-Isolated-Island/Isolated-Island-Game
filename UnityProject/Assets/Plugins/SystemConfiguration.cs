namespace IsolatedIslandGame.Client
{
    public class SystemConfiguration
    {
        public string ServerName { get; set; }
        public string ServerAddress { get; set; }
        public int ServerPort { get; set; }
        public string ServerVersion { get; set; }
        public string ClientVersion { get; set; }

        public SystemConfiguration() { }
    }
}
