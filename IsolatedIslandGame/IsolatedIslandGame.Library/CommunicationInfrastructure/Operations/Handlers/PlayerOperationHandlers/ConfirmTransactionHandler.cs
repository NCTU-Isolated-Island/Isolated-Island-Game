using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.OperationParameters.Player;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.PlayerOperationHandlers
{
    class ConfirmTransactionHandler : PlayerOperationHandler
    {
        public ConfirmTransactionHandler(Player subject) : base(subject, 1)
        {
        }

        internal override bool Handle(PlayerOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(operationCode, parameters))
            {
                string debugMessage;
                ErrorCode errorCode;
                int transactionID = (int)parameters[(byte)ConfirmTransactionParameterCode.TransactionID];
                if (subject.User.CommunicationInterface.ConfirmTransaction(subject.PlayerID, transactionID))
                {
                    SendResponse(operationCode, new Dictionary<byte, object>());
                    LogService.InfoFormat($"Player: {subject.IdentityInformation}, ConfirmTransaction, TransactionID: {transactionID}");
                    return true;
                }
                else
                {
                    errorCode = ErrorCode.Fail;
                    debugMessage = "ConfirmTransaction Fail";
                    SendError(operationCode, errorCode, debugMessage);
                    LogService.ErrorFormat($"Player: {subject.IdentityInformation}, ConfirmTransaction Fail, TransactionID: {transactionID}");
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
