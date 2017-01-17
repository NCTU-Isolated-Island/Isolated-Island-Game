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
            fetchTable.Add(PlayerFetchDataCode.Vessel, new FetchVesselHandler(subject));
            fetchTable.Add(PlayerFetchDataCode.VesselDecorations, new FetchVesselDecorationsHandler(subject));
            fetchTable.Add(PlayerFetchDataCode.AllKnownBlueprints, new FetchAllKnownBlueprintsHandler(subject));
            fetchTable.Add(PlayerFetchDataCode.FriendInformations, new FetchFriendInformationsHandler(subject));
            fetchTable.Add(PlayerFetchDataCode.AllPlayerConversations, new FetchAllPlayerConversationsHandler(subject));
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
        public void FetchVessel()
        {
            SendOperation(PlayerFetchDataCode.Vessel, new Dictionary<byte, object>());
        }
        public void FetchVesselDecorations(int vesselID)
        {
            var parameters = new Dictionary<byte, object>
            {
                { (byte)FetchVesselDecorationsParameterCode.VesselID, vesselID },
            };
            SendOperation(PlayerFetchDataCode.VesselDecorations, parameters);
        }
        public void FetchAllKnownBlueprints()
        {
            SendOperation(PlayerFetchDataCode.AllKnownBlueprints, new Dictionary<byte, object>());
        }
        public void FetchFriendInformations()
        {
            SendOperation(PlayerFetchDataCode.FriendInformations, new Dictionary<byte, object>());
        }
        public void FetchAllPlayerConversations()
        {
            SendOperation(PlayerFetchDataCode.AllPlayerConversations, new Dictionary<byte, object>());
        }
    }
}
