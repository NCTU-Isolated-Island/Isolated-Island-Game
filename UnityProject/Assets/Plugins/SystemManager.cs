using IsolatedIslandGame.Library;
using UnityEngine;

namespace IsolatedIslandGame.Client
{
    public class SystemManager
    {
        private static SystemManager instance;
        public static SystemManager Instance { get { return instance; } }

        static SystemManager()
        {
            instance = new SystemManager();
        }

        public SystemConfiguration SystemConfiguration { get; private set; }

        private SystemManager()
        {
            LogService.InitialService(Debug.Log, Debug.LogFormat, Debug.LogWarning, Debug.LogWarningFormat, Debug.LogError, Debug.LogErrorFormat);
            SystemConfiguration = new SystemConfiguration
            {
                ServerName = "IsolatedIsland.TestServer",
                ServerAddress = "140.113.123.134",
                ServerPort = 4531,
                ServerVersion = "Development 0",
                ClientVersion = "Development 0"
            };
        }
    }
}
