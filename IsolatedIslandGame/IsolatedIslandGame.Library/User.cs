using System;
using System.Net;

namespace IsolatedIslandGame.Library
{
    public class User
    {
        #region properties
        public Player Player { get; protected set; }
        private IPAddress lastConnectedIPAddress;
        public IPAddress LastConnectedIPAddress
        {
            get
            {
                return (IsOnline) ? Player.LastConnectedIPAddress : lastConnectedIPAddress;
            }
            protected set
            {
                lastConnectedIPAddress = value;
                if(IsOnline)
                {
                    Player.LastConnectedIPAddress = value;
                }
            }
        }
        public bool IsOnline { get { return Player != null; } }
        #endregion

        #region events
        private event Action<Player> onPlayerOnline;
        public event Action<Player> OnPlayerOnline
        {
            add { onPlayerOnline += value; }
            remove { onPlayerOnline -= value; }
        }
        private event Action<Player> onPlayerOffline;
        public event Action<Player> OnPlayerOffline
        {
            add { onPlayerOffline += value; }
            remove { onPlayerOffline -= value; }
        }
        #endregion

        #region methods
        public void PlayerOnline(Player player)
        {
            Player = player;
            onPlayerOnline?.Invoke(Player);
        }
        public void PlayerOffline()
        {
            onPlayerOffline?.Invoke(Player);
            Player = null;
        }
        #endregion
    }
}
