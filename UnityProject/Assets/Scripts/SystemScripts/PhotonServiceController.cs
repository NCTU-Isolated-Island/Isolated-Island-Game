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

        void Start()
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
                        serverName: SystemConfiguration.Instance.ServerName,
                        serverAddress: SystemConfiguration.Instance.ServerAddress,
                        port: SystemConfiguration.Instance.ServerPort
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
        }

        public void RegisterEvents()
        {
			PhotonService.Instance.OnConnectChange += OnConnectChange;
        }

        private void OnConnectChange(bool connected)
        {
            if (connected)
            {
                LogService.Info("Connected");
                UserManager.Instance.User.OperationManager.FetchDataResolver.FetchSystemVersion();
            }
            else
            {
                LogService.Info("Disconnected");
                reconnectCountdownTimer = reconnectInterval;
                SceneManager.LoadScene("LoginScene");
            }
        }
    }
}
