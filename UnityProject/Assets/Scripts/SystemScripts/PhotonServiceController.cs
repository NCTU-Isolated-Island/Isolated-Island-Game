using UnityEngine;
using IsolatedIslandGame.Client.Communication;
using IsolatedIslandGame.Library;

namespace IsolatedIslandGame.Client.Scripts.SystemScripts
{
    public class PhotonServiceController : MonoBehaviour
    {
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
                if (GUI.Button(new Rect(Screen.width / 2 - 200, Screen.height / 2 - 150, 400, 100), "連接至開發伺服器"))
                {
                    PhotonService.Instance.Connect(
                        serverName: SystemManager.Instance.SystemConfiguration.ServerName, 
                        serverAddress: SystemManager.Instance.SystemConfiguration.ServerAddress, 
                        port: SystemManager.Instance.SystemConfiguration.ServerPort
                    );
                }
            }
        }

        void OnDestroy()
        {
            EraseEvents();
        }
        void Update()
        {
            PhotonService.Instance.Service();
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
            }
            else
            {
                LogService.Info("Disconnected");
            }
        }
    }
}
