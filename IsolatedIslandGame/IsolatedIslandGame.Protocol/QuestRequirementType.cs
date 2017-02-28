namespace IsolatedIslandGame.Protocol
{
    public enum QuestRequirementType : byte
    {
        CumulativeLoginSpecificDay,
        StayInSpecificOcean,
        MakeFriendSuccessfulSpecificNumberOfTime,
        CloseDealSpecificNumberOfTime,
        SendMaterialToIslandSpecificNumberOfTime,
        ExistedInSpecificNumberOcean,
        SendMessageToSpecificNumberDifferentOnlineFriendInTheSameSpecificOcean,
        CloseDealWithSpecificNumberDifferentFriendInTheSameSpecificOcean,
        GetSpecificItem,
        CloseDealWithOutlander,
        CollectSpecificNumberBelongingGroupMaterial,
        SynthesizeSpecificScoreMaterial,
        ScanSpecificQR_Code,
        HaveSpecificNumberFriend,
        SynthesizeSuccessfulSpecificNumberOfTime,
        HaveSpecificNumberKindMaterial,
        AddSpecificNumberDecorationToVessel,
        HaveSpecificNumberDecorationOnVessel,
    }
}
