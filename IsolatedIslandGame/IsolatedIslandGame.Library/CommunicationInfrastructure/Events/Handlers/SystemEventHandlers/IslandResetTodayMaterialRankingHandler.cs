using IsolatedIslandGame.Protocol.Communication.EventCodes;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.SystemEventHandlers
{
    class IslandResetTodayMaterialRankingHandler : EventHandler<SystemManager, SystemEventCode>
    {
        public IslandResetTodayMaterialRankingHandler(SystemManager subject) : base(subject, 0)
        {
        }
        internal override bool Handle(SystemEventCode eventCode, Dictionary<byte, object> parameters)
        {
            if(base.Handle(eventCode, parameters))
            {
                Island.Instance.ResetTodayMaterialRanking();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
