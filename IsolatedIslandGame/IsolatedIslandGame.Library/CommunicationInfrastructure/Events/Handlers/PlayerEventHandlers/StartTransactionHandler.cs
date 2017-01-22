using IsolatedIslandGame.Protocol.Communication.EventCodes;
using IsolatedIslandGame.Protocol.Communication.EventParameters.Player;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.PlayerEventHandlers
{
    class StartTransactionHandler : EventHandler<Player, PlayerEventCode>
    {
        public StartTransactionHandler(Player subject) : base(subject, 3)
        {
        }

        internal override bool Handle(PlayerEventCode eventCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(eventCode, parameters))
            {
                try
                {
                    int transactionID = (int)parameters[(byte)StartTransactionParameterCode.TransactionID];
                    int requesterPlayerID = (int)parameters[(byte)StartTransactionParameterCode.RequesterPlayerID];
                    int accepterPlayerID = (int)parameters[(byte)StartTransactionParameterCode.AccepterPlayerID];

                    subject.AddTransaction(new Transaction(transactionID, requesterPlayerID, accepterPlayerID));
                    return true;
                }
                catch (InvalidCastException ex)
                {
                    LogService.Error("StartTransaction Parameter Cast Error");
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
