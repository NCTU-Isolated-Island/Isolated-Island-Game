using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataResponseParameters.Player;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.PlayerOperationHandlers.FetchDataHandlers
{
    class FetchAllQuestRecordsHandler : PlayerFetchDataHandler
    {
        public FetchAllQuestRecordsHandler(Player subject) : base(subject, 0)
        {
        }
        public override bool Handle(PlayerFetchDataCode fetchCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(fetchCode, parameters))
            {
                try
                {
                    foreach (var questRecord in subject.QuestRecords)
                    {
                        if(questRecord.IsFinished && questRecord.HasGottenReward)
                        {
                            continue;
                        }
                        if (questRecord.IsFinished || !questRecord.Quest.IsHidden)
                        {
                            var information = questRecord.QuestRecordInformation;
                            var result = new Dictionary<byte, object>
                            {
                                { (byte)FetchAllQuestRecordsResponseParameterCode.QuestRecordID, information.questRecordID },
                                { (byte)FetchAllQuestRecordsResponseParameterCode.QuestType, (byte)information.questType },
                                { (byte)FetchAllQuestRecordsResponseParameterCode.QuestName, information.questName },
                                { (byte)FetchAllQuestRecordsResponseParameterCode.QuestDescription, information.questDescription },
                                { (byte)FetchAllQuestRecordsResponseParameterCode.IsHidden, information.isHidden },
                                { (byte)FetchAllQuestRecordsResponseParameterCode.RequirementsDescription, information.requirementsDescription },
                                { (byte)FetchAllQuestRecordsResponseParameterCode.RewardsDescription, information.rewardsDescription },
                                { (byte)FetchAllQuestRecordsResponseParameterCode.HasGottenReward, information.hasGottenReward },
                                { (byte)FetchAllQuestRecordsResponseParameterCode.IsFinished, information.isFinished }
                            };
                            SendResponse(fetchCode, result);
                        }
                    }
                    return true;
                }
                catch (InvalidCastException ex)
                {
                    LogService.Error("FetchAllQuestRecords Invalid Cast!");
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
