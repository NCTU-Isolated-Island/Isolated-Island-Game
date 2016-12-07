using IsolatedIslandGame.Library;
using UnityEngine;

namespace IsolatedIslandGame.Client
{
    public class GameManager : SystemManager
    {
        private static GameManager instance;
        public static new GameManager Instance { get { return instance; } }

        static GameManager()
        {
            instance = new GameManager(UserManager.Instance.User);
            Initial(instance);
        }

        public SystemConfiguration SystemConfiguration { get; private set; }

        private GameManager(User user) : base(user)
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
