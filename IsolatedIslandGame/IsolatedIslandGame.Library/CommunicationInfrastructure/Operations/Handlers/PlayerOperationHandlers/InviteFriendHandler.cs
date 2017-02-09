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
                int accepterPlayerID = (int)parameters[(byte)InviteFriendParameterCode.AccepterPlayerID];
                if (subject.User.CommunicationInterface.InviteFriend(subject.PlayerID, accepterPlayerID))
                {
                    LogService.InfoFormat($"Player: {subject.IdentityInformation}, InviteFriend, AccepterPlayerID: {accepterPlayerID}");
                    return true;
                }
                else
                {
                    LogService.ErrorFormat($"Player: {subject.IdentityInformation}, InviteFriend Fail, AccepterPlayerID: {accepterPlayerID}");
                    subject.User.EventManager.UserInform("失敗", "邀請好友失敗。");
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
