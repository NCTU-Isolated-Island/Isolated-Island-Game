using IsolatedIslandGame.Library.Quests;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.OperationParameters.Player;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.PlayerOperationHandlers
{
    class TakeQuestRewardHandler : PlayerOperationHandler
    {
        public TakeQuestRewardHandler(Player subject) : base(subject, 1)
        {
        }

        internal override bool Handle(PlayerOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(operationCode, parameters))
            {
                int questRecorndID = (int)parameters[(byte)TakeQuestRewardParameterCode.QuestRecorndID];
                QuestRecord record;
                if (subject.FindQuestRecord(questRecorndID, out record))
                {
                    if(record.IsFinished)
                    {
                        if(record.HasGottenReward)
                        {
                            LogService.ErrorFormat($"Player: {subject.IdentityInformation}, TakeQuestReward Fail, has gotten reward, QuestRecorndID: {questRecorndID}");
                            subject.User.EventManager.UserInform("失敗", "已領取過獎勵。");
                            return false;
                        }
                        else
                        {
                            record.GiveReward();
                            return true;
                        }
                    }
                    else
                    {
                        LogService.ErrorFormat($"Player: {subject.IdentityInformation}, TakeQuestReward Fail, still not finished, QuestRecorndID: {questRecorndID}");
                        subject.User.EventManager.UserInform("失敗", "任務尚未完成。");
                        return false;
                    }
                }
                else
                {
                    LogService.ErrorFormat($"Player: {subject.IdentityInformation}, TakeQuestReward Fail, QuestRecorndID: {questRecorndID}");
                    subject.User.EventManager.UserInform("失敗", "該任務不存在。");
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
