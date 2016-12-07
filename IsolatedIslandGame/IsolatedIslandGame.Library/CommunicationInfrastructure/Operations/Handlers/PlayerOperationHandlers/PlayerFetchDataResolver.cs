using IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.PlayerOperationHandlers.FetchDataHandlers;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataParameters.Player;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers
{
    public class PlayerFetchDataResolver : FetchDataResolver<Player, PlayerOperationCode, PlayerFetchDataCode>
    {
        public PlayerFetchDataResolver(Player subject) : base(subject)
        {
            fetchTable.Add(PlayerFetchDataCode.Inventory, new FetchInventoryHandler(subject));
            fetchTable.Add(PlayerFetchDataCode.InventoryItemInfos, new FetchInventoryItemInfosHandler(subject));
        }

        internal override void SendResponse(PlayerOperationCode operationCode, Dictionary<byte, object> parameter)
        {
            subject.ResponseManager.SendResponse(operationCode, ErrorCode.NoError, null, parameter);
        }
        internal override void SendError(PlayerOperationCode operationCode, ErrorCode errorCode, string debugMessage)
        {
            base.SendError(operationCode, errorCode, debugMessage);
            Dictionary<byte, object> parameters = new Dictionary<byte, object>();
            subject.ResponseManager.SendResponse(operationCode, errorCode, debugMessage, parameters);
        }
        internal void SendOperation(PlayerFetchDataCode fetchCode, Dictionary<byte, object> parameters)
        {
            subject.OperationManager.SendFetchDataOperation(fetchCode, parameters);
        }

        public void FetchInventory()
        {
            SendOperation(PlayerFetchDataCode.Inventory, new Dictionary<byte, object>());
        }
        public void FetchInventoryItemInfos(int inventoryID)
        {
            var parameters = new Dictionary<byte, object>
            {
                { (byte)FetchInventoryItemInfosParameterCode.InventoryID, inventoryID },
            };
            SendOperation(PlayerFetchDataCode.InventoryItemInfos, parameters);
        }
    }
}
