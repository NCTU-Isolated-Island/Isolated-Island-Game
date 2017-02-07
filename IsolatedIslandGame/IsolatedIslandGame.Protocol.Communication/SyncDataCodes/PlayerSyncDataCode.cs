namespace IsolatedIslandGame.Protocol.Communication.SyncDataCodes
{
    public enum PlayerSyncDataCode : byte
    {
        InventoryItemInfoChange,
        FriendInformationChange,
        PlayerInformation,
        TransactionItemChange,
        TransactionConfirmStatusChange,
        QuestRecordUpdated
    }
}
