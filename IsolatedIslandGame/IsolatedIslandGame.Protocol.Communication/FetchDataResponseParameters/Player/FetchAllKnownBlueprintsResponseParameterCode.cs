namespace IsolatedIslandGame.Protocol.Communication.FetchDataResponseParameters.Player
{
    public enum FetchAllKnownBlueprintsResponseParameterCode : byte
    {
        BlueprintID,
        IsOrderless,
        IsBlueprintRequired,
        RequirementsItemID_Array,
        RequirementsItemCountArray,
        RequirementsPositionIndexArray,
        ProductsItemID_Array,
        ProductsItemCountArray,
        ProductsPositionIndexArray
    }
}
