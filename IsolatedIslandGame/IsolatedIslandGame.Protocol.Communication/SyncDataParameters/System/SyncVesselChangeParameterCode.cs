namespace IsolatedIslandGame.Protocol.Communication.SyncDataParameters.System
{
    public enum SyncVesselChangeParameterCode : byte
    {
        DataChangeType,
        VesselID,
        PlayerID,
        Nickname,
        Signature,
        GroupType,
        LocationX,
        LocationZ,
        EulerAngleY
    }
}
