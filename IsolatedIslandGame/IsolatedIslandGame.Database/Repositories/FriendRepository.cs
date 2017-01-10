using IsolatedIslandGame.Library;
using System.Collections.Generic;

namespace IsolatedIslandGame.Database.Repositories
{
    public abstract class FriendRepository
    {
        public abstract void AddFriend(int senderPlayerID, int receiverPlayerID);
        public abstract void ConfirmFriend(int senderPlayerID, int receiverPlayerID);
        public abstract void DeleteFriend(int senderPlayerID, int receiverPlayerID);
        public abstract List<FriendInformation> ListOfFriendInformations(int playerID);
    }
}
