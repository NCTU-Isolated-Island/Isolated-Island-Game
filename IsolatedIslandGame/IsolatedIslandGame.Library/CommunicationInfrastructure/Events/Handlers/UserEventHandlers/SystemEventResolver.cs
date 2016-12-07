using IsolatedIslandGame.Protocol.Communication.EventCodes;
using IsolatedIslandGame.Protocol.Communication.EventParameters.User;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.UserEventHandlers
{
    internal class SystemEventResolver : EventHandler<User, UserEventCode>
    {
        internal SystemEventResolver(User user) : base(user, 2)
        {
        }

        internal override bool Handle(UserEventCode eventCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(eventCode, parameters))
            {
                try
                {
                    SystemEventCode resolvedEventCode = (SystemEventCode)parameters[(byte)SystemEventParameterCode.EventCode];
                    Dictionary<byte, object> resolvedParameters = (Dictionary<byte, object>)parameters[(byte)SystemEventParameterCode.Parameters];
                    SystemManager.Instance.EventManager.Operate(resolvedEventCode, resolvedParameters);
                    return true;
                }
                catch (InvalidCastException ex)
                {
                    LogService.ErrorFormat("SystemEvent Parameter Cast Error");
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
