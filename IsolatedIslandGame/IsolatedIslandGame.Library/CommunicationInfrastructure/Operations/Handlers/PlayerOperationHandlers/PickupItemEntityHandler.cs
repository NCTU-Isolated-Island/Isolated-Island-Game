using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.OperationParameters.Player;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.PlayerOperationHandlers
{
    class PickupItemEntityHandler : PlayerOperationHandler
    {
        public PickupItemEntityHandler(Player subject) : base(subject, 1)
        {
        }

        internal override bool Handle(PlayerOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(operationCode, parameters))
            {
                int itemEntityID = (int)parameters[(byte)PickupItemEntityParameterCode.ItemEntityID];

                lock (subject.Inventory)
                {
                    lock (ItemEntityManager.Instance)
                    {
                        ItemEntity itemEntity;
                        if (!ItemEntityManager.Instance.FindItemEntity(itemEntityID, out itemEntity))
                        {
                            LogService.Error($"PickupItemEntity error Player: {subject.IdentityInformation}, ItemEntity Not Exist ItemEntityID: {itemEntityID}");
                            subject.User.EventManager.UserInform("失敗", "物品不存在。");
                            return false;
                        }
                        else if (Math.Sqrt(Math.Pow(itemEntity.PositionX - subject.Vessel.LocationX, 2) + Math.Pow(itemEntity.PositionZ - subject.Vessel.LocationZ, 2)) > 50)
                        {
                            LogService.Error($"PickupItemEntity error Player: {subject.IdentityInformation}, ItemEntity Too Far ItemEntityID: {itemEntityID}");
                            subject.User.EventManager.UserInform("失敗", "距離太遠了。");
                            return false;
                        }
                        else if (subject.Inventory.AddItemCheck(itemEntity.ItemID, 1))
                        {
                            Item item;
                            if (ItemManager.Instance.FindItem(itemEntity.ItemID, out item))
                            {
                                subject.Inventory.AddItem(item, 1);
                                subject.User.EventManager.UserInform("提示", $"撿到了一個{item.ItemName}");
                                ItemEntityManager.Instance.RemoveItemEntity(itemEntityID);
                                return true;
                            }
                            else
                            {
                                LogService.Error($"PickupItemEntity error Player: {subject.IdentityInformation}, Item Not Existed ItemEntityID: {itemEntityID}");
                                subject.User.EventManager.UserInform("失敗", "那不是一個物品。");
                                return false;
                            }
                        }
                        else
                        {
                            LogService.Error($"PickupItemEntity error Player: {subject.IdentityInformation}, Inventory Full ItemEntityID: {itemEntityID}");
                            subject.User.EventManager.UserInform("失敗", "沒有空間放了。");
                            return false;
                        }
                    }
                }
            }
            else
            {
                return false;
            }
        }
    }
}
