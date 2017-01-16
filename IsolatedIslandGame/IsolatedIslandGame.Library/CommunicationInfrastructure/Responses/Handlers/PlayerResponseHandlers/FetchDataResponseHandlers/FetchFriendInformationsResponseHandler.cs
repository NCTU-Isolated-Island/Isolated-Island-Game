using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataResponseParameters.Player;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers.PlayerResponseHandlers.FetchDataResponseHandlers
{
    class FetchFriendInformationsResponseHandler : FetchDataResponseHandler<Player, PlayerFetchDataCode>
    {
        public FetchFriendInformationsResponseHandler(Player subject) : base(subject)
        {
        }

        public override bool CheckError(Dictionary<byte, object> parameters, ErrorCode returnCode, string debugMessage)
        {
            switch (returnCode)
            {
                case ErrorCode.NoError:
                    {
                        if (parameters.Count != 3)
                        {
                            LogService.ErrorFormat(string.Format("FetchFriendInformationsResponse Parameter Error, Parameter Count: {0}", parameters.Count));
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                default:
                    {
                        LogService.ErrorFormat("FetchFriendInformationsResponse Error DebugMessage: {0}", debugMessage);
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
                    int friendPlayerID = (int)parameters[(byte)FetchFriendInformationsResponseParameterCode.FriendPlayerID];
                    bool isInviter = (bool)parameters[(byte)FetchFriendInformationsResponseParameterCode.IsInviter];
                    bool isConfirmed = (bool)parameters[(byte)FetchFriendInformationsResponseParameterCode.IsConfirmed];

                    subject.AddFriend(new FriendInformation
                    {
                        friendPlayerID = friendPlayerID,
                        isInviter = isInviter,
                        isConfirmed = isConfirmed
                    });
                    return true;
                }
                catch (InvalidCastException ex)
                {
                    LogService.Error("FetchFriendInformationsResponse Parameter Cast Error");
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
