using IsolatedIslandGame.Protocol.Communication.SyncDataCodes;
using IsolatedIslandGame.Protocol.Communication.SyncDataParameters.System;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.SystemEventHandlers.SyncDataHandlers
{
    class SyncIslandPlayerScoreRankingUpdatedHandler : SyncDataHandler<SystemManager, SystemSyncDataCode>
    {
        public SyncIslandPlayerScoreRankingUpdatedHandler(SystemManager subject) : base(subject, 2)
        {
        }
        internal override bool Handle(SystemSyncDataCode syncCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(syncCode, parameters))
            {
                try
                {
                    int playerID = (int)parameters[(byte)SyncIslandPlayerScoreRankingUpdatedParameterCode.PlayerID];
                    int score = (int)parameters[(byte)SyncIslandPlayerScoreRankingUpdatedParameterCode.Score];

                    Island.Instance.UpdatePlayerScoreRanking(new Island.PlayerScoreInfo { playerID = playerID, score = score });
                    return true;
                }
                catch (InvalidCastException ex)
                {
                    LogService.Error("SyncIslandPlayerScoreRankingUpdated Parameter Cast Error");
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
