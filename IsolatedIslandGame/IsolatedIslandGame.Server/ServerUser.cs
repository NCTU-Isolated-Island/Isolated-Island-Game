using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.CommunicationInfrastructure;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.EventCodes;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using System;
using System.Collections.Generic;
using System.Net;

namespace IsolatedIslandGame.Server
{
    public class ServerUser : User
    {
        public Guid Guid { get; protected set; }
        private ServerCommunicationInterface serverCommunicationInterface;

        public ServerUser(ServerCommunicationInterface serverCommunicationInterface, UserCommunicationInterface communicationInterface, IPAddress lastConnectedIPAddress) : base(communicationInterface)
        {
            LastConnectedIPAddress = lastConnectedIPAddress;
            Guid = Guid.NewGuid();
            while(UserFactory.Instance.ContainsUserGuid(Guid))
            {
                Guid = Guid.NewGuid();
            }
            this.serverCommunicationInterface = serverCommunicationInterface;
        }

        public void SendEvent(UserEventCode eventCode, Dictionary<byte, object> parameters)
        {
            serverCommunicationInterface.SendEvent(eventCode, parameters);
        }
        public void SendResponse(UserOperationCode operationCode, ErrorCode errorCode, string debugMessage, Dictionary<byte, object> parameters)
        {
            serverCommunicationInterface.SendResponse(operationCode, errorCode, debugMessage, parameters);
        }
    }
}
