namespace IsolatedIslandGame.Protocol.Communication.SyncDataParameters.System
{
    public enum SyncVesselChangeParameterCode : byte
    {
        VesselID,
        OwnerPlayerID,
        Name,
        LocationX,
        LocationZ,
        EulerAngleY,
        DataChangeType
    }
}
