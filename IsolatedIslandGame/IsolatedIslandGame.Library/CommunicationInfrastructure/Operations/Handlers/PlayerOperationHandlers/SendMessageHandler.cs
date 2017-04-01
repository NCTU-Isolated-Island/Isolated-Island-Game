using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.OperationParameters.Player;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.PlayerOperationHandlers
{
    class SendMessageHandler : PlayerOperationHandler
    {
        public SendMessageHandler(Player subject) : base(subject, 2)
        {
        }

        internal override bool Handle(PlayerOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(operationCode, parameters))
            {
                int receiverPlayerID = (int)parameters[(byte)SendMessageParameterCode.ReceiverPlayerID];
                string content = (string)parameters[(byte)SendMessageParameterCode.Content];

                if (SystemManager.Instance.OperationInterface.SendMessage(subject.PlayerID, receiverPlayerID, content))
                {
                    return true;
                }
                else
                {
                    subject.User.EventManager.UserInform("失敗", "發送訊息的對象並不存在。");
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
