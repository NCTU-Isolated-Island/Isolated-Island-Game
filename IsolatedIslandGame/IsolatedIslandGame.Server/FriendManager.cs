using IsolatedIslandGame.Database;
using IsolatedIslandGame.Library;

namespace IsolatedIslandGame.Server
{
    public class FriendManager
    {
        public static FriendManager Instance { get; private set; }

        public static void InitialManager()
        {
            Instance = new FriendManager();
        }

        public bool InviteFriend(int inviterPlayerID, int accepterPlayerID)
        {
            Player inviter;
            if(PlayerFactory.Instance.FindPlayer(inviterPlayerID, out inviter) && !inviter.ContainsFriend(accepterPlayerID))
            {
                FriendInformation information;
                if(DatabaseService.RepositoryList.FriendRepository.AddFriend(inviterPlayerID, accepterPlayerID, out information))
                {
                    inviter.AddFriend(information);
                    Player accepter;
                    if (PlayerFactory.Instance.FindPlayer(accepterPlayerID, out accepter))
                    {
                        accepter.AddFriend(new FriendInformation
                        {
                            friendPlayerID = inviter.PlayerID,
                            isInviter = true,
                            isConfirmed = false
                        });
                        accepter.User.EventManager.UserInform("邀請", $"玩家 {inviter.Nickname} 向你發出了好友邀請，去查看一下吧!");
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
        public bool AcceptFriend(int inviterPlayerID, int accepterPlayerID)
        {
            Player accepter;
            if (PlayerFactory.Instance.FindPlayer(accepterPlayerID, out accepter) && accepter.ContainsFriend(inviterPlayerID))
            {
                FriendInformation information;
                if (DatabaseService.RepositoryList.FriendRepository.ConfirmFriend(inviterPlayerID, accepterPlayerID, out information))
                {
                    accepter.AddFriend(information);
                    Player inviter;
                    if (PlayerFactory.Instance.FindPlayer(inviterPlayerID, out inviter))
                    {
                        inviter.AddFriend(new FriendInformation
                        {
                            friendPlayerID = accepter.PlayerID,
                            isInviter = false,
                            isConfirmed = true
                        });
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
        public bool DeleteFriend(int selfPlayerID, int targetPlayerID)
        {
            Player self;
            if (PlayerFactory.Instance.FindPlayer(selfPlayerID, out self) && self.ContainsFriend(targetPlayerID))
            {
                DatabaseService.RepositoryList.FriendRepository.DeleteFriend(selfPlayerID, targetPlayerID);
                self.RemoveFriend(targetPlayerID);
                Player target;
                if (PlayerFactory.Instance.FindPlayer(targetPlayerID, out target))
                {
                    target.RemoveFriend(selfPlayerID);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
