using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataResponseParameters.Player;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.PlayerOperationHandlers.FetchDataHandlers
{
    class FetchAllPlayerConversationsHandler : PlayerFetchDataHandler
    {
        public FetchAllPlayerConversationsHandler(Player subject) : base(subject, 0)
        {
        }

        public override bool Handle(PlayerFetchDataCode fetchCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(fetchCode, parameters))
            {
                try
                {
                    foreach (var conversation in subject.User.CommunicationInterface.GetPlayerConversations(subject.PlayerID))
                    {
                        var result = new Dictionary<byte, object>
                        {
                            { (byte)FetchAllPlayerConversationsResponseParameterCode.PlayerMessageID, conversation.message.playerMessageID },
                            { (byte)FetchAllPlayerConversationsResponseParameterCode.SenderPlayerID, conversation.message.senderPlayerID },
                            { (byte)FetchAllPlayerConversationsResponseParameterCode.SendTime, conversation.message.sendTime.ToBinary() },
                            { (byte)FetchAllPlayerConversationsResponseParameterCode.Content, conversation.message.content },
                            { (byte)FetchAllPlayerConversationsResponseParameterCode.ReceiverPlayerID, conversation.receiverPlayerID },
                            { (byte)FetchAllPlayerConversationsResponseParameterCode.HasRead, conversation.hasRead }
                        };
                        SendResponse(fetchCode, result);
                    }
                    return true;
                }
                catch (InvalidCastException ex)
                {
                    LogService.ErrorFormat("FetchAllPlayerConversations Invalid Cast!");
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
