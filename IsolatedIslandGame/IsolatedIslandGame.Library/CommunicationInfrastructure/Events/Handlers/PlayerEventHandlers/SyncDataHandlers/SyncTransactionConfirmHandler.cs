using IsolatedIslandGame.Protocol.Communication.SyncDataCodes;
using IsolatedIslandGame.Protocol.Communication.SyncDataParameters.Player;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.PlayerEventHandlers.SyncDataHandlers
{
    class SyncTransactionConfirmHandler : SyncDataHandler<Player, PlayerSyncDataCode>
    {
        public SyncTransactionConfirmHandler(Player subject) : base(subject, 2)
        {
        }
        internal override bool Handle(PlayerSyncDataCode syncCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(syncCode, parameters))
            {
                try
                {
                    int transactionID = (int)parameters[(byte)SyncTransactionConfirmParameterCode.TransactionID];
                    int confirmedPlayerID = (int)parameters[(byte)SyncTransactionConfirmParameterCode.ConfirmedPlayerID];

                    Transaction transaction;
                    if (subject.FindTransaction(transactionID, out transaction))
                    {
                        if(transaction.Confirm(confirmedPlayerID))
                        {
                            return true;
                        }
                        else
                        {
                            LogService.Error($"SyncTransactionConfirm Error ConfirmedPlayerID Not In The Transaction, TransactionID: {transactionID}, ConfirmedPlayerID: {confirmedPlayerID}, Transaction RequesterPlayerID: {transaction.RequesterPlayerID}, Transaction AccepterPlayerID: {transaction.AccepterPlayerID}");
                            return false;
                        }
                    }
                    else
                    {
                        LogService.Error($"SyncTransactionConfirm Error Transaction NotExist, TransactionID: {transactionID}, ConfirmedPlayerID: {confirmedPlayerID}");
                        return false;
                    }
                }
                catch (InvalidCastException ex)
                {
                    LogService.Error("SyncTransactionConfirm Parameter Cast Error");
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
