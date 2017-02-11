using IsolatedIslandGame.Protocol.Communication.EventCodes;
using IsolatedIslandGame.Protocol.Communication.EventParameters.User;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.UserEventHandlers
{
    class UserInformHandler : EventHandler<User, UserEventCode>
    {
        public UserInformHandler(User user) : base(user, 2)
        {
        }
        internal override bool Handle(UserEventCode eventCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(eventCode, parameters))
            {
                string title = (string)parameters[(byte)UserInformParameterCode.Title];
                string content = (string)parameters[(byte)UserInformParameterCode.Content];
                subject.UserInform(title, content);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
