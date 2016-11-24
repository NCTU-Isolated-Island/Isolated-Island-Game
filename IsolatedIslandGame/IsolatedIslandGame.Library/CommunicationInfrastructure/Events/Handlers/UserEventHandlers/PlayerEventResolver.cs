using IsolatedIslandGame.Protocol.Communication.EventCodes;
using IsolatedIslandGame.Protocol.Communication.EventParameters.User;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.UserEventHandlers
{
    internal class PlayerEventResolver : EventHandler<User, UserEventCode>
    {
        internal PlayerEventResolver(User user) : base(user, 3)
        {
        }

        internal override bool Handle(UserEventCode eventCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(eventCode, parameters))
            {
                try
                {
                    int playerID = (int)parameters[(byte)PlayerEventParameterCode.PlayerID];
                    PlayerEventCode resolvedEventCode = (PlayerEventCode)parameters[(byte)PlayerEventParameterCode.EventCode];
                    Dictionary<byte, object> resolvedParameters = (Dictionary<byte, object>)parameters[(byte)PlayerEventParameterCode.Parameters];
                    if (subject.Player.PlayerID == playerID)
                    {
                        subject.Player.EventManager.Operate(resolvedEventCode, resolvedParameters);
                        return true;
                    }
                    else
                    {
                        LogService.ErrorFormat("PlayerEvent Error Player ID: {0} Not in Idnetity: {1}", playerID, subject.IdentityInformation);
                        return false;
                    }
                }
                catch (InvalidCastException ex)
                {
                    LogService.ErrorFormat("PlayerEvent Parameter Cast Error");
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
