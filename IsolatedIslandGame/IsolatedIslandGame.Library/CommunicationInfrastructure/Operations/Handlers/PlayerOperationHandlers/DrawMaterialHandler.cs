using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.ResponseParameters.Player;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.PlayerOperationHandlers
{
    class DrawMaterialHandler : PlayerOperationHandler
    {
        public DrawMaterialHandler(Player subject) : base(subject, 0)
        {
        }

        internal override bool Handle(PlayerOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(operationCode, parameters))
            {
                Random randomGenerator = new Random(DateTime.Now.GetHashCode());
                int resultCaseNumber = randomGenerator.Next(1, 101);
                IEnumerable<Material> materials = null;
                if (resultCaseNumber <= 60)
                {
                    materials = ItemManager.Instance.Items.OfType<Material>().Where(x => x.GroupType == subject.GroupType && x.Level == 1);
                }
                else if(resultCaseNumber <= 80)
                {
                    materials = ItemManager.Instance.Items.OfType<Material>().Where(x => x.GroupType != subject.GroupType && x.Level == 1);
                }
                else if(resultCaseNumber <= 90)
                {
                    materials = ItemManager.Instance.Items.OfType<Material>().Where(x => x.Level == 2);
                }
                else if(resultCaseNumber <= 97)
                {
                    materials = ItemManager.Instance.Items.OfType<Material>().Where(x => x.Level == 3);
                }
                else if (resultCaseNumber <= 99)
                {
                    materials = ItemManager.Instance.Items.OfType<Material>().Where(x => x.Level == 4);
                }
                else
                {
                    materials = ItemManager.Instance.Items.OfType<Material>().Where(x => x.Level == 1);
                }
                Item randomItem = materials.ElementAt(randomGenerator.Next(0, materials.Count()));
                int itemCount = 1;

                lock(subject.Inventory)
                {
                    if(DateTime.Now > subject.NextDrawMaterialTime && subject.Inventory.AddItemCheck(randomItem.ItemID, itemCount))
                    {
                        subject.Inventory.AddItem(randomItem, itemCount);
                        Dictionary<byte, object> responseParameters = new Dictionary<byte, object>
                        {
                            { (byte)DrawMaterialResponseParameterCode.ItemID, randomItem.ItemID },
                            { (byte)DrawMaterialResponseParameterCode.ItemCount, itemCount }
                        };
                        SendResponse(operationCode, responseParameters);
                        LogService.InfoFormat("Player: {0}, DrawMaterial, ItemID: {1}, ItemCount{2}", subject.IdentityInformation, randomItem.ItemID, itemCount);
                        DateTime nextDrawMaterialTime = DateTime.Today;
                        while(nextDrawMaterialTime < DateTime.Now)
                        {
                            nextDrawMaterialTime += TimeSpan.FromHours(6);
                        }
                        subject.NextDrawMaterialTime = nextDrawMaterialTime;
                        return true;
                    }
                    else if(DateTime.Now <= subject.NextDrawMaterialTime)
                    {
                        SendError(operationCode, ErrorCode.Fail, "DrawMaterial Fail");
                        LogService.ErrorFormat("Player: {0}, DrawMaterial Fail, ItemID: {1}, ItemCount{2}", subject.IdentityInformation, randomItem.ItemID, itemCount);
                        var remainedTimeSpan = subject.NextDrawMaterialTime - DateTime.Now;
                        subject.User.EventManager.UserInform("失敗", $"抽取素材失敗，距離下一次收取時間，剩餘{remainedTimeSpan.Hours}小時{remainedTimeSpan.Minutes}分{remainedTimeSpan.Seconds}秒。");
                        return false;
                    }
                    else
                    {
                        SendError(operationCode, ErrorCode.Fail, "DrawMaterial Fail");
                        LogService.ErrorFormat("Player: {0}, DrawMaterial Fail, ItemID: {1}, ItemCount{2}", subject.IdentityInformation, randomItem.ItemID, itemCount);
                        subject.User.EventManager.UserInform("失敗", "抽取素材失敗，物品欄已經滿了。");
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
