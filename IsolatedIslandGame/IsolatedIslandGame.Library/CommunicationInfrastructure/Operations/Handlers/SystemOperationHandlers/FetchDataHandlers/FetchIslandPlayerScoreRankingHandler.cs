using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataResponseParameters.System;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.SystemOperationHandlers.FetchDataHandlers
{
    class FetchIslandPlayerScoreRankingHandler : SystemFetchDataHandler
    {
        public FetchIslandPlayerScoreRankingHandler(SystemManager subject) : base(subject, 0)
        {
        }

        public override bool Handle(CommunicationInterface communicationInterface, SystemFetchDataCode fetchCode, Dictionary<byte, object> parameter)
        {
            if (base.Handle(communicationInterface, fetchCode, parameter))
            {
                try
                {
                    List<int> playerIDs = new List<int>();
                    List<int> scores = new List<int>();
                    foreach(var info in Island.Instance.PlayerScoreRanking)
                    {
                        communicationInterface.User?.Player.SyncPlayerInformation(info.playerID);
                        playerIDs.Add(info.playerID);
                        scores.Add(info.score);
                    }
                    var result = new Dictionary<byte, object>
                    {
                        { (byte)FetchIslandPlayerScoreRankingResponseParameterCode.PlayerIDArray, playerIDs.ToArray() },
                        { (byte)FetchIslandPlayerScoreRankingResponseParameterCode.ScoreArray, scores.ToArray() }
                    };
                    SendResponse(communicationInterface, fetchCode, result);
                    return true;
                }
                catch (InvalidCastException ex)
                {
                    LogService.ErrorFormat("FetchIslandPlayerScoreRanking Invalid Cast!");
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
