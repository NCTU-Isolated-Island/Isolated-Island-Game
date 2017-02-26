using IsolatedIslandGame.Library.Quests;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataResponseParameters.Player;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers.PlayerResponseHandlers.FetchDataResponseHandlers
{
    class FetchAllQuestRecordsResponseHandler : FetchDataResponseHandler<Player, PlayerFetchDataCode>
    {
        public FetchAllQuestRecordsResponseHandler(Player subject) : base(subject)
        {
        }

        public override bool CheckError(Dictionary<byte, object> parameters, ErrorCode returnCode, string debugMessage)
        {
            switch (returnCode)
            {
                case ErrorCode.NoError:
                    {
                        if (parameters.Count != 8)
                        {
                            LogService.ErrorFormat(string.Format("FetchAllQuestRecordsResponse Parameter Error, Parameter Count: {0}", parameters.Count));
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                default:
                    {
                        LogService.ErrorFormat("FetchAllQuestRecordsResponse Error DebugMessage: {0}", debugMessage);
                        return false;
                    }
            }
        }

        public override bool Handle(PlayerFetchDataCode fetchCode, ErrorCode returnCode, string fetchDebugMessage, Dictionary<byte, object> parameters)
        {
            if (base.Handle(fetchCode, returnCode, fetchDebugMessage, parameters))
            {
                try
                {
                    int questRecordID = (int)parameters[(byte)FetchAllQuestRecordsResponseParameterCode.QuestRecordID];
                    QuestType questType = (QuestType)parameters[(byte)FetchAllQuestRecordsResponseParameterCode.QuestType];
                    string questName = (string)parameters[(byte)FetchAllQuestRecordsResponseParameterCode.QuestName];
                    string questDescription = (string)parameters[(byte)FetchAllQuestRecordsResponseParameterCode.QuestDescription];
                    string requirementsDescription = (string)parameters[(byte)FetchAllQuestRecordsResponseParameterCode.RequirementsDescription];
                    string rewardsDescription = (string)parameters[(byte)FetchAllQuestRecordsResponseParameterCode.RewardsDescription];
                    bool hasGottenReward = (bool)parameters[(byte)FetchAllQuestRecordsResponseParameterCode.HasGottenReward];
                    bool isFinished = (bool)parameters[(byte)FetchAllQuestRecordsResponseParameterCode.IsFinished];

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
                    LogService.Error("FetchAllQuestRecordsResponse Parameter Cast Error");
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
