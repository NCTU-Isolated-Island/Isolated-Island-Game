using IsolatedIslandGame.Library;
using UnityEngine;

namespace IsolatedIslandGame.Client
{
    public class ClientSystemManager : SystemManager
    {
        private static ClientSystemManager instance;
        public static new ClientSystemManager Instance { get { return instance; } }

        static ClientSystemManager()
        {
            instance = new ClientSystemManager(UserManager.Instance.User);
            Initial(instance);
        }

        public SystemConfiguration SystemConfiguration { get; private set; }

        private ClientSystemManager(User user) : base(user)
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
