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
                if (resultCaseNumber <= 15)
                {
                    materials = ItemManager.Instance.Items.OfType<Material>().Where(x => x.Level == 0);
                }
                else if(resultCaseNumber <= 75)
                {
                    materials = ItemManager.Instance.Items.OfType<Material>().Where(x => x.Level == 1);
                    if(randomGenerator.Next(1,101) <= 80)
                    {
                        materials = materials.Where(x => x.GroupType == subject.GroupType);
                    }
                    else
                    {
                        materials = materials.Where(x => x.GroupType != subject.GroupType);
                    }
                }
                else if(resultCaseNumber <= 95)
                {
                    materials = ItemManager.Instance.Items.OfType<Material>().Where(x => x.Level == 2);
                    if (randomGenerator.Next(1, 101) <= 70)
                    {
                        materials = materials.Where(x => x.GroupType == subject.GroupType);
                    }
                    else
                    {
                        materials = materials.Where(x => x.GroupType != subject.GroupType);
                    }
                }
                else
                {
                    materials = ItemManager.Instance.Items.OfType<Material>().Where(x => x.Level == 3);
                    if (randomGenerator.Next(1, 101) <= 60)
                    {
                        materials = materials.Where(x => x.GroupType == subject.GroupType);
                    }
                    else
                    {
                        materials = materials.Where(x => x.GroupType != subject.GroupType);
                    }
                }
                Item randomItem = materials.ElementAt(randomGenerator.Next(0, materials.Count()));
                int itemCount = 1;
                DateTime drawTime = DateTime.Now;
                lock(subject.Inventory)
                {
                    if(drawTime >= subject.NextDrawMaterialTime && subject.Inventory.AddItemCheck(randomItem.ItemID, itemCount))
                    {
                        subject.Inventory.AddItem(randomItem, itemCount);
                        Dictionary<byte, object> responseParameters = new Dictionary<byte, object>
                        {
                            { (byte)DrawMaterialResponseParameterCode.ItemID, randomItem.ItemID },
                            { (byte)DrawMaterialResponseParameterCode.ItemCount, itemCount }
                        };
                        SendResponse(operationCode, responseParameters);
                        LogService.InfoFormat("Player: {0}, DrawMaterial, ItemID: {1}, ItemCount{2}", subject.IdentityInformation, randomItem.ItemID, itemCount);
                        subject.NextDrawMaterialTime = ItemManager.Instance.NextDrawMaterialTime + ItemManager.Instance.NextDrawMaterialTimeSpan;
                        subject.DrawMaterial();
                        return true;
                    }
                    else if(drawTime < subject.NextDrawMaterialTime)
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
