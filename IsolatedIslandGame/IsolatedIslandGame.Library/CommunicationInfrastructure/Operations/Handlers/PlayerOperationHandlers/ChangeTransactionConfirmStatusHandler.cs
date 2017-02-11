using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.OperationParameters.Player;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.PlayerOperationHandlers
{
    class ChangeTransactionConfirmStatusHandler : PlayerOperationHandler
    {
        public ChangeTransactionConfirmStatusHandler(Player subject) : base(subject, 2)
        {
        }

        internal override bool Handle(PlayerOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(operationCode, parameters))
            {
                int transactionID = (int)parameters[(byte)ChangeTransactionConfirmStatusParameterCode.TransactionID];
                bool isConfirmed = (bool)parameters[(byte)ChangeTransactionConfirmStatusParameterCode.IsConfirmed];

                if (subject.User.CommunicationInterface.ChangeTransactionConfirmStatus(subject.PlayerID, transactionID, isConfirmed))
                {
                    LogService.InfoFormat($"Player: {subject.IdentityInformation}, ChangeTransactionConfirmStatus, TransactionID: {transactionID}, IsConfirmed: {isConfirmed}");
                    return true;
                }
                else
                {
                    LogService.ErrorFormat($"Player: {subject.IdentityInformation}, ChangeTransactionConfirmStatus Fail, TransactionID: {transactionID}, IsConfirmed: {isConfirmed}");
                    subject.User.EventManager.UserInform("失敗", "確認交易失敗。");
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
