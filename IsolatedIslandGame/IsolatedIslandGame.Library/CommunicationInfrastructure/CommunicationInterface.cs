using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.EventCodes;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure
{
    public abstract class CommunicationInterface
    {
        public User User { get; protected set; }
        public virtual void BindUser(User user)
        {
            User = user;
        }
        public abstract void SendEvent(UserEventCode eventCode, Dictionary<byte, object> parameters);
        public abstract void SendOperation(UserOperationCode operationCode, Dictionary<byte, object> parameters);
        public abstract void SendResponse(UserOperationCode operationCode, ErrorCode errorCode, string debugMessage, Dictionary<byte, object> parameters);
    }
}
