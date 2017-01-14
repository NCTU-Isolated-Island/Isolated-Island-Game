using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.CommunicationInfrastructure;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.EventCodes;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using System.Collections.Generic;
using System;

namespace IsolatedIslandGame.Client.Communication
{
    class ClientUserCommunicationInterface : CommunicationInterface
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
            if(SystemConfiguration.Instance.ServerVersion != serverVersion)
            {
                LogService.FatalFormat("ServerVersion Inconsistent {0}/{1}", SystemConfiguration.Instance.ServerVersion, serverVersion);
            }
            else
            {
                LogService.InfoFormat("ServerVersion: {0}", serverVersion);
            }
            if (SystemConfiguration.Instance.ClientVersion != clientVersion)
            {
                LogService.FatalFormat("ClientVersion Inconsistent {0}/{1}", SystemConfiguration.Instance.ClientVersion, clientVersion);
            }
            else
            {
                LogService.InfoFormat("ClientVersion: {0}", clientVersion);
            }
        }

        public override bool Login(ulong facebookID, string accessToken, out string debugMessage, out ErrorCode errorCode)
        {
            LogService.FatalFormat("ClientUser Login ");
            debugMessage = "ClientUser Login";
            errorCode = ErrorCode.InvalidOperation;
            return false;
        }

        public override bool PlayerIDLogin(int playerID, string password, out string debugMessage, out ErrorCode errorCode)
        {
            LogService.FatalFormat("ClientUser Login ");
            debugMessage = "ClientUser Login";
            errorCode = ErrorCode.InvalidOperation;
            return false;
        }
    }
}
