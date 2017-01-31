using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataResponseParameters.System;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.SystemOperationHandlers.FetchDataHandlers
{
    class FetchIslandTotalScoreHandler : SystemFetchDataHandler
    {
        public FetchIslandTotalScoreHandler(SystemManager subject) : base(subject, 0)
        {
        }

        public override bool Handle(CommunicationInterface communicationInterface, SystemFetchDataCode fetchCode, Dictionary<byte, object> parameter)
        {
            if (base.Handle(communicationInterface, fetchCode, parameter))
            {
                try
                {
                    for(GroupType groupType = GroupType.Animal; groupType <= GroupType.Farmer; groupType++)
                    {
                        int totalScore = Island.Instance.GetTotalScore(groupType);
                        var result = new Dictionary<byte, object>
                        {
                            { (byte)FetchIslandTotalScoreResponseParameterCode.GroupType, (byte)groupType },
                            { (byte)FetchIslandTotalScoreResponseParameterCode.TotalScore, totalScore }
                        };
                        SendResponse(communicationInterface, fetchCode, result);
                    }
                    return true;
                }
                catch (InvalidCastException ex)
                {
                    LogService.ErrorFormat("FetchIslandTotalScore Invalid Cast!");
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
