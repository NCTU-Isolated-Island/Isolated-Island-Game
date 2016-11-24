using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.EventCodes;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure
{
    public abstract class UserCommunicationInterface
    {
        protected User user;
        public virtual void BindUser(User user)
        {
            this.user = user;
        }
        public abstract void SendEvent(UserEventCode eventCode, Dictionary<byte, object> parameters);
        public abstract void SendOperation(UserOperationCode operationCode, Dictionary<byte, object> parameters);
        public abstract void SendResponse(UserOperationCode operationCode, ErrorCode errorCode, string debugMessage, Dictionary<byte, object> parameters);
        public abstract void ErrorInform(string title, string message);

        public abstract void GetSystemVersion(out string serverVersion, out string clientVersion);
        public abstract void CheckSystemVersion(string serverVersion, string clientVersion);
    }
}
