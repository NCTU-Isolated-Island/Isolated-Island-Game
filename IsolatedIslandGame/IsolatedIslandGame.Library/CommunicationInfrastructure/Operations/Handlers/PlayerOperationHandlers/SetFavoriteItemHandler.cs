using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.OperationParameters.Player;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.PlayerOperationHandlers
{
    class SetFavoriteItemHandler : PlayerOperationHandler
    {
        public SetFavoriteItemHandler(Player subject) : base(subject, 2)
        {
        }

        internal override bool Handle(PlayerOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(operationCode, parameters))
            {
                int inventoryID = (int)parameters[(byte)SetFavoriteItemParameterCode.InventoryID];
                int inventoryItemInfoID = (int)parameters[(byte)SetFavoriteItemParameterCode.InventoryItemInfoID];

                if(subject.Inventory.InventoryID == inventoryID)
                {
                    lock (subject.Inventory)
                    {
                        if (subject.Inventory.ContainsInventoryItemInfo(inventoryItemInfoID))
                        {
                            InventoryItemInfo info;
                            subject.Inventory.FindInventoryItemInfo(inventoryItemInfoID, out info);
                            return true;
                        }
                        else
                        {
                            LogService.ErrorFormat("SetFavoriteItem error Player: {0}, the ItemInfo is not existed InventoryItemInfoID: {1}", inventoryItemInfoID);
                            return false;
                        }
                    }
                }
                else
                {
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
