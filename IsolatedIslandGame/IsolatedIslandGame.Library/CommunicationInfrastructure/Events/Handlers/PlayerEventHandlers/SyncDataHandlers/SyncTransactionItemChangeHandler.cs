using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.SyncDataCodes;
using IsolatedIslandGame.Protocol.Communication.SyncDataParameters.Player;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.PlayerEventHandlers.SyncDataHandlers
{
    class SyncTransactionItemChangeHandler : SyncDataHandler<Player, PlayerSyncDataCode>
    {
        public SyncTransactionItemChangeHandler(Player subject) : base(subject, 6)
        {
        }
        internal override bool Handle(PlayerSyncDataCode syncCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(syncCode, parameters))
            {
                try
                {
                    int transactionID = (int)parameters[(byte)SyncTransactionItemChangeParameterCode.TransactionID];
                    int playerID = (int)parameters[(byte)SyncTransactionItemChangeParameterCode.PlayerID];
                    DataChangeType changeType = (DataChangeType)parameters[(byte)SyncTransactionItemChangeParameterCode.DataChangeType];
                    int itemID = (int)parameters[(byte)SyncTransactionItemChangeParameterCode.ItemID];
                    int itemCount = (int)parameters[(byte)SyncTransactionItemChangeParameterCode.ItemCount];
                    int positionIndex = (int)parameters[(byte)SyncTransactionItemChangeParameterCode.PositionIndex];

                    Transaction transaction;
                    if (subject.FindTransaction(transactionID, out transaction))
                    {
                        Item item;
                        if(ItemManager.Instance.FindItem(itemID, out item))
                        {
                            if (transaction.ChangeTransactionItem(playerID, changeType, new TransactionItemInfo(item, itemCount, positionIndex)))
                            {
                                return true;
                            }
                            else
                            {
                                LogService.Error($"SyncTransactionItemChange Fail, TransactionID: {transactionID}, PlayerID: {playerID}, DataChangeType: {changeType}, ItemID: {itemID}, ItemCount: {itemCount}, PositionIndex: {positionIndex}");
                                return false;
                            }
                        }
                        else
                        {
                            LogService.Error($"SyncTransactionItemChange Error ItemNotExist, TransactionID: {transactionID}, PlayerID: {playerID}, DataChangeType: {changeType}, ItemID: {itemID}, ItemCount: {itemCount}, PositionIndex: {positionIndex}");
                            return false;
                        }
                    }
                    else
                    {
                        LogService.Error($"SyncTransactionItemChange Error Transaction NotExist, TransactionID: {transactionID}");
                        return false;
                    }
                }
                catch (InvalidCastException ex)
                {
                    LogService.Error("SyncTransactionItemChange Parameter Cast Error");
                    LogService.Error(ex.Message);
                    LogService.Error(ex.StackTrace);
                    return false;
                }
                catch (Exception ex)
                {
                    LogService.Error(ex.Message);
                    LogService.Error(ex.StackTrace);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
