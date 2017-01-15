using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.OperationParameters.Player;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.PlayerOperationHandlers
{
    class AcceptFriendHandler : PlayerOperationHandler
    {
        public AcceptFriendHandler(Player subject) : base(subject, 1)
        {
        }

        internal override bool Handle(PlayerOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(operationCode, parameters))
            {
                string debugMessage;
                ErrorCode errorCode;
                int inviterPlayerID = (int)parameters[(byte)AcceptFriendParameterCode.InviterPlayerID];
                if (subject.User.CommunicationInterface.AcceptFriend(inviterPlayerID, subject.PlayerID))
                {
                    SendResponse(operationCode, new Dictionary<byte, object>());
                    LogService.InfoFormat($"Player: {subject.IdentityInformation}, AcceptFriend, InviterPlayerID: {inviterPlayerID}");
                    return true;
                }
                else
                {
                    errorCode = ErrorCode.Fail;
                    debugMessage = "accept friend fail";
                    SendError(operationCode, errorCode, debugMessage);
                    LogService.ErrorFormat($"Player: {subject.IdentityInformation}, AcceptFriend Fail, InviterPlayerID: {inviterPlayerID}");
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
