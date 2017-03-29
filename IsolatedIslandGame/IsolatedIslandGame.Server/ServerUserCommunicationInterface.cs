using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.CommunicationInfrastructure;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.EventCodes;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using System.Collections.Generic;

namespace IsolatedIslandGame.Server
{
    public class ServerUserCommunicationInterface : CommunicationInterface
    {
        protected ServerUser serverUser;

        public override void BindUser(User user)
        {
            base.BindUser(user);
            serverUser = user as ServerUser;
        }

        public override void SendEvent(UserEventCode eventCode, Dictionary<byte, object> parameters)
        {
            serverUser.SendEvent(eventCode, parameters);
        }

        public override void SendOperation(UserOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            LogService.FatalFormat("ServerUser SendOperation Identity: {0}, UserOperationCode: {1}", User.IdentityInformation, operationCode);
        }

        public override void SendResponse(UserOperationCode operationCode, ErrorCode errorCode, string debugMessage, Dictionary<byte, object> parameters)
        {
            serverUser.SendResponse(operationCode, errorCode, debugMessage, parameters);
        }
    }
}
