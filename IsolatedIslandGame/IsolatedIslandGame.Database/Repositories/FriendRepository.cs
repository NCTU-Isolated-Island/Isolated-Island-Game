using IsolatedIslandGame.Library;
using System.Collections.Generic;

namespace IsolatedIslandGame.Database.Repositories
{
    public abstract class FriendRepository
    {
        public abstract void AddFriend(int inviterPlayerID, int accepterPlayerID);
        public abstract void ConfirmFriend(int inviterPlayerID, int accepterPlayerID);
        public abstract void DeleteFriend(int inviterPlayerID, int accepterPlayerID);
        public abstract List<FriendInformation> ListOfFriendInformations(int playerID);
    }
}
