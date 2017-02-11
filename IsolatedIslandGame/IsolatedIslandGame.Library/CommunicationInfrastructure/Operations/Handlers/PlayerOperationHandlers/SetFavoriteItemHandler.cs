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
                            info.IsFavorite = info.IsFavorite;
                            subject.Inventory.LoadItemInfo(info);
                            return true;
                        }
                        else
                        {
                            LogService.Error($"SetFavoriteItem error Player: {subject.IdentityInformation}, the ItemInfo is not existed InventoryItemInfoID: {inventoryItemInfoID}");
                            subject.User.EventManager.UserInform("錯誤", "操作的物品並不在物品欄中。");
                            return false;
                        }
                    }
                }
                else
                {
                    subject.User.EventManager.UserInform("錯誤", "操作的物品欄並非你所有。");
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
