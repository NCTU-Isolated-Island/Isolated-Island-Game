using IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers.LandmarkResponseHandlers.FetchDataResponseHandlers;
using IsolatedIslandGame.Library.Landmarks;
using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers.LandmarkResponseHandlers
{
    class LandmarkFetchDataResponseResolver : FetchDataResponseResolver<Landmark, LandmarkOperationCode, LandmarkFetchDataCode>
    {
        public LandmarkFetchDataResponseResolver(Landmark subject) : base(subject)
        {
            fetchResponseTable.Add(LandmarkFetchDataCode.AllLandmarkRooms, new FetchAllLandmarkRoomsResponseHandler(subject));
        }
    }
}
