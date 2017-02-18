namespace IsolatedIslandGame.Protocol.Communication.FetchDataCodes
{
    public enum SystemFetchDataCode : byte
    {
        Item,
        AllItems,
        AllVessels,
        Vessel,
        VesselWithOwnerPlayerID,
        VesselDecorations,
        IslandTotalScore,
        IslandTodayMaterialRanking,
        IslandPlayerScoreRanking,
        AllLandmarks
    }
}
