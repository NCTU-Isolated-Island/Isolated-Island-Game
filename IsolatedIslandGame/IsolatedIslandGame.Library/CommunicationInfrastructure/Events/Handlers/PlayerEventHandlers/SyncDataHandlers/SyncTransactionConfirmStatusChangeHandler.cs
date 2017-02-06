using IsolatedIslandGame.Protocol.Communication.SyncDataCodes;
using IsolatedIslandGame.Protocol.Communication.SyncDataParameters.Player;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.PlayerEventHandlers.SyncDataHandlers
{
    class SyncTransactionConfirmStatusChangeHandler : SyncDataHandler<Player, PlayerSyncDataCode>
    {
        public SyncTransactionConfirmStatusChangeHandler(Player subject) : base(subject, 3)
        {
        }
        internal override bool Handle(PlayerSyncDataCode syncCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(syncCode, parameters))
            {
                try
                {
                    int transactionID = (int)parameters[(byte)SyncTransactionConfirmStatusChangeParameterCode.TransactionID];
                    int confirmedPlayerID = (int)parameters[(byte)SyncTransactionConfirmStatusChangeParameterCode.PlayerID];
                    bool isConfirmed = (bool)parameters[(byte)SyncTransactionConfirmStatusChangeParameterCode.IsConfirmed];

                    Transaction transaction;
                    if (subject.FindTransaction(transactionID, out transaction))
                    {
                        if(transaction.ChangeConfirmStatus(confirmedPlayerID, isConfirmed))
                        {
                            return true;
                        }
                        else
                        {
                            LogService.Error($"SyncTransactionConfirmStatusChange Error ConfirmedPlayerID Not In The Transaction, TransactionID: {transactionID}, ConfirmedPlayerID: {confirmedPlayerID}, IsConfirmed: {isConfirmed}, Transaction RequesterPlayerID: {transaction.RequesterPlayerID}, Transaction AccepterPlayerID: {transaction.AccepterPlayerID}");
                            return false;
                        }
                    }
                    else
                    {
                        LogService.Error($"SyncTransactionConfirmStatusChange Error Transaction NotExist, TransactionID: {transactionID}, ConfirmedPlayerID: {confirmedPlayerID}, IsConfirmed: {isConfirmed}");
                        return false;
                    }
                }
                catch (InvalidCastException ex)
                {
                    LogService.Error("SyncTransactionConfirmStatusChange Parameter Cast Error");
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
