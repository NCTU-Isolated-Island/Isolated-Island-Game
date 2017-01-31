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
                    if(ItemManager.Instance.FindItem(materialItemID, out item) && item is Material && subject.Inventory.RemoveItemCheck(materialItemID, 1))
                    {
                        subject.Inventory.RemoveItem(materialItemID, 1);
                        return Island.Instance.SendMaterial(subject, item as Material);
                    }
                    else
                    {
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
