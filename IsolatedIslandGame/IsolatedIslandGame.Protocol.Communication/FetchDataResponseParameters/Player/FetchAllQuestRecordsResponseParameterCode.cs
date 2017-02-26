namespace IsolatedIslandGame.Protocol.Communication.FetchDataResponseParameters.Player
{
    public enum FetchAllQuestRecordsResponseParameterCode : byte
    {
        QuestRecordID,
        QuestType,
        QuestName,
        QuestDescription,
        RequirementsDescription,
        RewardsDescription,
        HasGottenReward,
        IsFinished
    }
}
