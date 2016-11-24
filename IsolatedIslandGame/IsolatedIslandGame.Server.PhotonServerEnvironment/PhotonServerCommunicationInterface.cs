using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.EventCodes;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using Photon.SocketServer;
using System.Collections.Generic;

namespace IsolatedIslandGame.Server.PhotonServerEnvironment
{
    class PhotonServerCommunicationInterface : ServerCommunicationInterface
    {
        private Peer peer;

        public PhotonServerCommunicationInterface(Peer peer)
        {
            this.peer = peer;
        }

        public void SendEvent(UserEventCode eventCode, Dictionary<byte, object> parameters)
        {
            EventData eventData = new EventData
            {
                Code = (byte)eventCode,
                Parameters = parameters
            };
            peer.SendEvent(eventData, new SendParameters());
        }

        public void SendResponse(UserOperationCode operationCode, ErrorCode errorCode, string debugMessage, Dictionary<byte, object> parameters)
        {
            OperationResponse response = new OperationResponse((byte)operationCode, parameters)
            {
                ReturnCode = (short)errorCode,
                DebugMessage = debugMessage
            };
            peer.SendOperationResponse(response, new SendParameters());
        }
    }
}
