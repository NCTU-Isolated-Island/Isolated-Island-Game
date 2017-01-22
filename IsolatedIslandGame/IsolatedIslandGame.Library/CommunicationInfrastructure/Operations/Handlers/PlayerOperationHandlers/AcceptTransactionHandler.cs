using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.OperationParameters.Player;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.PlayerOperationHandlers
{
    class AcceptTransactionHandler : PlayerOperationHandler
    {
        public AcceptTransactionHandler(Player subject) : base(subject, 1)
        {
        }

        internal override bool Handle(PlayerOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(operationCode, parameters))
            {
                string debugMessage;
                ErrorCode errorCode;
                int requesterPlayerID = (int)parameters[(byte)AcceptTransactionParameterCode.RequesterPlayerID];
                if (subject.User.CommunicationInterface.AcceptTransaction(requesterPlayerID, subject.PlayerID))
                {
                    SendResponse(operationCode, new Dictionary<byte, object>());
                    LogService.InfoFormat($"Player: {subject.IdentityInformation}, AcceptTransaction, RequesterPlayerID: {requesterPlayerID}");
                    return true;
                }
                else
                {
                    errorCode = ErrorCode.Fail;
                    debugMessage = "AcceptTransaction Fail";
                    SendError(operationCode, errorCode, debugMessage);
                    LogService.ErrorFormat($"Player: {subject.IdentityInformation}, AcceptTransaction Fail, RequesterPlayerID: {requesterPlayerID}");
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
