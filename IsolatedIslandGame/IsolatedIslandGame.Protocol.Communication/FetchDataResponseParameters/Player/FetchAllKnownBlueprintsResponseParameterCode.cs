namespace IsolatedIslandGame.Protocol.Communication.FetchDataResponseParameters.Player
{
    public enum FetchAllKnownBlueprintsResponseParameterCode : byte
    {
        BlueprintID,
        IsOrderless,
        IsBlueprintRequired,
        Requirements,
        Products
    }
}
