using IsolatedIslandGame.Library;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;
using System;

namespace IsolatedIslandGame.Server.PhotonServerEnvironment
{
    public class Peer : ClientPeer
    {
        public Guid Guid { get { return User.Guid; } }
        public ServerUser User { get; protected set; }


        public Peer(InitRequest initRequest) : base(initRequest)
        {
            User = new ServerUser(new PhotonServerCommunicationInterface(this), new ServerUserCommunicationInterface(), RemoteIPAddress);
            UserFactory.Instance.UserConnect(User);
            User.OnPlayerOffline += Disconnect;
        }

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            Application.Log.InfoFormat("User Disconnect from: {0} because: {1}", RemoteIPAddress, reasonDetail);
            UserFactory.Instance.UserDisconnect(User);
            User.OnPlayerOffline -= Disconnect;
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            User.OperationManager.Operate((UserOperationCode)operationRequest.OperationCode, operationRequest.Parameters);
        }

        private void Disconnect(Player player)
        {
            Disconnect();
        }
    }
}
