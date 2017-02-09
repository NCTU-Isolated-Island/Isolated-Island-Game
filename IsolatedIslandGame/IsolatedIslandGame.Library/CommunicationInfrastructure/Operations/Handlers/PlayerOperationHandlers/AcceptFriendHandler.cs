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
                int inviterPlayerID = (int)parameters[(byte)AcceptFriendParameterCode.InviterPlayerID];
                if (subject.User.CommunicationInterface.AcceptFriend(inviterPlayerID, subject.PlayerID))
                {
                    LogService.InfoFormat($"Player: {subject.IdentityInformation}, AcceptFriend, InviterPlayerID: {inviterPlayerID}");
                    return true;
                }
                else
                {
                    LogService.ErrorFormat($"Player: {subject.IdentityInformation}, AcceptFriend Fail, InviterPlayerID: {inviterPlayerID}");
                    subject.User.EventManager.UserInform("失敗", "接受好友失敗。");
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
