namespace IsolatedIslandGame.Protocol.Communication.SyncDataParameters.Player
{
    public enum SyncInventoryItemInfoChangeParameterCode : byte
    {
        InventoryID,
        InventoryItemInfoID,
        ItemID,
        ItemCount,
        PositionIndex,
        DataChangeType
    }
}
