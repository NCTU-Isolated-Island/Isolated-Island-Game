using System.Collections.Generic;

namespace IsolatedIslandGame.Database.Repositories
{
    public abstract class FriendRepository
    {
        public abstract void AddFriend(int selfPlayerID, int friendPlayerID);
<<<<<<< HEAD
=======
        public abstract void ConfirmFriend(int selfPlayerID, int friendPlayerID);
>>>>>>> refs/remotes/origin/master
        public abstract void DeleteFriend(int selfPlayerID, int friendPlayerID);
        public abstract List<int> ListOfFriendPlayerIDs(int selfPlayerID);
    }
}
