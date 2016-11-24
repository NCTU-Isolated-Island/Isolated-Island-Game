using IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers;
using IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.PlayerEventHandlers;
using IsolatedIslandGame.Protocol.Communication.EventCodes;
using IsolatedIslandGame.Protocol.Communication.EventParameters.User;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Managers
{
    public class PlayerEventManager
    {
        private readonly Dictionary<PlayerEventCode, EventHandler<Player, PlayerEventCode>> eventTable;
        protected readonly Player player;
        public PlayerInformDataResolver InformDataResolver { get; protected set; }

        internal PlayerEventManager(Player player)
        {
            this.player = player;
            InformDataResolver = new PlayerInformDataResolver(player);
            eventTable = new Dictionary<PlayerEventCode, EventHandler<Player, PlayerEventCode>>
            {
                { PlayerEventCode.InformData, InformDataResolver },
            };
        }

        internal void Operate(PlayerEventCode eventCode, Dictionary<byte, object> parameters)
        {
            if (eventTable.ContainsKey(eventCode))
            {
                if (!eventTable[eventCode].Handle(eventCode, parameters))
                {
                    LogService.ErrorFormat("Player Event Error: {0} from PlayerID: {1}", eventCode, player.PlayerID);
                }
            }
            else
            {
                LogService.ErrorFormat("Unknow Player Event:{0} from PlayerID: {1}", eventCode, player.PlayerID);
            }
        }

        internal void SendEvent(PlayerEventCode eventCode, Dictionary<byte, object> parameters)
        {
            Dictionary<byte, object> eventData = new Dictionary<byte, object>
            {
                { (byte)PlayerEventParameterCode.PlayerID, player.PlayerID },
                { (byte)PlayerEventParameterCode.EventCode, (byte)eventCode },
                { (byte)PlayerEventParameterCode.Parameters, parameters }
            };
            player.User.EventManager.SendEvent(UserEventCode.PlayerEvent, eventData);
        }

        public void ErrorInform(string title, string message)
        {
            player.User.EventManager.ErrorInform(title, message);
        }
    }
}
