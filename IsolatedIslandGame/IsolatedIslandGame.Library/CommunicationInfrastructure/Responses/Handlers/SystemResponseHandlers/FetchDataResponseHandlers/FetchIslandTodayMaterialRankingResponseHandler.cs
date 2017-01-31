using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataResponseParameters.System;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers.SystemResponseHandlers.FetchDataResponseHandlers
{
    class FetchIslandTodayMaterialRankingResponseHandler : FetchDataResponseHandler<SystemManager, SystemFetchDataCode>
    {
        public FetchIslandTodayMaterialRankingResponseHandler(SystemManager subject) : base(subject)
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
                            LogService.ErrorFormat(string.Format("FetchIslandTodayMaterialRankingResponse Parameter Error, Parameter Count: {0}", parameters.Count));
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                default:
                    {
                        LogService.ErrorFormat("FetchIslandTodayMaterialRankingResponse Error DebugMessage: {0}", debugMessage);
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
                    int[] playerIDArray = (int[])parameters[(byte)FetchIslandTodayMaterialRankingResponseParameterCode.PlayerIDArray];
                    int[] materialItemIDArray = (int[])parameters[(byte)FetchIslandTodayMaterialRankingResponseParameterCode.MaterialItemIDArray];
                    if (playerIDArray.Length != materialItemIDArray.Length)
                    {
                        LogService.Error("FetchIslandTodayMaterialRankingResponse Error ArraySize Incorrect");
                        return false;
                    }
                    else
                    {
                        for (int i = 0; i < playerIDArray.Length; i++)
                        {
                            Island.Instance.UpdateTodayMaterialRanking(new Island.PlayerMaterialInfo { playerID = playerIDArray[i], materialItemID = materialItemIDArray[i] });
                        }
                        return true;
                    }
                }
                catch (InvalidCastException ex)
                {
                    LogService.Error("FetchIslandTodayMaterialRankingResponse Parameter Cast Error");
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
