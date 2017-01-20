namespace IsolatedIslandGame.Protocol.Communication.SyncDataParameters.Player
{
    public enum SyncTransactionItemChangeParameterCode : byte
    {
        TransactionID,
        DataChangeType,
        ItemID,
        ItemCount,
        PositionIndex
    }
}
