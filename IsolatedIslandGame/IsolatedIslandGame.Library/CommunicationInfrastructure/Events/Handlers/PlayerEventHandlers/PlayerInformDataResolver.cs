using IsolatedIslandGame.Protocol.Communication.EventCodes;
using IsolatedIslandGame.Protocol.Communication.InformDataCodes;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.PlayerEventHandlers
{
    public class PlayerInformDataResolver : InformDataResolver<Player, PlayerEventCode, PlayerInformDataCode>
    {
        internal PlayerInformDataResolver(Player player) : base(player)
        {
        }

        internal override void SendInform(PlayerInformDataCode informCode, Dictionary<byte, object> parameters)
        {
            subject.EventManager.SendInformDataEvent(informCode, parameters);
        }
    }
}
