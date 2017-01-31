using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataResponseParameters.System;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers.SystemResponseHandlers.FetchDataResponseHandlers
{
    class FetchIslandPlayerScoreRankingResponseHandler : FetchDataResponseHandler<SystemManager, SystemFetchDataCode>
    {
        public FetchIslandPlayerScoreRankingResponseHandler(SystemManager subject) : base(subject)
        {
        }

        public override bool CheckError(Dictionary<byte, object> parameters, ErrorCode returnCode, string debugMessage)
        {
            switch (returnCode)
            {
                case ErrorCode.NoError:
                    {
                        if (parameters.Count != 2)
                        {
                            LogService.ErrorFormat(string.Format("FetchIslandPlayerScoreRankingResponse Parameter Error, Parameter Count: {0}", parameters.Count));
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                default:
                    {
                        LogService.ErrorFormat("FetchIslandPlayerScoreRankingResponse Error DebugMessage: {0}", debugMessage);
                        return false;
                    }
            }
        }

        public override bool Handle(SystemFetchDataCode fetchCode, ErrorCode returnCode, string fetchDebugMessage, Dictionary<byte, object> parameters)
        {
            if (base.Handle(fetchCode, returnCode, fetchDebugMessage, parameters))
            {
                try
                {
                    int[] playerIDArray = (int[])parameters[(byte)FetchIslandPlayerScoreRankingResponseParameterCode.PlayerIDArray];
                    int[] scoreArray = (int[])parameters[(byte)FetchIslandPlayerScoreRankingResponseParameterCode.ScoreArray];
                    if(playerIDArray.Length != scoreArray.Length)
                    {
                        LogService.Error("FetchIslandPlayerScoreRankingResponse Error ArraySize Incorrect");
                        return false;
                    }
                    else
                    {
                        for(int i = 0; i < playerIDArray.Length; i++)
                        {
                            Island.Instance.UpdatePlayerScoreRanking(new Island.PlayerScoreInfo { playerID = playerIDArray[i], score = scoreArray[i] });
                        }
                        return true;
                    }
                }
                catch (InvalidCastException ex)
                {
                    LogService.Error("FetchIslandPlayerScoreRankingResponse Parameter Cast Error");
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
