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
                int requesterPlayerID = (int)parameters[(byte)AcceptTransactionParameterCode.RequesterPlayerID];
                if (subject.User.CommunicationInterface.AcceptTransaction(requesterPlayerID, subject.PlayerID))
                {
                    LogService.InfoFormat($"Player: {subject.IdentityInformation}, AcceptTransaction, RequesterPlayerID: {requesterPlayerID}");
                    return true;
                }
                else
                {
                    LogService.ErrorFormat($"Player: {subject.IdentityInformation}, AcceptTransaction Fail, RequesterPlayerID: {requesterPlayerID}");
                    subject.User.EventManager.UserInform("失敗", "接受交易失敗。");
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
