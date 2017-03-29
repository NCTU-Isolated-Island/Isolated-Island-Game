using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Library.TextData;
using IsolatedIslandGame.Protocol;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure
{
    public interface OperationInterface
    {
        void GetSystemVersion(out string serverVersion, out string clientVersion);
        bool Login(User user, ulong facebookID, string accessToken, out string debugMessage, out ErrorCode errorCode);
        bool PlayerIDLogin(User user, int playerID, string password, out string debugMessage, out ErrorCode errorCode);
        bool InviteFriend(int inviterPlayerID, int accepterPlayerID);
        bool AcceptFriend(int inviterPlayerID, int accepterPlayerID);
        bool DeleteFriend(int selfPlayerID, int targetPlayerID);
        bool SendMessage(int senderPlayerID, int receiverPlayerID, string content);
        List<PlayerConversation> GetPlayerConversations(int playerID);

        bool TransactionRequest(int requesterPlayerID, int accepterPlayerID);
        bool AcceptTransaction(int requesterPlayerID, int accepterPlayerID);
        bool ChangeTransactionConfirmStatus(int playerID, int transactionID, bool isConfirmed);
        bool ChangeTransactionItem(int playerID, int transactionID, DataChangeType changeType, TransactionItemInfo info);
        bool CancelTransaction(int playerID, int transactionID);
        bool ReadPlayerMessage(int playerID, int playerMessageID);
        bool DonateItemToPlayer(int senderPlayerID, int receiverPlayerID, Item item, int itemCount);
        bool AssignQuestToAllPlayer(int questID, string administratorPassword);
        bool SendWorldChannelMessage(int senderPlayerID, string content);
    }
}
