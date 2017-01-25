using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.SyncDataCodes;
using IsolatedIslandGame.Protocol.Communication.SyncDataParameters.Player;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.PlayerEventHandlers.SyncDataHandlers
{
    class SyncInventoryItemInfoChangeHandler : SyncDataHandler<Player, PlayerSyncDataCode>
    {
        public SyncInventoryItemInfoChangeHandler(Player subject) : base(subject, 7)
        {
        }
        internal override bool Handle(PlayerSyncDataCode syncCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(syncCode, parameters))
            {
                try
                {
                    DataChangeType changeType = (DataChangeType)parameters[(byte)SyncInventoryItemInfoChangeParameterCode.DataChangeType];
                    int inventoryID = (int)parameters[(byte)SyncInventoryItemInfoChangeParameterCode.InventoryID];
                    int inventoryItemInfoID = (int)parameters[(byte)SyncInventoryItemInfoChangeParameterCode.InventoryItemInfoID];
                    int itemID = (int)parameters[(byte)SyncInventoryItemInfoChangeParameterCode.ItemID];
                    int itemCount = (int)parameters[(byte)SyncInventoryItemInfoChangeParameterCode.ItemCount];
                    int positionIndex = (int)parameters[(byte)SyncInventoryItemInfoChangeParameterCode.PositionIndex];
                    bool isFavorite = (bool)parameters[(byte)SyncInventoryItemInfoChangeParameterCode.IsFavorite];

                    if (subject.Inventory.InventoryID == inventoryID)
                    {
                        Item item;
                        if(ItemManager.Instance.FindItem(itemID, out item))
                        {
                            InventoryItemInfo info = new InventoryItemInfo(
                            inventoryItemInfoID: inventoryItemInfoID,
                            item: item,
                            count: itemCount,
                            positionIndex: positionIndex,
                            isFavorite: isFavorite);
                            switch (changeType)
                            {
                                case DataChangeType.Add:
                                case DataChangeType.Update:
                                    subject.Inventory.LoadItemInfo(info);
                                    break;
                                case DataChangeType.Remove:
                                    subject.Inventory.RemoveItemInfo(info.InventoryItemInfoID);
                                    break;
                                default:
                                    LogService.Error("SyncInventoryItemInfoChange Error Undefined DataChangeType");
                                    return false;
                            }
                            return true;
                        }
                        else
                        {
                            LogService.Error($"SyncInventoryItemInfoChange Error Item not existed ItemID: {itemID}");
                            return false;
                        }
                    }
                    else
                    {
                        LogService.Error($"SyncInventoryItemInfoChange Error InventoryID incorrect, self: {subject.Inventory.InventoryID}, received: {inventoryID}");
                        return false;
                    }
                }
                catch (InvalidCastException ex)
                {
                    LogService.Error("SyncInventoryItemInfoChange Parameter Cast Error");
                    LogService.Error(ex.Message);
                    LogService.Error(ex.StackTrace);
                    return false;
                }
                catch (Exception ex)
                {
                    LogService.Error(ex.Message);
                    LogService.Error(ex.StackTrace);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
