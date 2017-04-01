using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.OperationParameters.Player;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.PlayerOperationHandlers
{
    class SendWorldChannelMessageHandler : PlayerOperationHandler
    {
        public SendWorldChannelMessageHandler(Player subject) : base(subject, 1)
        {
        }

        internal override bool Handle(PlayerOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(operationCode, parameters))
            {
                string content = (string)parameters[(byte)SendWorldChannelMessageParameterCode.Content];

                if (SystemManager.Instance.OperationInterface.SendWorldChannelMessage(subject.PlayerID, content))
                {
                    return true;
                }
                else
                {
                    subject.User.EventManager.UserInform("失敗", "不知道為什麼就是發不出去!");
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
