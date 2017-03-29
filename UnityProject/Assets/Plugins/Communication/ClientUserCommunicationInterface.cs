using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.CommunicationInfrastructure;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.EventCodes;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using System.Collections.Generic;
using System;
using IsolatedIslandGame.Library.TextData;
using IsolatedIslandGame.Library.Items;

namespace IsolatedIslandGame.Client.Communication
{
    class ClientUserCommunicationInterface : CommunicationInterface
    {
        public override void SendEvent(UserEventCode eventCode, Dictionary<byte, object> parameters)
        {
            LogService.FatalFormat("ClientUser SendEvent UserEventCode: {0}", eventCode);
        }

        public override void SendOperation(UserOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            PhotonService.Instance.SendOperation(operationCode, parameters);
        }

        public override void SendResponse(UserOperationCode operationCode, ErrorCode errorCode, string debugMessage, Dictionary<byte, object> parameters)
        {
            LogService.FatalFormat("ClientUser SendResponse UserOperationCode: {0}", operationCode);
        }
    }
}
