using IsolatedIslandGame.Database;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.CommunicationInfrastructure;
using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Library.TextData;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.EventCodes;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Server.Configuration;
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

        public override void ErrorInform(string title, string message)
        {
            LogService.ErrorFormat("User Identity:{0} ErrorInform Title: {1}, Message: {2}", User.IdentityInformation, title, message);
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

        public override void GetSystemVersion(out string serverVersion, out string clientVersion)
        {
            serverVersion = SystemConfiguration.Instance.ServerVersion;
            clientVersion = SystemConfiguration.Instance.ClientVersion;
        }
        public override void CheckSystemVersion(string serverVersion, string clientVersion)
        {
            LogService.FatalFormat("Server UpdateSystemVersion User Identity: {0}", User.IdentityInformation);
        }

        public override bool Login(ulong facebookID, string accessToken, out string debugMessage, out ErrorCode errorCode)
        {
            return PlayerFactory.Instance.PlayerLogin(serverUser, facebookID, accessToken, out debugMessage, out errorCode);
        }
        public override bool PlayerIDLogin(int playerID, string password, out string debugMessage, out ErrorCode errorCode)
        {
            return PlayerFactory.Instance.PlayerLoginWithPlayerID(serverUser, playerID, password, out debugMessage, out errorCode);
        }

        public override bool InviteFriend(int inviterPlayerID, int accepterPlayerID)
        {
            return FriendManager.Instance.InviteFriend(inviterPlayerID, accepterPlayerID);
        }

        public override bool AcceptFriend(int inviterPlayerID, int accepterPlayerID)
        {
            return FriendManager.Instance.AcceptFriend(inviterPlayerID, accepterPlayerID);
        }

        public override bool DeleteFriend(int selfPlayerID, int targetPlayerID)
        {
            return FriendManager.Instance.DeleteFriend(selfPlayerID, targetPlayerID);
        }

        public override bool SendMessage(int senderPlayerID, int receiverPlayerID, string content)
        {
            PlayerMessage message;
            if(DatabaseService.RepositoryList.PlayerMessageRepository.Create(senderPlayerID, content, out message))
            {
                PlayerConversation receiverConversation, senderConversation;
                if(DatabaseService.RepositoryList.PlayerConversationRepository.Create(receiverPlayerID, message.playerMessageID, false, out receiverConversation) &&
                   DatabaseService.RepositoryList.PlayerConversationRepository.Create(senderPlayerID, message.playerMessageID, true, out senderConversation))
                {
                    Player sender, receiver;
                    if(PlayerFactory.Instance.FindPlayer(senderPlayerID, out sender))
                    {
                        sender.GetPlayerConversation(senderConversation);
                    }
                    if (PlayerFactory.Instance.FindPlayer(receiverPlayerID, out receiver))
                    {
                        receiver.GetPlayerConversation(receiverConversation);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public override List<PlayerConversation> GetPlayerConversations(int playerID)
        {
            return DatabaseService.RepositoryList.PlayerConversationRepository.ListOfReceiver(playerID);
        }

        public override bool TransactionRequest(int requesterPlayerID, int accepterPlayerID)
        {
            Player accepter;
            if(PlayerFactory.Instance.ContainsPlayer(requesterPlayerID) && PlayerFactory.Instance.FindPlayer(accepterPlayerID, out accepter))
            {
                accepter.TransactionRequest(requesterPlayerID);
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool AcceptTransaction(int requesterPlayerID, int accepterPlayerID)
        {
            Transaction transaction;
            if(TransactionManager.Instance.CreateTransaction(requesterPlayerID, accepterPlayerID, out transaction))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool ConfirmTransaction(int playerID, int transactionID)
        {
            Transaction transaction; ;
            if (TransactionManager.Instance.FindTransaction(transactionID, out transaction))
            {
                return transaction.Confirm(playerID);
            }
            else
            {
                return false;
            }
        }

        public override bool ChangeTransactionItem(int playerID, int transactionID, DataChangeType changeType, TransactionItemInfo info)
        {
            Transaction transaction; ;
            if (TransactionManager.Instance.FindTransaction(transactionID, out transaction))
            {
                return transaction.ChangeTransactionItem(playerID, changeType, info);
            }
            else
            {
                return false;
            }
        }
    }
}
