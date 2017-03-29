using IsolatedIslandGame.Client.Communication;
using IsolatedIslandGame.Library;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IsolatedIslandGame.Client.Scripts.SystemScripts
{
    public class PhotonServiceController : MonoBehaviour
    {
        private bool isFirstConnected = true;

        void Start()
        {
            RegisterEvents();
            PhotonService.Instance.Connect(
                serverName: SystemConfiguration.Instance.ServerName,
                serverAddress: SystemConfiguration.Instance.ServerAddress,
                port: SystemConfiguration.Instance.ServerPort
            );
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
                UserManager.Instance.User.OperationManager.FetchDataResolver.FetchSystemVersion();
                
                if (isFirstConnected)
                {
                    isFirstConnected = false;
                }
                else
                {
                    GameManager.Instance.Login();
                }
            }
            else
            {
                LogService.Info("Disconnected");
                UserManager.Instance.User.PlayerOffline();
                PhotonService.Instance.Connect(
                    serverName: SystemConfiguration.Instance.ServerName,
                    serverAddress: SystemConfiguration.Instance.ServerAddress,
                    port: SystemConfiguration.Instance.ServerPort
                );
            }
        }
    }
}
