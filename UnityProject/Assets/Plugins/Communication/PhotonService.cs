using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace IsolatedIslandGame.Client.Communication
{
    public class PhotonService : IPhotonPeerListener
    {
        private PhotonPeer peer;
        private bool serverConnected;
        //protected EventResolver eventResolver;
        //protected ResponseResolver responseResolver;

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

        public PhotonService()
        {
            //eventResolver = new EventResolver(this);
            //responseResolver = new ResponseResolver(this);
        }

        public void DebugReturn(DebugLevel level, string message)
        {
            Debug.Log(level.ToString() + " : " + message);
        }

        public void OnEvent(EventData eventData)
        {
            //eventResolver.Operate(eventData);
        }

        public void OnOperationResponse(OperationResponse operationResponse)
        {
            //responseResolver.Operate(operationResponse);
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
                if (!peer.Connect(serverAddress + ":" + port.ToString(), serverName))
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
    }
}
