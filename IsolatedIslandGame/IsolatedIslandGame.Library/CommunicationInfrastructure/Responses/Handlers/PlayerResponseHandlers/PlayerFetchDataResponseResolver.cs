using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers.PlayerResponseHandlers
{
    class PlayerFetchDataResponseResolver : FetchDataResponseResolver<Player, PlayerOperationCode, PlayerFetchDataCode>
    {
        public PlayerFetchDataResponseResolver(Player subject) : base(subject)
        {

        }
    }
}
