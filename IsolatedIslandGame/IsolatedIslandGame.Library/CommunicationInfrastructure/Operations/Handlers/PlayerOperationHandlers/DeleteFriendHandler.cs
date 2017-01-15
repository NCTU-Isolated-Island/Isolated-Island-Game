using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.OperationParameters.Player;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.PlayerOperationHandlers
{
    class DeleteFriendHandler : PlayerOperationHandler
    {
        public DeleteFriendHandler(Player subject) : base(subject, 1)
        {
        }

        internal override bool Handle(PlayerOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(operationCode, parameters))
            {
                string debugMessage;
                ErrorCode errorCode;
                int targetPlayerID = (int)parameters[(byte)DeleteFriendParameterCode.TargetPlayerID];
                if (subject.User.CommunicationInterface.DeleteFriend(subject.PlayerID, targetPlayerID))
                {
                    SendResponse(operationCode, new Dictionary<byte, object>());
                    LogService.InfoFormat($"Player: {subject.IdentityInformation}, DeleteFriend, TargetPlayerID: {targetPlayerID}");
                    return true;
                }
                else
                {
                    errorCode = ErrorCode.Fail;
                    debugMessage = "delete friend fail";
                    SendError(operationCode, errorCode, debugMessage);
                    LogService.ErrorFormat($"Player: {subject.IdentityInformation}, DeleteFriend Fail, TargetPlayerID: {targetPlayerID}");
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
