namespace IsolatedIslandGame.Protocol.Communication.SyncDataParameters.Player
{
    public enum SyncTransactionItemChangeParameterCode : byte
    {
        TransactionID,
        PlayerID,
        DataChangeType,
        ItemID,
        ItemCount,
        PositionIndex
    }
}
