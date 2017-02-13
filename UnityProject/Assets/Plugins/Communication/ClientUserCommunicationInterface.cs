﻿using IsolatedIslandGame.Library;
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

        public override void GetSystemVersion(out string serverVersion, out string clientVersion)
        {
            LogService.FatalFormat("ClientUser GetSystemVersion");
            serverVersion = "";
            clientVersion = "";
        }
        public override void CheckSystemVersion(string serverVersion, string clientVersion)
        {
            if(SystemConfiguration.Instance.ServerVersion != serverVersion || SystemConfiguration.Instance.ClientVersion != clientVersion)
            {
                UserManager.Instance.User.UserInform("提示", "有新版本，請更新以獲取最新的遊戲內容");
                LogService.FatalFormat("ServerVersion Inconsistent {0}/{1}", SystemConfiguration.Instance.ServerVersion, serverVersion);
                LogService.FatalFormat("ClientVersion Inconsistent {0}/{1}", SystemConfiguration.Instance.ClientVersion, clientVersion);
            }
            else
            {
                LogService.InfoFormat("ServerVersion: {0}", serverVersion);
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

        public override bool ChangeTransactionConfirmStatus(int playerID, int transactionID, bool isConfirmed)
        {
            LogService.FatalFormat("ClienPlayer ChangeTransactionConfirmStatus ");
            return false;
        }

        public override bool CancelTransaction(int playerID, int transactionID)
        {
            LogService.FatalFormat("ClienPlayer CancelTransaction ");
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
