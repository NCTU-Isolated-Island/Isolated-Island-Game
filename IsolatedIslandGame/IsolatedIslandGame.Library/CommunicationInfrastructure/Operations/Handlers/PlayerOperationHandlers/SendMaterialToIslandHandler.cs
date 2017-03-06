using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.OperationParameters.Player;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.PlayerOperationHandlers
{
    class SendMaterialToIslandHandler : PlayerOperationHandler
    {
        public SendMaterialToIslandHandler(Player subject) : base(subject, 1)
        {
        }

        internal override bool Handle(PlayerOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(operationCode, parameters))
            {
                int materialItemID = (int)parameters[(byte)SendMaterialToIslandParameterCode.MaterialItemID];

                lock(subject.Inventory)
                {
                    Item item;
                    if(ItemManager.Instance.FindItem(materialItemID, out item) && item is Material && subject.Inventory.RemoveItemCheck(materialItemID, 1) && Island.Instance.SendMaterial(subject, item as Material))
                    {
                        if (Island.Instance.SendMaterial(subject, item as Material))
                        {
                            subject.Inventory.RemoveItem(materialItemID, 1);
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else if(!ItemManager.Instance.FindItem(materialItemID, out item))
                    {
                        subject.User.EventManager.UserInform("錯誤", "投送到島上的物品未被定義。");
                        return false;
                    }
                    else if (!(item is Material))
                    {
                        subject.User.EventManager.UserInform("錯誤", "投送到島上的物品並非素材。");
                        return false;
                    }
                    else
                    {
                        subject.User.EventManager.UserInform("錯誤", "你並沒有這個素材。");
                        return false;
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
