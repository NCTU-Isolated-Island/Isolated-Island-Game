namespace IsolatedIslandGame.Protocol.Communication.SyncDataParameters.Player
{
    public enum SyncFriendInformationChangeParameterCode : byte
    {
        DataChangeType,
        PlayerID,
        Nickname,
        Signature,
        GroupType,
        VesselID,
        IsInviter,
        IsConfirmed
    }
}
