using IsolatedIslandGame.Library.Quests;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.SyncDataCodes;
using IsolatedIslandGame.Protocol.Communication.SyncDataParameters.Player;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.PlayerEventHandlers.SyncDataHandlers
{
    class SyncQuestRecordUpdatedHandler : SyncDataHandler<Player, PlayerSyncDataCode>
    {
        public SyncQuestRecordUpdatedHandler(Player subject) : base(subject, 8)
        {
        }
        internal override bool Handle(PlayerSyncDataCode syncCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(syncCode, parameters))
            {
                try
                {
                    int questRecordID = (int)parameters[(byte)SyncQuestRecordUpdatedParameterCode.QuestRecordID];
                    QuestType questType = (QuestType)parameters[(byte)SyncQuestRecordUpdatedParameterCode.QuestType];
                    string questName = (string)parameters[(byte)SyncQuestRecordUpdatedParameterCode.QuestName];
                    string questDescription = (string)parameters[(byte)SyncQuestRecordUpdatedParameterCode.QuestDescription];
                    string requirementsDescription = (string)parameters[(byte)SyncQuestRecordUpdatedParameterCode.RequirementsDescription];
                    string rewardsDescription = (string)parameters[(byte)SyncQuestRecordUpdatedParameterCode.RewardsDescription];
                    bool hasGottenReward = (bool)parameters[(byte)SyncQuestRecordUpdatedParameterCode.HasGottenReward];
                    bool isFinished = (bool)parameters[(byte)SyncQuestRecordUpdatedParameterCode.IsFinished];

                    QuestRecordInformation information = new QuestRecordInformation
                    {
                        questRecordID = questRecordID,
                        questType = questType,
                        questName = questName,
                        questDescription = questDescription,
                        requirementsDescription = requirementsDescription,
                        rewardsDescription = rewardsDescription,
                        hasGottenReward = hasGottenReward,
                        isFinished = isFinished
                    };

                    subject.LoadQuestRecordInformation(information);
                    return true;
                }
                catch (InvalidCastException ex)
                {
                    LogService.Error("SyncQuestRecordUpdated Parameter Cast Error");
                    LogService.Error(ex.Message);
                    LogService.Error(ex.StackTrace);
                    return false;
                }
                catch (Exception ex)
                {
                    LogService.Error(ex.Message);
                    LogService.Error(ex.StackTrace);
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
