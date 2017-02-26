namespace IsolatedIslandGame.Protocol.Communication.EventParameters.Player
{
    public enum GetBlueprintParameterCode : byte
    {
        BlueprintID,
        IsOrderless,
        IsBlueprintRequired,
        RequirementsItemID_Array,
        RequirementsItemCountArray,
        RequirementsPositionIndexArray,
        ProductsItemID_Array,
        ProductsItemCountArray,
        ProductsPositionIndexArray,
    }
}
