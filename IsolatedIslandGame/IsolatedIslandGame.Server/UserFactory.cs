using IsolatedIslandGame.Library;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Server
{
    public class UserFactory
    {
        private static UserFactory instance;
        public static UserFactory Instance { get { return instance; } }
        
        public static void InitialFactory()
        {
            instance = new UserFactory();
        }

        private Dictionary<Guid, ServerUser> connectedUsers;

        private UserFactory()
        {
            connectedUsers = new Dictionary<Guid, ServerUser>();
        }

        public bool UserConnect(ServerUser user)
        {
            if (connectedUsers.ContainsKey(user.Guid))
            {
                return false;
            }
            else
            {
                connectedUsers.Add(user.Guid, user);
                LogService.Instance.InfoFormat("User Guid: {0} Connect from {1}", user.Guid, user.LastConnectedIPAddress);
                return true;
            }
        }
        public void UserDisconnect(ServerUser user)
        {
            if (connectedUsers.ContainsKey(user.Guid))
            {
                connectedUsers.Remove(user.Guid);
                LogService.Instance.InfoFormat("User Guid: {0} Disconnect from {1}", user.Guid, user.LastConnectedIPAddress);
            }
            PlayerFactory.Instance.PlayerOffline(user.Player);
        }
    }
}
