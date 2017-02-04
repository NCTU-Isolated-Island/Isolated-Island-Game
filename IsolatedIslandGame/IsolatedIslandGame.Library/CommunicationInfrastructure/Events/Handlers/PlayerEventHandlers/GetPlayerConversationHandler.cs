using IsolatedIslandGame.Protocol.Communication.EventCodes;
using IsolatedIslandGame.Protocol.Communication.EventParameters.Player;
using System;
using System.Collections.Generic;
using IsolatedIslandGame.Library.TextData;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.PlayerEventHandlers
{
    class GetPlayerConversationHandler : EventHandler<Player, PlayerEventCode>
    {
        public GetPlayerConversationHandler(Player subject) : base(subject, 6)
        {
        }

        internal override bool Handle(PlayerEventCode eventCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(eventCode, parameters))
            {
                try
                {
                    int playerMessageID = (int)parameters[(byte)GetPlayerConversationParameterCode.PlayerMessageID];
                    int senderPlayerID = (int)parameters[(byte)GetPlayerConversationParameterCode.SenderPlayerID];
                    DateTime sendTime = DateTime.FromBinary((long)parameters[(byte)GetPlayerConversationParameterCode.SendTime]);
                    string content = (string)parameters[(byte)GetPlayerConversationParameterCode.Content];
                    int receiverPlayerID = (int)parameters[(byte)GetPlayerConversationParameterCode.ReceiverPlayerID];
                    bool hasRead = (bool)parameters[(byte)GetPlayerConversationParameterCode.HasRead];

                    subject.GetPlayerConversation(new PlayerConversation
                    {
                        message = new PlayerMessage
                        {
                            playerMessageID = playerMessageID,
                            senderPlayerID = senderPlayerID,
                            sendTime = sendTime,
                            content = content
                        },
                        receiverPlayerID = receiverPlayerID,
                        hasRead = hasRead
                    });
                    return true;
                }
                catch (InvalidCastException ex)
                {
                    LogService.Error("GetPlayerConversation Parameter Cast Error");
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
