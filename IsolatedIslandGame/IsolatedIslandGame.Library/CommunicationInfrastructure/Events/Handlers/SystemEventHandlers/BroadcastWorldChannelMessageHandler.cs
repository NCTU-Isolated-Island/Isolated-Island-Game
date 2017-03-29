using IsolatedIslandGame.Library.TextData;
using IsolatedIslandGame.Protocol.Communication.EventCodes;
using IsolatedIslandGame.Protocol.Communication.EventParameters.System;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.SystemEventHandlers
{
    class BroadcastWorldChannelMessageHandler : EventHandler<SystemManager, SystemEventCode>
    {
        public BroadcastWorldChannelMessageHandler(SystemManager subject) : base(subject, 5)
        {
        }
        internal override bool Handle(SystemEventCode eventCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(eventCode, parameters))
            {
                try
                {
                    int worldChannelMessagesID = (int)parameters[(byte)BroadcastWorldChannelMessageParameterCode.WorldChannelMessagesID];
                    int playerMessageID = (int)parameters[(byte)BroadcastWorldChannelMessageParameterCode.PlayerMessageID];
                    int senderPlayerID = (int)parameters[(byte)BroadcastWorldChannelMessageParameterCode.SenderPlayerID];
                    DateTime sendTime = DateTime.FromBinary((long)parameters[(byte)BroadcastWorldChannelMessageParameterCode.SendTime]);
                    string content = (string)parameters[(byte)BroadcastWorldChannelMessageParameterCode.Content];

                    subject.LoadWorldChannelMessage(new WorldChannelMessage(worldChannelMessagesID, new PlayerMessage
                    {
                        playerMessageID = playerMessageID,
                        senderPlayerID = senderPlayerID,
                        sendTime = sendTime,
                        content = content
                    }));
                    return true;
                }
                catch (InvalidCastException ex)
                {
                    LogService.ErrorFormat("BroadcastWorldChannelMessage Parameter Cast Error");
                    LogService.ErrorFormat(ex.Message);
                    LogService.ErrorFormat(ex.StackTrace);
                    return false;
                }
                catch (Exception ex)
                {
                    LogService.ErrorFormat(ex.Message);
                    LogService.ErrorFormat(ex.StackTrace);
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
