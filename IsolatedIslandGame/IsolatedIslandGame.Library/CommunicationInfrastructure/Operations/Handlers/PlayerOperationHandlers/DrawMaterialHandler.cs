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
                IEnumerable<Item> basicMaterials = ItemManager.Instance.Items.Where(x => x.ItemID < 2000);
                Item randomItem = basicMaterials.ElementAt(randomGenerator.Next(0, basicMaterials.Count()));
                int randomNumber = randomGenerator.Next(1, 10);

                lock(subject.Inventory)
                {
                    if(DateTime.Now > subject.NextDrawMaterialTime && subject.Inventory.AddItemCheck(randomItem.ItemID, randomNumber))
                    {
                        subject.Inventory.AddItem(randomItem, randomNumber);
                        Dictionary<byte, object> responseParameters = new Dictionary<byte, object>
                        {
                            { (byte)DrawMaterialResponseParameterCode.ItemID, randomItem.ItemID },
                            { (byte)DrawMaterialResponseParameterCode.ItemCount, randomNumber }
                        };
                        SendResponse(operationCode, responseParameters);
                        LogService.InfoFormat("Player: {0}, DrawMaterial, ItemID: {1}, ItemCount{2}", subject.IdentityInformation, randomItem.ItemID, randomNumber);
                        DateTime nextDrawMaterialTime = DateTime.Today;
                        while(nextDrawMaterialTime < DateTime.Now)
                        {
                            nextDrawMaterialTime += TimeSpan.FromHours(6);
                        }
                        subject.NextDrawMaterialTime = nextDrawMaterialTime;
                        return true;
                    }
                    else
                    {
                        SendError(operationCode, ErrorCode.Fail, "DrawMaterial Fail");
                        LogService.ErrorFormat("Player: {0}, DrawMaterial Fail, ItemID: {1}, ItemCount{2}", subject.IdentityInformation, randomItem.ItemID, randomNumber);
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
