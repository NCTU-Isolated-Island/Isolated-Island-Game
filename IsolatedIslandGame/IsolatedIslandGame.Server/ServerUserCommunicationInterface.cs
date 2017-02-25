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
                PlayerConversation conversation;
                if(DatabaseService.RepositoryList.PlayerConversationRepository.Create(receiverPlayerID, message.playerMessageID, false, out conversation))
                {
                    Player sender, receiver;
                    if(PlayerFactory.Instance.FindPlayer(senderPlayerID, out sender))
                    {
                        sender.GetPlayerConversation(conversation);
                    }
                    if (PlayerFactory.Instance.FindPlayer(receiverPlayerID, out receiver))
                    {
                        receiver.GetPlayerConversation(conversation);
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
            return DatabaseService.RepositoryList.PlayerConversationRepository.ListOfPlayer(playerID);
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

        public override bool ChangeTransactionConfirmStatus(int playerID, int transactionID, bool isConfirmed)
        {
            Transaction transaction; ;
            if (TransactionManager.Instance.FindTransaction(transactionID, out transaction))
            {
                if(isConfirmed)
                {
                    return transaction.ChangeConfirmStatus(playerID, isConfirmed);
                }
                else
                {
                    return transaction.ChangeConfirmStatus(transaction.RequesterPlayerID, isConfirmed) && transaction.ChangeConfirmStatus(transaction.AccepterPlayerID, isConfirmed);
                }
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

        public override bool CancelTransaction(int playerID, int transactionID)
        {
            Transaction transaction; ;
            if (TransactionManager.Instance.FindTransaction(transactionID, out transaction))
            {
                transaction.EndTransaction(false);
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool ReadPlayerMessage(int playerID, int playerMessageID)
        {
            if (DatabaseService.RepositoryList.PlayerConversationRepository.SetPlayerMessageRead(playerID, playerMessageID))
            {
                PlayerConversation conversation;
                if (DatabaseService.RepositoryList.PlayerConversationRepository.Read(playerID, playerMessageID, out conversation))
                {
                    Player sender, receiver;
                    if (PlayerFactory.Instance.FindPlayer(conversation.message.senderPlayerID, out sender))
                    {
                        sender.GetPlayerConversation(conversation);
                    }
                    if (PlayerFactory.Instance.FindPlayer(conversation.receiverPlayerID, out receiver))
                    {
                        receiver.GetPlayerConversation(conversation);
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

        public override bool DonateItemToPlayer(int senderPlayerID, int receiverPlayerID, Item item, int itemCount)
        {
            Player sender, receiver;
            if (PlayerFactory.Instance.FindPlayer(senderPlayerID, out sender) && PlayerFactory.Instance.FindPlayer(receiverPlayerID, out receiver))
            {
                lock(sender.Inventory)
                {
                    lock(receiver.Inventory)
                    {
                        if(sender.Inventory.RemoveItemCheck(item.ItemID, itemCount) && sender.Inventory.AddItemCheck(item.ItemID, itemCount))
                        {
                            sender.Inventory.RemoveItem(item.ItemID, itemCount);
                            receiver.Inventory.AddItem(item, itemCount);
                            sender.User.EventManager.UserInform("成功", "贈送物品成功");
                            receiver.User.EventManager.UserInform("提示", $"{sender.Nickname} 贈送了{itemCount}個{item.ItemName}給你");
                            return true;
                        }
                        else
                        {
                            sender.User.EventManager.UserInform("失敗", "贈送物品失敗，且確認是否擁有足夠的物品");
                            return false;
                        }
                    }
                }
            }
            else
            {
                return false;
            }
        }
    }
}
