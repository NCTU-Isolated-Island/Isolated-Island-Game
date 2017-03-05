//using IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers.LandmarkRoomResponseHandlers.FetchDataResponseHandlers;
using IsolatedIslandGame.Library.Landmarks;
using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers.LandmarkRoomResponseHandlers
{
    class LandmarkRoomFetchDataResponseResolver : FetchDataResponseResolver<LandmarkRoom, LandmarkRoomOperationCode, LandmarkRoomFetchDataCode>
    {
        public LandmarkRoomFetchDataResponseResolver(LandmarkRoom subject) : base(subject)
        {
            //fetchResponseTable.Add(LandmarkRoomFetchDataCode.AllMultiplayerSynthesizeParticipantInfos, new FetchAllMultiplayerSynthesizeParticipantInfosResponseHandler(subject));
        }
    }
}
