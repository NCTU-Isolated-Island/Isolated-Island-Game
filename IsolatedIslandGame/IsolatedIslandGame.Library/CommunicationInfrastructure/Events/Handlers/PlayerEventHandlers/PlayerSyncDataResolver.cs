using IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.PlayerEventHandlers.SyncDataHandlers;
using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.EventCodes;
using IsolatedIslandGame.Protocol.Communication.SyncDataCodes;
using IsolatedIslandGame.Protocol.Communication.SyncDataParameters.Player;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.PlayerEventHandlers
{
    public class PlayerSyncDataResolver : SyncDataResolver<Player, PlayerEventCode, PlayerSyncDataCode>
    {
        internal PlayerSyncDataResolver(Player player) : base(player)
        {
            syncTable.Add(PlayerSyncDataCode.InventoryItemInfoChange, new SyncInventoryItemInfoChangeHandler(subject));
            
        }

        internal override void SendSyncData(PlayerSyncDataCode syncCode, Dictionary<byte, object> parameters)
        {
            subject.EventManager.SendSyncDataEvent(syncCode, parameters);
        }

        public void SyncInventoryItemInfoChange(DataChangeType changeType, InventoryItemInfo info)
        {
            var parameters = new Dictionary<byte, object>
            {
                { (byte)SyncInventoryItemInfoChangeParameterCode.DataChangeType, (byte)changeType },
                { (byte)SyncInventoryItemInfoChangeParameterCode.InventoryID, subject.Inventory.InventoryID },
                { (byte)SyncInventoryItemInfoChangeParameterCode.InventoryItemInfoID, info.InventoryItemInfoID },
                { (byte)SyncInventoryItemInfoChangeParameterCode.ItemID, info.Item.ItemID },
                { (byte)SyncInventoryItemInfoChangeParameterCode.ItemCount, info.Count },
                { (byte)SyncInventoryItemInfoChangeParameterCode.PositionIndex, info.PositionIndex }
            };
            SendSyncData(PlayerSyncDataCode.InventoryItemInfoChange, parameters);
        }
        public void SyncFriendInformationChange(DataChangeType changeType, FriendInformation information)
        {
            var parameters = new Dictionary<byte, object>
            {
                { (byte)SyncFriendInformationChangeParameterCode.DataChangeType, (byte)changeType },
                { (byte)SyncFriendInformationChangeParameterCode.PlayerID, information.playerInformation.playerID },
                { (byte)SyncFriendInformationChangeParameterCode.Nickname, information.playerInformation.nickname },
                { (byte)SyncFriendInformationChangeParameterCode.Signature, information.playerInformation.signature },
                { (byte)SyncFriendInformationChangeParameterCode.GroupType, (byte)information.playerInformation.groupType },
                { (byte)SyncFriendInformationChangeParameterCode.VesselID, information.playerInformation.vesselID },
                { (byte)SyncFriendInformationChangeParameterCode.IsSender, information.isSender },
                { (byte)SyncFriendInformationChangeParameterCode.IsConfirmed, information.isConfirmed }
            };
            SendSyncData(PlayerSyncDataCode.FriendInformationChange, parameters);
        }
    }
}
