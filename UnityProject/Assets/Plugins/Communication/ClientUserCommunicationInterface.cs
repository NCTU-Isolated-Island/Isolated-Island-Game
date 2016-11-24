using System;
using System.Collections.Generic;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.CommunicationInfrastructure;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.EventCodes;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;

namespace IsolatedIslandGame.Client.Communication
{
    class ClientUserCommunicationInterface : UserCommunicationInterface
    {
        public override void ErrorInform(string title, string message)
        {
            //will be implement in alert window
            LogService.ErrorFormat("ClientUser ErrorInform Title: {0}, Message: {1}", title, message);
        }

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

        public override void GetSystemVersion(out string serverVersion, out string clientVersion)
        {
            LogService.FatalFormat("ClientUser GetSystemVersion");
            serverVersion = "";
            clientVersion = "";
        }
        public override void CheckSystemVersion(string serverVersion, string clientVersion)
        {
            if(SystemManager.Instance.SystemConfiguration.ServerVersion != serverVersion)
            {
                LogService.FatalFormat("ServerVersion Inconsistent {0}/{1}", SystemManager.Instance.SystemConfiguration.ServerVersion, serverVersion);
            }
            else
            {
                LogService.InfoFormat("ServerVersion: {0}", serverVersion);
            }
            if (SystemManager.Instance.SystemConfiguration.ClientVersion != clientVersion)
            {
                LogService.FatalFormat("ClientVersion Inconsistent {0}/{1}", SystemManager.Instance.SystemConfiguration.ClientVersion, clientVersion);
            }
            else
            {
                LogService.InfoFormat("ClientVersion: {0}", clientVersion);
            }
        }
    }
}
