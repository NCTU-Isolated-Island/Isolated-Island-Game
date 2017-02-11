using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.OperationParameters.Player;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.PlayerOperationHandlers
{
    class CancelTransactionHandler : PlayerOperationHandler
    {
        public CancelTransactionHandler(Player subject) : base(subject, 1)
        {
        }

        internal override bool Handle(PlayerOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(operationCode, parameters))
            {
                int transactionID = (int)parameters[(byte)CancelTransactionParameterCode.TransactionID];

                if (subject.User.CommunicationInterface.CancelTransaction(subject.PlayerID, transactionID))
                {
                    LogService.InfoFormat($"Player: {subject.IdentityInformation}, CancelTransaction, TransactionID: {transactionID}");
                    return true;
                }
                else
                {
                    LogService.ErrorFormat($"Player: {subject.IdentityInformation}, CancelTransaction Fail, TransactionID: {transactionID}");
                    subject.User.EventManager.UserInform("失敗", "取消交易失敗。");
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
