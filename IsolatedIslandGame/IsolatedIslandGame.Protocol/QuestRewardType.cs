namespace IsolatedIslandGame.Protocol
{
    public enum QuestRewardType : byte
    {
        GiveSpecificNumberSpecificItem,
        UnlockSpecificBlueprint,
        AcceptSpecificQuest,
        GiveSpecificNumberSpecificScoreRandomMaterial,
        GiveSpecificNumberSpecificScoreSpecificGroupRandomMaterial,
        GiveSpecificNumberSpecificScoreBelongingGroupRandomMaterial,
        GiveSpecificNumberSpecificLevelRandomMaterial,
        GiveSpecificNumberSpecificLevelSpecificGroupRandomMaterial,
        GiveSpecificNumberSpecificLevelBelongingGroupRandomMaterial,
    }
}
