using IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.PlayerEventHandlers.SyncDataHandlers;
using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Library.Quests;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.EventCodes;
using IsolatedIslandGame.Protocol.Communication.SyncDataCodes;
using IsolatedIslandGame.Protocol.Communication.SyncDataParameters.Player;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.PlayerEventHandlers
{
    public class PlayerSyncDataResolver : SyncDataResolver<Player, PlayerEventCode, PlayerSyncDataCode>
    {
        internal PlayerSyncDataResolver(Player player) : base(player)
        {
            syncTable.Add(PlayerSyncDataCode.InventoryItemInfoChange, new SyncInventoryItemInfoChangeHandler(subject));
            syncTable.Add(PlayerSyncDataCode.FriendInformationChange, new SyncFriendInformationChangeHandler(subject));
            syncTable.Add(PlayerSyncDataCode.PlayerInformation, new SyncPlayerInformationHandler(subject));
            syncTable.Add(PlayerSyncDataCode.TransactionItemChange, new SyncTransactionItemChangeHandler(subject));
            syncTable.Add(PlayerSyncDataCode.TransactionConfirmStatusChange, new SyncTransactionConfirmStatusChangeHandler(subject));
            syncTable.Add(PlayerSyncDataCode.QuestRecordUpdated, new SyncQuestRecordUpdatedHandler(subject));
            syncTable.Add(PlayerSyncDataCode.NextDrawMaterialTimeUpdated, new SyncNextDrawMaterialTimeUpdatedHandler(subject));
        }

        internal override void SendSyncData(PlayerSyncDataCode syncCode, Dictionary<byte, object> parameters)
        {
            subject.EventManager.SendSyncDataEvent(syncCode, parameters);
        }

        public void SyncInventoryItemInfoChange(DataChangeType changeType, InventoryItemInfo info)
        {
            var parameters = new Dictionary<byte, object>
            {
                { (byte)SyncInventoryItemInfoChangeParameterCode.DataChangeType, (byte)changeType },
                { (byte)SyncInventoryItemInfoChangeParameterCode.InventoryID, subject.Inventory.InventoryID },
                { (byte)SyncInventoryItemInfoChangeParameterCode.InventoryItemInfoID, info.InventoryItemInfoID },
                { (byte)SyncInventoryItemInfoChangeParameterCode.ItemID, info.Item.ItemID },
                { (byte)SyncInventoryItemInfoChangeParameterCode.ItemCount, info.Count },
                { (byte)SyncInventoryItemInfoChangeParameterCode.PositionIndex, info.PositionIndex },
                { (byte)SyncInventoryItemInfoChangeParameterCode.IsFavorite, info.IsFavorite }
            };
            SendSyncData(PlayerSyncDataCode.InventoryItemInfoChange, parameters);
        }
        public void SyncFriendInformationChange(DataChangeType changeType, FriendInformation information)
        {
            subject.SyncPlayerInformation(information.friendPlayerID);
            var parameters = new Dictionary<byte, object>
            {
                { (byte)SyncFriendInformationChangeParameterCode.DataChangeType, (byte)changeType },
                { (byte)SyncFriendInformationChangeParameterCode.FriendPlayerID, information.friendPlayerID },
                { (byte)SyncFriendInformationChangeParameterCode.IsInviter, information.isInviter },
                { (byte)SyncFriendInformationChangeParameterCode.IsConfirmed, information.isConfirmed }
            };
            SendSyncData(PlayerSyncDataCode.FriendInformationChange, parameters);
        }
        public void SyncPlayerInformation(PlayerInformation playerInformation)
        {
            var parameters = new Dictionary<byte, object>
            {
                { (byte)SyncPlayerInformationParameterCode.PlayerID, playerInformation.playerID },
                { (byte)SyncPlayerInformationParameterCode.Nickname, playerInformation.nickname },
                { (byte)SyncPlayerInformationParameterCode.Signature, playerInformation.signature },
                { (byte)SyncPlayerInformationParameterCode.GroupType, (byte)playerInformation.groupType },
                { (byte)SyncPlayerInformationParameterCode.VesselID, playerInformation.vesselID }
            };
            SendSyncData(PlayerSyncDataCode.PlayerInformation, parameters);
        }
        public void SyncTransactionItemChange(int transactionID, int playerID, DataChangeType changeType, TransactionItemInfo info)
        {
            var parameters = new Dictionary<byte, object>
            {
                { (byte)SyncTransactionItemChangeParameterCode.TransactionID, transactionID },
                { (byte)SyncTransactionItemChangeParameterCode.PlayerID, playerID },
                { (byte)SyncTransactionItemChangeParameterCode.DataChangeType, (byte)changeType },
                { (byte)SyncTransactionItemChangeParameterCode.ItemID, info.Item.ItemID },
                { (byte)SyncTransactionItemChangeParameterCode.ItemCount, info.Count },
                { (byte)SyncTransactionItemChangeParameterCode.PositionIndex, info.PositionIndex }
            };
            SendSyncData(PlayerSyncDataCode.TransactionItemChange, parameters);
        }
        public void SyncTransactionConfirmStatusChange(int transactionID, int confirmedPlayerID, bool isConfirmed)
        {
            var parameters = new Dictionary<byte, object>
            {
                { (byte)SyncTransactionConfirmStatusChangeParameterCode.TransactionID, transactionID },
                { (byte)SyncTransactionConfirmStatusChangeParameterCode.PlayerID, confirmedPlayerID },
                { (byte)SyncTransactionConfirmStatusChangeParameterCode.IsConfirmed, isConfirmed }
            };
            SendSyncData(PlayerSyncDataCode.TransactionConfirmStatusChange, parameters);
        }
        public void SyncQuestRecordUpdated(QuestRecord questRecord)
        {
            var parameters = new Dictionary<byte, object>
            {
                { (byte)SyncQuestRecordUpdatedParameterCode.QuestRecordDataByteArray, SerializationHelper.QuestRecordSerialize(questRecord) },
            };
            SendSyncData(PlayerSyncDataCode.QuestRecordUpdated, parameters);
        }
        public void SyncNextDrawMaterialTimeUpdated(DateTime nextDrawMaterialTime)
        {
            var parameters = new Dictionary<byte, object>
            {
                { (byte)SyncNextDrawMaterialTimeUpdatedParameterCode.NextDrawMaterialTime, nextDrawMaterialTime.ToBinary() },
            };
            SendSyncData(PlayerSyncDataCode.NextDrawMaterialTimeUpdated, parameters);
        }
    }
}
