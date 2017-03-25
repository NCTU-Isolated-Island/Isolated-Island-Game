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
        SynthesizeSpecificBlueprint,
        ScanSpecificQR_Code,
        HaveSpecificNumberFriend,
        SynthesizeSuccessfulSpecificNumberOfTime,
        HaveSpecificNumberKindMaterial,
        AddSpecificNumberDecorationToVessel,
        HaveSpecificNumberDecorationOnVessel,
        FinishedBeforeSpecificTime,
        FinishedInSpecificTimeSpan,
        DrawMaterial,

        SendMessageToSpecificNumberDifferentPlayer,
        DonateItemSpecificNumberOfTime,
        RemoveSpecificNumberDecorationFromVessel,
        HaveSpecificNumberSpecificItem,
        MakeFriendSuccessfulWithSpecificGroupPlayerSpecificNumberOfTime,
        MakeFriendSuccessfulWithOtherGroupPlayerSpecificNumberOfTime,
        CloseDealWithSpecificNumberDifferentOtherGroupPlayer,
        SendMessageToSpecificNumberSpecificGroupDifferentPlayer,
        StillNotOpened
    }
}
