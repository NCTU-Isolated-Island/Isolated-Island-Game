using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.OperationParameters.Player;
using System.Collections.Generic;
using IsolatedIslandGame.Library.Items;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.PlayerOperationHandlers
{
    class DiscardItemHandler : PlayerOperationHandler
    {
        public DiscardItemHandler(Player subject) : base(subject, 2)
        {
        }

        internal override bool Handle(PlayerOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(operationCode, parameters))
            {
                int itemID = (int)parameters[(byte)DiscardItemParameterCode.ItemID];
                int itemCount = (int)parameters[(byte)DiscardItemParameterCode.ItemCount];

                lock (subject.Inventory)
                {
                    if (subject.Inventory.RemoveItemCheck(itemID, itemCount) && SystemManager.Instance.OperationInterface.DiscardItem(itemID, itemCount, subject.Vessel.LocationX, subject.Vessel.LocationZ))
                    {
                        subject.Inventory.RemoveItem(itemID, itemCount);
                        return true;
                    }
                    else
                    {
                        LogService.Error($"DiscardItem error Player: {subject.IdentityInformation}, Item Not Enough ItemID: {itemID}, ItemCount: {itemCount}");
                        subject.User.EventManager.UserInform("錯誤", "沒有足夠的物品可以丟棄。");
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
