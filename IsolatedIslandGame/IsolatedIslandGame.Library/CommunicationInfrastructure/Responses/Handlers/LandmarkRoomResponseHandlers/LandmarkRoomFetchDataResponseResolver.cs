using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Library.Landmarks;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers.LandmarkRoomResponseHandlers
{
    class LandmarkRoomFetchDataResponseResolver : FetchDataResponseResolver<LandmarkRoom, LandmarkRoomOperationCode, LandmarkRoomFetchDataCode>
    {
        public LandmarkRoomFetchDataResponseResolver(LandmarkRoom subject) : base(subject)
        {

        }
    }
}
