using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataParameters.Player;
using IsolatedIslandGame.Protocol.Communication.FetchDataResponseParameters.Player;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.PlayerOperationHandlers.FetchDataHandlers
{
    class FetchInventoryItemInfosHandler : PlayerFetchDataHandler
    {
        public FetchInventoryItemInfosHandler(Player subject) : base(subject, 1)
        {
        }

        public override bool Handle(PlayerFetchDataCode fetchCode, Dictionary<byte, object> parameter)
        {
            if (base.Handle(fetchCode, parameter))
            {
                try
                {
                    int inventoryID = (int)parameter[(byte)FetchInventoryItemInfosParameterCode.InventoryID];
                    if (subject.Inventory.InventoryID == inventoryID)
                    {
                        Inventory inventory = subject.Inventory;
                        foreach(var info in inventory.ItemInfos)
                        {
                            var result = new Dictionary<byte, object>
                            {
                                { (byte)FetchInventoryItemInfosResponseParameterCode.InventoryID, inventory.InventoryID },
                                { (byte)FetchInventoryItemInfosResponseParameterCode.InventoryItemInfoID, info.InventoryItemInfoID },
                                { (byte)FetchInventoryItemInfosResponseParameterCode.ItemID, info.Item.ItemID },
                                { (byte)FetchInventoryItemInfosResponseParameterCode.ItemCount, info.Count },
                                { (byte)FetchInventoryItemInfosResponseParameterCode.PositionIndex, info.PositionIndex },
                                { (byte)FetchInventoryItemInfosResponseParameterCode.IsUsing, info.IsUsing }
                            };
                            SendResponse(fetchCode, result);
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (InvalidCastException ex)
                {
                    LogService.ErrorFormat("Fetch System Version Invalid Cast!");
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
