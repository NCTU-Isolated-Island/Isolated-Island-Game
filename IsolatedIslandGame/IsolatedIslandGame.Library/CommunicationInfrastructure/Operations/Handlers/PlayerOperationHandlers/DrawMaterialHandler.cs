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
                Item randomItem = ItemManager.Instance.Items.ElementAt(randomGenerator.Next(0, ItemManager.Instance.ItemCount));
                int randomNumber = randomGenerator.Next(1, 10);
                subject.Inventory.AddItem(randomItem, randomNumber);
                Dictionary<byte, object> responseParameters = new Dictionary<byte, object>
                {
                    { (byte)DrawMaterialResponseParameterCode.ItemID, randomItem.ItemID },
                    { (byte)DrawMaterialResponseParameterCode.ItemCount, randomNumber }
                };
                SendResponse(operationCode, responseParameters);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
