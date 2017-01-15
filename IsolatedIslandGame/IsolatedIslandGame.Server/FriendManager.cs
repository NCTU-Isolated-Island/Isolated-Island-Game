using System;
using System.Collections.Generic;
using System.Linq;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Database;

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
                            playerInformation = new PlayerInformation
                            {
                                playerID = inviter.PlayerID,
                                nickname = inviter.Nickname,
                                signature = inviter.Signature,
                                groupType = inviter.GroupType,
                                vesselID = inviter.Vessel.VesselID
                            },
                            isInviter = true,
                            isConfirmed = false
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
                            playerInformation = new PlayerInformation
                            {
                                playerID = accepter.PlayerID,
                                nickname = accepter.Nickname,
                                signature = accepter.Signature,
                                groupType = accepter.GroupType,
                                vesselID = accepter.Vessel.VesselID
                            },
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
