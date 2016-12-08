﻿using IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.PlayerEventHandlers.SyncDataHandlers;
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

        public void SyncInventoryItemInfoChange(InventoryItemInfo info, DataChangeType changeType)
        {
            var parameters = new Dictionary<byte, object>
            {
                { (byte)SyncInventoryItemInfoChangeParameterCode.InventoryID, subject.Inventory.InventoryID },
                { (byte)SyncInventoryItemInfoChangeParameterCode.InventoryItemInfoID, info.InventoryItemInfoID },
                { (byte)SyncInventoryItemInfoChangeParameterCode.ItemID, info.Item.ItemID },
                { (byte)SyncInventoryItemInfoChangeParameterCode.ItemCount, info.Count },
                { (byte)SyncInventoryItemInfoChangeParameterCode.PositionIndex, info.PositionIndex },
                { (byte)SyncInventoryItemInfoChangeParameterCode.DataChangeType, (byte)changeType }
            };
            SendSyncData(PlayerSyncDataCode.InventoryItemInfoChange, parameters);
        }
    }
}
