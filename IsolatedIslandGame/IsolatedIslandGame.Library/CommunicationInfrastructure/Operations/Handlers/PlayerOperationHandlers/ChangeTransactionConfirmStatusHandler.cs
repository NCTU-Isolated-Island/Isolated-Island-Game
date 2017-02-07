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
                string debugMessage;
                ErrorCode errorCode;
                int transactionID = (int)parameters[(byte)ChangeTransactionConfirmStatusParameterCode.TransactionID];
                bool isConfirmed = (bool)parameters[(byte)ChangeTransactionConfirmStatusParameterCode.IsConfirmed];

                if (subject.User.CommunicationInterface.ChangeTransactionConfirmStatus(subject.PlayerID, transactionID, isConfirmed))
                {
                    SendResponse(operationCode, new Dictionary<byte, object>());
                    LogService.InfoFormat($"Player: {subject.IdentityInformation}, ChangeTransactionConfirmStatus, TransactionID: {transactionID}, IsConfirmed: {isConfirmed}");
                    return true;
                }
                else
                {
                    errorCode = ErrorCode.Fail;
                    debugMessage = "ConfirmTransaction Fail";
                    SendError(operationCode, errorCode, debugMessage);
                    LogService.ErrorFormat($"Player: {subject.IdentityInformation}, ChangeTransactionConfirmStatus Fail, TransactionID: {transactionID}, IsConfirmed: {isConfirmed}");
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
