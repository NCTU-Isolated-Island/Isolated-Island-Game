namespace IsolatedIslandGame.Protocol
{
    public enum QuestRequirementType : byte
    {
        SendMessageToDifferentOnlineFriendInTheSameSpecificOcean,
        CloseDealWithDifferentFriendInTheSameSpecificOcean,
        ScanQR_Code,
        TimeLimit,
        CumulativeLogin,
    }
}
