namespace IsolatedIslandGame.Protocol.Communication.SyncDataCodes
{
    public enum SystemSyncDataCode : byte
    {
        VesselChange,
        VesselTransform,
        VesselDecorationChange,
        PlayerInformation,
        IslandTotalScoreUpdated,
        IslandTodayMaterialRankingUpdated,
        IslandPlayerScoreRankingUpdated,
        LandmarkUpdated,
    }
}
