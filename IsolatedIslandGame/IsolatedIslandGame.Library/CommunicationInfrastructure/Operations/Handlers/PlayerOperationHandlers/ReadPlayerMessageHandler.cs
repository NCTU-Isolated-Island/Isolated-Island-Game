using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.OperationParameters.Player;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.PlayerOperationHandlers
{
    class ReadPlayerMessageHandler : PlayerOperationHandler
    {
        public ReadPlayerMessageHandler(Player subject) : base(subject, 1)
        {
        }

        internal override bool Handle(PlayerOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(operationCode, parameters))
            {
                int playerMessageID = (int)parameters[(byte)ReadPlayerMessageParameterCode.PlayerMessageID];

                if(SystemManager.Instance.OperationInterface.ReadPlayerMessage(subject.PlayerID, playerMessageID))
                {
                    return true;
                }
                else
                {
                    subject.User.EventManager.UserInform("失敗", "標記已讀玩家訊息失敗。");
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
