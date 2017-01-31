namespace IsolatedIslandGame.Protocol.Communication.FetchDataCodes
{
    public enum SystemFetchDataCode : byte
    {
        Item,
        AllVessels,
        Vessel,
        VesselWithOwnerPlayerID,
        VesselDecorations,
        IslandTotalScore,
        IslandTodayMaterialRanking,
        IslandPlayerScoreRanking,
    }
}
