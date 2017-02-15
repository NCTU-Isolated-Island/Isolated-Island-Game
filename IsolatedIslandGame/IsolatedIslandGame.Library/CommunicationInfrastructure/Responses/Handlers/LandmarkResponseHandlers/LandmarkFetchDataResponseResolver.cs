using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Library.Landmarks;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers.LandmarkResponseHandlers
{
    class LandmarkFetchDataResponseResolver : FetchDataResponseResolver<Landmark, LandmarkOperationCode, LandmarkFetchDataCode>
    {
        public LandmarkFetchDataResponseResolver(Landmark subject) : base(subject)
        {

        }
    }
}
