using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.OperationParameters.Player;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.PlayerOperationHandlers
{
    class TransactionRequestHandler : PlayerOperationHandler
    {
        public TransactionRequestHandler(Player subject) : base(subject, 1)
        {
        }

        internal override bool Handle(PlayerOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(operationCode, parameters))
            {
                int accepterPlayerID = (int)parameters[(byte)TransactionRequestParameterCode.AccepterPlayerID];
                if (subject.User.CommunicationInterface.TransactionRequest(subject.PlayerID, accepterPlayerID))
                {
                    LogService.InfoFormat($"Player: {subject.IdentityInformation}, TransactionRequest, AccepterPlayerID: {accepterPlayerID}");
                    return true;
                }
                else
                {
                    LogService.ErrorFormat($"Player: {subject.IdentityInformation}, TransactionRequest Fail, AccepterPlayerID: {accepterPlayerID}");
                    subject.User.EventManager.UserInform("失敗", "發送交易邀請失敗，請確認該玩家是否在線。");
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
