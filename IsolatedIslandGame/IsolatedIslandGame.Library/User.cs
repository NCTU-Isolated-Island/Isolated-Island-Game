using IsolatedIslandGame.Library.CommunicationInfrastructure;
using IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Managers;
using IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Managers;
using IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Managers;
using System;
using System.Net;

namespace IsolatedIslandGame.Library
{
    public class User : IIdentityProvidable
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

        internal UserCommunicationInterface UserCommunicationInterface { get; private set; }
        public UserEventManager EventManager { get; private set; }
        public UserOperationManager OperationManager { get; private set; }
        public UserResponseManager ResponseManager { get; private set; }

        public string IdentityInformation { get { return string.Format("User IP: {0}", LastConnectedIPAddress); } }
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
        public User(UserCommunicationInterface communicationInterface)
        {
            EventManager = new UserEventManager(this);
            OperationManager = new UserOperationManager(this);
            ResponseManager = new UserResponseManager(this);
            UserCommunicationInterface = communicationInterface;
            UserCommunicationInterface.BindUser(this);
        }
        public void PlayerOnline(Player player)
        {
            Player = player;
            if(player.LastConnectedIPAddress == null)
            {
                player.LastConnectedIPAddress = lastConnectedIPAddress;
            }
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
