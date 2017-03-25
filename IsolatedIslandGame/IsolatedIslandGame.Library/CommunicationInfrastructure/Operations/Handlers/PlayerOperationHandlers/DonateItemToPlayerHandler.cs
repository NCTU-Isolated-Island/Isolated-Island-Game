using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.OperationParameters.Player;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.PlayerOperationHandlers
{
    class DonateItemToPlayerHandler : PlayerOperationHandler
    {
        public DonateItemToPlayerHandler(Player subject) : base(subject, 3)
        {
        }

        internal override bool Handle(PlayerOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(operationCode, parameters))
            {
                int playerID = (int)parameters[(byte)DonateItemToPlayerParameterCode.PlayerID];
                int itemID = (int)parameters[(byte)DonateItemToPlayerParameterCode.ItemID];
                int itemCount = (int)parameters[(byte)DonateItemToPlayerParameterCode.ItemCount];

                Item item;
                if (ItemManager.Instance.FindItem(itemID, out item))
                {
                    if(subject.User.CommunicationInterface.DonateItemToPlayer(subject.PlayerID, playerID, item, itemCount))
                    {
                        subject.DonateItem();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
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
