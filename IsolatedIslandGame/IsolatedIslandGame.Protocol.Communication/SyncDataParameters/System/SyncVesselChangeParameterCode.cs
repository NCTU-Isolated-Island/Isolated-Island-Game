namespace IsolatedIslandGame.Protocol.Communication.SyncDataParameters.System
{
    public enum SyncVesselChangeParameterCode : byte
    {
        DataChangeType,
        VesselID,
        OwnerPlayerID,
        LocationX,
        LocationZ,
        EulerAngleY
    }
}
