using IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers.PlayerResponseHandlers.FetchDataResponseHandlers;
using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers.PlayerResponseHandlers
{
    class PlayerFetchDataResponseResolver : FetchDataResponseResolver<Player, PlayerOperationCode, PlayerFetchDataCode>
    {
        public PlayerFetchDataResponseResolver(Player subject) : base(subject)
        {
            fetchResponseTable.Add(PlayerFetchDataCode.Inventory, new FetchInventoryResponseHandler(subject));
            fetchResponseTable.Add(PlayerFetchDataCode.InventoryItemInfos, new FetchInventoryItemInfosResponseHandler(subject));
        }
    }
}
