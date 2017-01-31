using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.SyncDataCodes;
using IsolatedIslandGame.Protocol.Communication.SyncDataParameters.System;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.SystemEventHandlers.SyncDataHandlers
{
    class SyncIslandTotalScoreUpdatedHandler : SyncDataHandler<SystemManager, SystemSyncDataCode>
    {
        public SyncIslandTotalScoreUpdatedHandler(SystemManager subject) : base(subject, 2)
        {
        }
        internal override bool Handle(SystemSyncDataCode syncCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(syncCode, parameters))
            {
                try
                {
                    GroupType groupType = (GroupType)parameters[(byte)SyncIslandTotalScoreUpdatedParameterCode.GroupType];
                    int totalScore = (int)parameters[(byte)SyncIslandTotalScoreUpdatedParameterCode.TotalScore];

                    Island.Instance.UpdateTotalScore(groupType, totalScore);
                    return true;
                }
                catch (InvalidCastException ex)
                {
                    LogService.Error("SyncIslandTotalScoreUpdated Parameter Cast Error");
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
