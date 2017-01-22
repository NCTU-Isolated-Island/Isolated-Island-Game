using IsolatedIslandGame.Protocol.Communication.EventCodes;
using IsolatedIslandGame.Protocol.Communication.EventParameters.Player;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.PlayerEventHandlers
{
    class EndTransactionHandler : EventHandler<Player, PlayerEventCode>
    {
        public EndTransactionHandler(Player subject) : base(subject, 2)
        {
        }

        internal override bool Handle(PlayerEventCode eventCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(eventCode, parameters))
            {
                try
                {
                    int transactionID = (int)parameters[(byte)EndTransactionParameterCode.TransactionID];
                    bool isSuccessful = (bool)parameters[(byte)EndTransactionParameterCode.IsSuccessful];

                    Transaction transaction;
                    if(subject.FindTransaction(transactionID, out transaction))
                    {
                        transaction.EndTransaction(isSuccessful);
                        subject.RemoveTransaction(transactionID);
                        return true;
                    }
                    else
                    {
                        LogService.Error($"EndTransaction Error TransactionID NotExist, TransactionID: {transactionID}");
                        return false;
                    }
                }
                catch (InvalidCastException ex)
                {
                    LogService.Error("EndTransaction Parameter Cast Error");
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
