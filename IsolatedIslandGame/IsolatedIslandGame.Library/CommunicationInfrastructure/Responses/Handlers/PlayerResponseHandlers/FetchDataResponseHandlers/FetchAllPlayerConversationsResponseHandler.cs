using IsolatedIslandGame.Library.TextData;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataResponseParameters.Player;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers.PlayerResponseHandlers.FetchDataResponseHandlers
{
    class FetchAllPlayerConversationsResponseHandler : FetchDataResponseHandler<Player, PlayerFetchDataCode>
    {
        public FetchAllPlayerConversationsResponseHandler(Player subject) : base(subject)
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
                            LogService.ErrorFormat(string.Format("FetchAllPlayerConversationsResponse Parameter Error, Parameter Count: {0}", parameters.Count));
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                default:
                    {
                        LogService.ErrorFormat("FetchAllPlayerConversationsResponse Error DebugMessage: {0}", debugMessage);
                        return false;
                    }
            }
        }

        public override bool Handle(PlayerFetchDataCode fetchCode, ErrorCode returnCode, string fetchDebugMessage, Dictionary<byte, object> parameters)
        {
            if (base.Handle(fetchCode, returnCode, fetchDebugMessage, parameters))
            {
                try
                {
                    int playerMessageID = (int)parameters[(byte)FetchAllPlayerConversationsResponseParameterCode.PlayerMessageID];
                    int senderPlayerID = (int)parameters[(byte)FetchAllPlayerConversationsResponseParameterCode.SenderPlayerID];
                    DateTime sendTime = DateTime.FromBinary((long)parameters[(byte)FetchAllPlayerConversationsResponseParameterCode.SendTime]);
                    string content = (string)parameters[(byte)FetchAllPlayerConversationsResponseParameterCode.Content];
                    bool hasRead = (bool)parameters[(byte)FetchAllPlayerConversationsResponseParameterCode.HasRead];

                    subject.GetPlayerConversation(new PlayerConversation
                    {
                        message = new PlayerMessage
                        {
                            playerMessageID = playerMessageID,
                            senderPlayerID = senderPlayerID,
                            sendTime = sendTime,
                            content = content
                        },
                        hasRead = hasRead
                    });
                    return true;
                }
                catch (InvalidCastException ex)
                {
                    LogService.Error("FetchAllPlayerConversationsResponse Parameter Cast Error");
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
