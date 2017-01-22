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
                string debugMessage;
                ErrorCode errorCode;
                int accepterPlayerID = (int)parameters[(byte)TransactionRequestParameterCode.AccepterPlayerID];
                if (subject.User.CommunicationInterface.TransactionRequest(subject.PlayerID, accepterPlayerID))
                {
                    SendResponse(operationCode, new Dictionary<byte, object>());
                    LogService.InfoFormat($"Player: {subject.IdentityInformation}, TransactionRequest, AccepterPlayerID: {accepterPlayerID}");
                    return true;
                }
                else
                {
                    errorCode = ErrorCode.Fail;
                    debugMessage = "transaction request fail";
                    SendError(operationCode, errorCode, debugMessage);
                    LogService.ErrorFormat($"Player: {subject.IdentityInformation}, TransactionRequest Fail, AccepterPlayerID: {accepterPlayerID}");
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
