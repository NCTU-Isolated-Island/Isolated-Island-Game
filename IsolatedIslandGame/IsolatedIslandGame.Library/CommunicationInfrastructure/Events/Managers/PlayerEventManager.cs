using IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers;
using IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.PlayerEventHandlers;
using IsolatedIslandGame.Protocol.Communication.EventCodes;
using IsolatedIslandGame.Protocol.Communication.EventParameters;
using IsolatedIslandGame.Protocol.Communication.InformDataCodes;
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
            player.User.EventManager.SendPlayerEvent(player, eventCode, parameters);
        }

        public void ErrorInform(string title, string message)
        {
            player.User.EventManager.ErrorInform(title, message);
        }

        internal void SendInformDataEvent(PlayerInformDataCode informCode, Dictionary<byte, object> parameters)
        {
            Dictionary<byte, object> informDataParameters = new Dictionary<byte, object>
            {
                { (byte)InformDataEventParameterCode.InformCode, (byte)informCode },
                { (byte)InformDataEventParameterCode.Parameters, parameters }
            };
            SendEvent(PlayerEventCode.InformData, informDataParameters);
        }
    }
}
