using ExitGames.Client.Photon;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.EventCodes;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace IsolatedIslandGame.Client.Communication
{
    public class PhotonService : IPhotonPeerListener
    {
        private static PhotonService instance;
        public static PhotonService Instance { get { return instance; } }

        static PhotonService()
        {
            instance = new PhotonService();
        }

        private PhotonPeer peer;
        private bool serverConnected;

        #region Connect Change
        private event Action<bool> onConnectChange;
        public event Action<bool> OnConnectChange
        {
            add { onConnectChange += value; }
            remove { onConnectChange -= value; }
        }
        #endregion

        public bool ServerConnected
        {
            get { return serverConnected; }
            private set
            {
                serverConnected = value;
                if (onConnectChange != null)
                {
                    onConnectChange(serverConnected);
                }
                else
                {
                    DebugReturn(DebugLevel.ERROR, "onConnectChange event is null");
                }
            }
        }
        private string serverName;
        private string serverAddress;
        private int port;

        public void DebugReturn(DebugLevel level, string message)
        {
            Debug.Log(level.ToString() + " : " + message);
        }

        public void OnEvent(EventData eventData)
        {
            UserManager.Instance.User.EventManager.Operate(
                eventCode: (UserEventCode)eventData.Code, 
                parameters: eventData.Parameters
            );
        }

        public void OnOperationResponse(OperationResponse operationResponse)
        {
            UserManager.Instance.User.ResponseManager.Operate(
                operationCode: (UserOperationCode)operationResponse.OperationCode, 
                returnCode: (ErrorCode)operationResponse.ReturnCode,
                debugMessage: operationResponse.DebugMessage,
                parameters: operationResponse.Parameters
            );
        }

        public void OnStatusChanged(StatusCode statusCode)
        {
            switch (statusCode)
            {
                case StatusCode.Connect:
                    peer.EstablishEncryption();
                    break;
                case StatusCode.Disconnect:
                    ServerConnected = false;
                    break;
                case StatusCode.EncryptionEstablished:
                    ServerConnected = true;
                    break;
            }
        }

        public void Connect(string serverName, string serverAddress, int port)
        {
            this.serverName = serverName;
            this.serverAddress = serverAddress;
            this.port = port;
            try
            {
                peer = new PhotonPeer(this, ConnectionProtocol.Tcp);
                if (!peer.Connect(this.serverAddress + ":" + this.port.ToString(), this.serverName))
                {
                    DebugReturn(DebugLevel.ERROR, "Connect Fail");
                    ServerConnected = false;
                }
                else
                {
                    DebugReturn(DebugLevel.INFO, peer.PeerState.ToString());
                }
            }
            catch (Exception ex)
            {
                ServerConnected = false;
                DebugReturn(DebugLevel.ERROR, ex.Message);
                DebugReturn(DebugLevel.ERROR, ex.StackTrace);
            }
        }

        public void Disconnect()
        {
            if (peer != null)
                peer.Disconnect();
        }

        public void Service()
        {
            try
            {
                if (peer != null)
                    peer.Service();
            }
            catch (Exception ex)
            {
                DebugReturn(DebugLevel.ERROR, ex.Message);
                DebugReturn(DebugLevel.ERROR, ex.StackTrace);
            }
        }

        public void SendOperation(UserOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            peer.OpCustom((byte)operationCode, parameters, true, 0, true);
        }
    }
}
