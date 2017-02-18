using IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers.SystemResponseHandlers.FetchDataResponseHandlers;
using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers.SystemResponseHandlers
{
    class SystemFetchDataResponseResolver : FetchDataResponseResolver<SystemManager, SystemOperationCode, SystemFetchDataCode>
    {
        public SystemFetchDataResponseResolver(SystemManager subject) : base(subject)
        {
            fetchResponseTable.Add(SystemFetchDataCode.Item, new FetchItemResponseHandler(subject));
            fetchResponseTable.Add(SystemFetchDataCode.Vessel, new FetchVesselResponseHandler(subject));
            fetchResponseTable.Add(SystemFetchDataCode.VesselWithOwnerPlayerID, new FetchVesselWithOwnerPlayerIDResponseHandler(subject));
            fetchResponseTable.Add(SystemFetchDataCode.VesselDecorations, new FetchVesselDecorationsResponseHandler(subject));
            fetchResponseTable.Add(SystemFetchDataCode.IslandTotalScore, new FetchIslandTotalScoreResponseHandler(subject));
            fetchResponseTable.Add(SystemFetchDataCode.IslandTodayMaterialRanking, new FetchIslandTodayMaterialRankingResponseHandler(subject));
            fetchResponseTable.Add(SystemFetchDataCode.IslandPlayerScoreRanking, new FetchIslandPlayerScoreRankingResponseHandler(subject));
            fetchResponseTable.Add(SystemFetchDataCode.AllLandmarks, new FetchAllLandmarksResponseHandler(subject));
        }
    }
}
