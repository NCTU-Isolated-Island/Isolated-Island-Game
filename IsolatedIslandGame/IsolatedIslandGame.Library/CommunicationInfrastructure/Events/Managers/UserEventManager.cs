using IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers;
using IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.UserEventHandlers;
using IsolatedIslandGame.Protocol.Communication.EventCodes;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Managers
{
    public class UserEventManager
    {
        private readonly Dictionary<UserEventCode, EventHandler<User, UserEventCode>> eventTable;
        protected readonly User user;
        public UserInformDataResolver InformDataResolver { get; protected set; }

        internal UserEventManager(User user)
        {
            this.user = user;
            InformDataResolver = new UserInformDataResolver(user);
            eventTable = new Dictionary<UserEventCode, EventHandler<User, UserEventCode>>
            {
                { UserEventCode.InformData, InformDataResolver },
                { UserEventCode.PlayerEvent, new PlayerEventResolver(user) },
            };
        }

        public void Operate(UserEventCode eventCode, Dictionary<byte, object> parameters)
        {
            if (eventTable.ContainsKey(eventCode))
            {
                if (!eventTable[eventCode].Handle(eventCode, parameters))
                {
                    LogService.ErrorFormat("User Event Error: {0} from User IP: {1}", eventCode, user.LastConnectedIPAddress);
                }
            }
            else
            {
                LogService.ErrorFormat("Unknow User Event:{0} from User IP: {1}", eventCode, user.LastConnectedIPAddress);
            }
        }

        internal void SendEvent(UserEventCode eventCode, Dictionary<byte, object> parameters)
        {
            user.UserCommunicationInterface.SendEvent(eventCode, parameters);
        }

        public void ErrorInform(string title, string message)
        {
            user.UserCommunicationInterface.ErrorInform(title, message);
        }
    }
}
