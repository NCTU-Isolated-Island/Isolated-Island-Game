using IsolatedIslandGame.Protocol.Communication.SyncDataCodes;
using IsolatedIslandGame.Protocol.Communication.SyncDataParameters.System;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.SystemEventHandlers.SyncDataHandlers
{
    class SyncIslandTodayMaterialRankingUpdatedHandler : SyncDataHandler<SystemManager, SystemSyncDataCode>
    {
        public SyncIslandTodayMaterialRankingUpdatedHandler(SystemManager subject) : base(subject, 2)
        {
        }
        internal override bool Handle(SystemSyncDataCode syncCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(syncCode, parameters))
            {
                try
                {
                    int playerID = (int)parameters[(byte)SyncIslandTodayMaterialRankingUpdatedParameterCode.PlayerID];
                    int materialItemID = (int)parameters[(byte)SyncIslandTodayMaterialRankingUpdatedParameterCode.MaterialItemID];

                    Island.Instance.UpdateTodayMaterialRanking(new Island.PlayerMaterialInfo { playerID = playerID, materialItemID = materialItemID });
                    return true;
                }
                catch (InvalidCastException ex)
                {
                    LogService.Error("SyncIslandTodayMaterialRankingUpdated Parameter Cast Error");
                    LogService.Error(ex.Message);
                    LogService.Error(ex.StackTrace);
                    return false;
                }
                catch (Exception ex)
                {
                    LogService.Error(ex.Message);
                    LogService.Error(ex.StackTrace);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
