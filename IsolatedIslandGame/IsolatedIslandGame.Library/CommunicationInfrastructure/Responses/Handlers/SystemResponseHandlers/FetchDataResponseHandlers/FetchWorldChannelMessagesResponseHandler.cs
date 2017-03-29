using IsolatedIslandGame.Library.TextData;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataResponseParameters.System;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers.SystemResponseHandlers.FetchDataResponseHandlers
{
    class FetchWorldChannelMessagesResponseHandler : FetchDataResponseHandler<SystemManager, SystemFetchDataCode>
    {
        public FetchWorldChannelMessagesResponseHandler(SystemManager subject) : base(subject)
        {
        }

        public override bool CheckError(Dictionary<byte, object> parameters, ErrorCode returnCode, string debugMessage)
        {
            switch (returnCode)
            {
                case ErrorCode.NoError:
                    {
                        if (parameters.Count != 5)
                        {
                            LogService.ErrorFormat(string.Format("FetchWorldChannelMessagesResponse Parameter Error, Parameter Count: {0}", parameters.Count));
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                default:
                    {
                        LogService.ErrorFormat("FetchWorldChannelMessagesResponse Error DebugMessage: {0}", debugMessage);
                        return false;
                    }
            }
        }

        public override bool Handle(SystemFetchDataCode fetchCode, ErrorCode returnCode, string fetchDebugMessage, Dictionary<byte, object> parameters)
        {
            if (base.Handle(fetchCode, returnCode, fetchDebugMessage, parameters))
            {
                try
                {
                    int worldChannelMessagesID = (int)parameters[(byte)FetchWorldChannelMessagesResponseParameterCode.WorldChannelMessagesID];
                    int playerMessageID = (int)parameters[(byte)FetchWorldChannelMessagesResponseParameterCode.PlayerMessageID];
                    int senderPlayerID = (int)parameters[(byte)FetchWorldChannelMessagesResponseParameterCode.SenderPlayerID];
                    DateTime sendTime = DateTime.FromBinary((long)parameters[(byte)FetchWorldChannelMessagesResponseParameterCode.SendTime]);
                    string content = (string)parameters[(byte)FetchWorldChannelMessagesResponseParameterCode.Content];

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
                    LogService.Error("FetchWorldChannelMessagesResponse Parameter Cast Error");
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
