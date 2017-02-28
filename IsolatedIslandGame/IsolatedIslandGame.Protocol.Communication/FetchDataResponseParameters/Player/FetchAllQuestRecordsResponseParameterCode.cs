namespace IsolatedIslandGame.Protocol.Communication.FetchDataResponseParameters.Player
{
    public enum FetchAllQuestRecordsResponseParameterCode : byte
    {
        QuestRecordID,
        QuestType,
        QuestName,
        QuestDescription,
        IsHidden,
        RequirementsDescription,
        RewardsDescription,
        HasGottenReward,
        IsFinished
    }
}
