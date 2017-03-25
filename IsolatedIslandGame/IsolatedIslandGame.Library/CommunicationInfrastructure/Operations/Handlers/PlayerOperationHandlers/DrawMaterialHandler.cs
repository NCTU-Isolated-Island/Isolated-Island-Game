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
                int itemCount = 2;
                List<Item> drawedItems = new List<Item>();
                for(int i = 0; i < itemCount; i++)
                {
                    drawedItems.Add(DrawAMaterial());
                }
                DateTime drawTime = DateTime.Now;
                lock(subject.Inventory)
                {
                    if(drawTime >= subject.NextDrawMaterialTime)
                    {
                        drawedItems.ForEach(x => 
                        {
                            subject.Inventory.AddItem(x, 1);
                            Dictionary<byte, object> responseParameters = new Dictionary<byte, object>
                            {
                                { (byte)DrawMaterialResponseParameterCode.ItemID, x.ItemID },
                                { (byte)DrawMaterialResponseParameterCode.ItemCount, 1 }
                            };
                            SendResponse(operationCode, responseParameters);
                            LogService.InfoFormat("Player: {0}, DrawMaterial, ItemID: {1}, ItemCount{2}", subject.IdentityInformation, x.ItemID, itemCount);
                        });
                        subject.NextDrawMaterialTime = ItemManager.Instance.NextDrawMaterialTime + ItemManager.Instance.NextDrawMaterialTimeSpan;
                        subject.DrawMaterial();
                        return true;
                    }
                    else if(drawTime < subject.NextDrawMaterialTime)
                    {
                        SendError(operationCode, ErrorCode.Fail, "DrawMaterial Fail");
                        LogService.ErrorFormat("Player: {0}, DrawMaterial Fail", subject.IdentityInformation);
                        var remainedTimeSpan = subject.NextDrawMaterialTime - DateTime.Now;
                        subject.User.EventManager.UserInform("失敗", $"抽取素材失敗，距離下一次收取時間，剩餘{remainedTimeSpan.Hours}小時{remainedTimeSpan.Minutes}分{remainedTimeSpan.Seconds}秒。");
                        return false;
                    }
                    else
                    {
                        SendError(operationCode, ErrorCode.Fail, "DrawMaterial Fail");
                        LogService.ErrorFormat("Player: {0}, DrawMaterial Fail", subject.IdentityInformation);
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
        private Item DrawAMaterial()
        {
            Random randomGenerator = new Random(Guid.NewGuid().GetHashCode());
            int resultCaseNumber = randomGenerator.Next(1, 1001);
            IEnumerable<Material> materials = null;
            if (resultCaseNumber <= 800)
            {
                materials = ItemManager.Instance.Items.OfType<Material>().Where(x => x.Level == 1);
                if (randomGenerator.Next(1, 11) <= 7)
                {
                    materials = materials.Where(x => x.GroupType == subject.GroupType);
                }
                else
                {
                    materials = materials.Where(x => x.GroupType != subject.GroupType);
                }
            }
            else if (resultCaseNumber <= 960)
            {
                materials = ItemManager.Instance.Items.OfType<Material>().Where(x => x.Level == 2);
                if (randomGenerator.Next(1, 11) <= 7)
                {
                    materials = materials.Where(x => x.GroupType == subject.GroupType);
                }
                else
                {
                    materials = materials.Where(x => x.GroupType != subject.GroupType);
                }
            }
            else if (resultCaseNumber <= 995)
            {
                materials = ItemManager.Instance.Items.OfType<Material>().Where(x => x.Level == 3);
                if (randomGenerator.Next(1, 11) <= 7)
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
                materials = ItemManager.Instance.Items.OfType<Material>().Where(x => x.Level == 4);
            }
            return materials.ElementAt(randomGenerator.Next(0, materials.Count()));
        }
    }
}
