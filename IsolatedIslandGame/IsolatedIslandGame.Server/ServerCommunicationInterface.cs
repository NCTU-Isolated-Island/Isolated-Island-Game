using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.EventCodes;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using System.Collections.Generic;

namespace IsolatedIslandGame.Server
{
    public interface ServerCommunicationInterface
    {
        void SendEvent(UserEventCode eventCode, Dictionary<byte, object> parameters);
        void SendResponse(UserOperationCode operationCode, ErrorCode errorCode, string debugMessage, Dictionary<byte, object> parameters);
    }
}
