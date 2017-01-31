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

        public override bool InviteFriend(int inviterPlayerID, int accepterPlayerID)
        {
            LogService.FatalFormat("ClienPlayer InviteFriend ");
            return false;
        }

        public override bool AcceptFriend(int inviterPlayerID, int accepterPlayerID)
        {
            LogService.FatalFormat("ClienPlayer AcceptFriend ");
            return false;
        }

        public override bool DeleteFriend(int selfPlayerID, int targetPlayerID)
        {
            LogService.FatalFormat("ClienPlayer DeleteFriend ");
            return false;
        }

        public override bool SendMessage(int senderPlayerID, int receiverPlayerID, string content)
        {
            LogService.FatalFormat("ClienPlayer SendMessage ");
            return false;
        }

        public override List<PlayerConversation> GetPlayerConversations(int playerID)
        {
            LogService.FatalFormat("ClienPlayer GetPlayerConversations ");
            return new List<PlayerConversation>();
        }

        public override bool TransactionRequest(int requesterPlayerID, int accepterPlayerID)
        {
            LogService.FatalFormat("ClienPlayer TransactionRequest ");
            return false;
        }

        public override bool AcceptTransaction(int requesterPlayerID, int accepterPlayerID)
        {
            LogService.FatalFormat("ClienPlayer AcceptTransaction ");
            return false;
        }

        public override bool ConfirmTransaction(int playerID, int transactionID)
        {
            LogService.FatalFormat("ClienPlayer ConfirmTransaction ");
            return false;
        }

        public override bool ChangeTransactionItem(int playerID, int transactionID, DataChangeType changeType, TransactionItemInfo info)
        {
            LogService.FatalFormat("ClienPlayer ChangeTransactionItem ");
            return false;
        }

        public override bool ReadPlayerMessage(int playerID, int playerMessageID)
        {
            LogService.FatalFormat("ClienPlayer ReadPlayerMessage ");
            return false;
        }
    }
}
