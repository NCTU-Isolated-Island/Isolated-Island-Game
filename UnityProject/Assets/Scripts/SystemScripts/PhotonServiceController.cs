using IsolatedIslandGame.Client.Communication;
using IsolatedIslandGame.Library;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IsolatedIslandGame.Client.Scripts.SystemScripts
{
    public class PhotonServiceController : MonoBehaviour
    {
        private float reconnectInterval = 10;
        private float reconnectCountdownTimer = 0;

        void Awake()
        {
            RegisterEvents();
        }
        void OnGUI()
        {
            if (PhotonService.Instance.ServerConnected)
            {
                GUI.Label(new Rect(20, 10, 100, 20), "connected");
            }
            else
            {
                GUI.Label(new Rect(20, 10, 100, 20), "connect failed");
            }
        }

        void OnDestroy()
        {
            EraseEvents();
        }
        void Update()
        {
            PhotonService.Instance.Service();
            if (!PhotonService.Instance.ServerConnected)
            {
                reconnectCountdownTimer -= Time.deltaTime;
                if (reconnectCountdownTimer <= 0)
                {
                    reconnectCountdownTimer = reconnectInterval;
                    PhotonService.Instance.Connect(
                        serverName: SystemManager.Instance.SystemConfiguration.ServerName,
                        serverAddress: SystemManager.Instance.SystemConfiguration.ServerAddress,
                        port: SystemManager.Instance.SystemConfiguration.ServerPort
                    );
                }
            }
        }

        void OnApplicationQuit()
        {
            PhotonService.Instance.Disconnect();
        }

        public void EraseEvents()
        {
            PhotonService.Instance.OnConnectChange -= OnConnectChange;
            UserManager.Instance.User.OnPlayerOnline -= ToMainScene;
            UserManager.Instance.User.OnPlayerOffline -= ToLoginScene;
        }

        public void RegisterEvents()
        {
            PhotonService.Instance.OnConnectChange += OnConnectChange;
            UserManager.Instance.User.OnPlayerOnline += ToMainScene;
            UserManager.Instance.User.OnPlayerOffline += ToLoginScene;
        }

        private void OnConnectChange(bool connected)
        {
            if (connected)
            {
                LogService.Info("Connected");
                UserManager.Instance.User.OperationManager.FetchDataResolver.FetchSystemVersion();
                FacebookService.InitialFacebook();
            }
            else
            {
                LogService.Info("Disconnected");
                reconnectCountdownTimer = reconnectInterval;
                SceneManager.LoadScene("Login");
            }
        }
        private void ToLoginScene(Player player)
        {
            SceneManager.LoadScene("Login");
        }
        private void ToMainScene(Player player)
        {
            SceneManager.LoadScene("Main");
            Debug.Log("IP:" + player.LastConnectedIPAddress);
        }
    }
}
