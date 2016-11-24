using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.CommunicationInfrastructure;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.EventCodes;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Server.Configuration;
using System.Collections.Generic;

namespace IsolatedIslandGame.Server
{
    public class ServerUserCommunicationInterface : UserCommunicationInterface
    {
        protected ServerUser serverUser;

        public override void BindUser(User user)
        {
            base.BindUser(user);
            serverUser = user as ServerUser;
        }

        public override void ErrorInform(string title, string message)
        {
            LogService.ErrorFormat("User Identity:{0} ErrorInform Title: {1}, Message: {2}", user.IdentityInformation, title, message);
        }

        public override void SendEvent(UserEventCode eventCode, Dictionary<byte, object> parameters)
        {
            serverUser.SendEvent(eventCode, parameters);
        }

        public override void SendOperation(UserOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            LogService.FatalFormat("ServerUser SendOperation Identity: {0}, UserOperationCode: {1}", user.IdentityInformation, operationCode);
        }

        public override void SendResponse(UserOperationCode operationCode, ErrorCode errorCode, string debugMessage, Dictionary<byte, object> parameters)
        {
            serverUser.SendResponse(operationCode, errorCode, debugMessage, parameters);
        }

        public override void GetSystemVersion(out string serverVersion, out string clientVersion)
        {
            serverVersion = SystemConfiguration.Instance.ServerVersion;
            clientVersion = SystemConfiguration.Instance.ClientVersion;
        }
        public override void CheckSystemVersion(string serverVersion, string clientVersion)
        {
            LogService.FatalFormat("Server UpdateSystemVersion User Identity: {0}", user.IdentityInformation);
        }
    }
}
