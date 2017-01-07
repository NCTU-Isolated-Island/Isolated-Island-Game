using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataResponseParameters.Player;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers.PlayerResponseHandlers.FetchDataResponseHandlers
{
    class FetchInventoryItemInfosResponseHandler : FetchDataResponseHandler<Player, PlayerFetchDataCode>
    {
        public FetchInventoryItemInfosResponseHandler(Player subject) : base(subject)
        {
        }

        public override bool CheckError(Dictionary<byte, object> parameters, ErrorCode returnCode, string debugMessage)
        {
            switch (returnCode)
            {
                case ErrorCode.NoError:
                    {
                        if (parameters.Count != 5)
                        {
                            LogService.ErrorFormat(string.Format("FetchInventoryItemInfosResponse Parameter Error, Parameter Count: {0}", parameters.Count));
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                default:
                    {
                        LogService.ErrorFormat("FetchInventoryItemInfosResponse Error DebugMessage: {0}", debugMessage);
                        return false;
                    }
            }
        }

        public override bool Handle(PlayerFetchDataCode fetchCode, ErrorCode returnCode, string fetchDebugMessage, Dictionary<byte, object> parameters)
        {
            if (base.Handle(fetchCode, returnCode, fetchDebugMessage, parameters))
            {
                try
                {
                    int inventoryID = (int)parameters[(byte)FetchInventoryItemInfosResponseParameterCode.InventoryID];
                    int inventoryItemInfoID = (int)parameters[(byte)FetchInventoryItemInfosResponseParameterCode.InventoryItemInfoID];
                    int itemID = (int)parameters[(byte)FetchInventoryItemInfosResponseParameterCode.ItemID];
                    int itemCount = (int)parameters[(byte)FetchInventoryItemInfosResponseParameterCode.ItemCount];
                    int positionIndex = (int)parameters[(byte)FetchInventoryItemInfosResponseParameterCode.PositionIndex];
                    if(subject.Inventory.InventoryID == inventoryID)
                    {
                        Item item;
                        if(ItemManager.Instance.FindItem(itemID, out item))
                        {
                            subject.Inventory.LoadItemInfo(new InventoryItemInfo(
                            inventoryItemInfoID: inventoryItemInfoID,
                            item: item,
                            count: itemCount,
                            positionIndex: positionIndex));
                            return true;
                        }
                        else
                        {
                            LogService.Error($"FetchInventoryItemInfosResponse Error, Item not existed ItemID: {itemID}");
                            return false;
                        }
                    }
                    else
                    {
                        LogService.Error($"FetchInventoryItemInfosResponse Error, InventoryID incorrect, self: {subject.Inventory.InventoryID}, received: {inventoryID}");
                        return false;
                    }
                }
                catch (InvalidCastException ex)
                {
                    LogService.Error("FetchInventoryItemInfosResponse Parameter Cast Error");
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
