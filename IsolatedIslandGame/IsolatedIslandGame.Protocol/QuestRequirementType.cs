namespace IsolatedIslandGame.Protocol
{
    public enum QuestRequirementType : byte
    {
        SendMessageToDifferentOnlineFriendInTheSameOcean,
        CloseDealWithDifferentFriendInTheSameOcean,
        ScanQR_Code,
        TimeLimit,
    }
}
