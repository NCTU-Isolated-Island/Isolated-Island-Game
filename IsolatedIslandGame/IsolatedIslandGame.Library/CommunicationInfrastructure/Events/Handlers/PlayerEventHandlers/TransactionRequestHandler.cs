using IsolatedIslandGame.Protocol.Communication.EventCodes;
using IsolatedIslandGame.Protocol.Communication.EventParameters.Player;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.PlayerEventHandlers
{
    class TransactionRequestHandler : EventHandler<Player, PlayerEventCode>
    {
        public TransactionRequestHandler(Player subject) : base(subject, 1)
        {
        }

        internal override bool Handle(PlayerEventCode eventCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(eventCode, parameters))
            {
                try
                {
                    int requesterPlayerID = (int)parameters[(byte)TransactionRequestParameterCode.RequesterPlayerID];

                    subject.TransactionRequest(requesterPlayerID);
                    return true;
                }
                catch (InvalidCastException ex)
                {
                    LogService.Error("TransactionRequest Parameter Cast Error");
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
