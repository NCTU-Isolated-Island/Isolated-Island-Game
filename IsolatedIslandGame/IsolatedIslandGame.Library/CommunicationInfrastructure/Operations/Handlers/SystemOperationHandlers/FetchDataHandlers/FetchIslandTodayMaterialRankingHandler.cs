using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataResponseParameters.System;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.SystemOperationHandlers.FetchDataHandlers
{
    class FetchIslandTodayMaterialRankingHandler : SystemFetchDataHandler
    {
        public FetchIslandTodayMaterialRankingHandler(SystemManager subject) : base(subject, 0)
        {
        }

        public override bool Handle(CommunicationInterface communicationInterface, SystemFetchDataCode fetchCode, Dictionary<byte, object> parameter)
        {
            if (base.Handle(communicationInterface, fetchCode, parameter))
            {
                try
                {
                    List<int> playerIDs = new List<int>();
                    List<int> materialItemIDs = new List<int>();
                    foreach (var info in Island.Instance.TodayMaterialRanking)
                    {
                        communicationInterface.User?.Player.SyncPlayerInformation(info.playerID);
                        playerIDs.Add(info.playerID);
                        materialItemIDs.Add(info.materialItemID);
                    }
                    var result = new Dictionary<byte, object>
                    {
                        { (byte)FetchIslandTodayMaterialRankingResponseParameterCode.PlayerIDArray, playerIDs.ToArray() },
                        { (byte)FetchIslandTodayMaterialRankingResponseParameterCode.MaterialItemIDArray, materialItemIDs.ToArray() }
                    };
                    SendResponse(communicationInterface, fetchCode, result);
                    return true;
                }
                catch (InvalidCastException ex)
                {
                    LogService.ErrorFormat("FetchIslandTodayMaterialRanking Invalid Cast!");
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
