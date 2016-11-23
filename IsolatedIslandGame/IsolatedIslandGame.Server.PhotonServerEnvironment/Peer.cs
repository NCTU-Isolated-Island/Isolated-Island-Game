using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;
using System;

namespace IsolatedIslandGame.Server.PhotonServerEnvironment
{
    public class Peer : ClientPeer
    {
        public Guid Guid { get; }
        public ServerUser User { get; protected set; }


        public Peer(InitRequest initRequest) : base(initRequest)
        {
            throw new NotImplementedException();
        }

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            throw new NotImplementedException();
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            throw new NotImplementedException();
        }
    }
}
