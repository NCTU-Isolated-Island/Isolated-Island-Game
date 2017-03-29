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
                int targetPlayerID = (int)parameters[(byte)DeleteFriendParameterCode.TargetPlayerID];
                if (SystemManager.Instance.OperationInterface.DeleteFriend(subject.PlayerID, targetPlayerID))
                {
                    LogService.InfoFormat($"Player: {subject.IdentityInformation}, DeleteFriend, TargetPlayerID: {targetPlayerID}");
                    return true;
                }
                else
                {
                    LogService.ErrorFormat($"Player: {subject.IdentityInformation}, DeleteFriend Fail, TargetPlayerID: {targetPlayerID}");
                    subject.User.EventManager.UserInform("失敗", "刪除好友失敗。");
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
