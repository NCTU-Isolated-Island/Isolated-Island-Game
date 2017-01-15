using IsolatedIslandGame.Library;
using System.Collections.Generic;

namespace IsolatedIslandGame.Database.Repositories
{
    public abstract class FriendRepository
    {
        public abstract bool AddFriend(int inviterPlayerID, int accepterPlayerID, out FriendInformation friendInformation);
        public abstract bool ConfirmFriend(int inviterPlayerID, int accepterPlayerID, out FriendInformation friendInformation);
        public abstract void DeleteFriend(int selfPlayerID, int targetPlayerID);
        public abstract List<FriendInformation> ListOfFriendInformations(int playerID);
    }
}
