using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.OperationParameters.Player;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.PlayerOperationHandlers
{
    class InviteFriendHandler : PlayerOperationHandler
    {
        public InviteFriendHandler(Player subject) : base(subject, 1)
        {
        }

        internal override bool Handle(PlayerOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(operationCode, parameters))
            {
                string debugMessage;
                ErrorCode errorCode;
                int accepterPlayerID = (int)parameters[(byte)InviteFriendParameterCode.AccepterPlayerID];
                if (subject.User.CommunicationInterface.InviteFriend(subject.PlayerID, accepterPlayerID))
                {
                    SendResponse(operationCode, new Dictionary<byte, object>());
                    LogService.InfoFormat($"Player: {subject.IdentityInformation}, InviteFriend, AccepterPlayerID: {accepterPlayerID}");
                    return true;
                }
                else
                {
                    errorCode = ErrorCode.Fail;
                    debugMessage = "invite friend fail";
                    SendError(operationCode, errorCode, debugMessage);
                    LogService.ErrorFormat($"Player: {subject.IdentityInformation}, InviteFriend Fail, AccepterPlayerID: {accepterPlayerID}");
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
