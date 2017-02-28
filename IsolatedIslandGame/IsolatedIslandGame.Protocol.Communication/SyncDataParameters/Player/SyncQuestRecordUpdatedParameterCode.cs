namespace IsolatedIslandGame.Protocol.Communication.SyncDataParameters.Player
{
    public enum SyncQuestRecordUpdatedParameterCode : byte
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
