using IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers;
using IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.UserEventHandlers;
using IsolatedIslandGame.Protocol.Communication.EventCodes;
using IsolatedIslandGame.Protocol.Communication.EventParameters;
using IsolatedIslandGame.Protocol.Communication.EventParameters.User;
using IsolatedIslandGame.Protocol.Communication.InformDataCodes;
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
                { UserEventCode.SystemEvent, new SystemEventResolver(user) },
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
        internal void SendPlayerEvent(Player player, PlayerEventCode eventCode, Dictionary<byte, object> parameters)
        {
            Dictionary<byte, object> eventData = new Dictionary<byte, object>
            {
                { (byte)PlayerEventParameterCode.PlayerID, player.PlayerID },
                { (byte)PlayerEventParameterCode.EventCode, (byte)eventCode },
                { (byte)PlayerEventParameterCode.Parameters, parameters }
            };
            SendEvent(UserEventCode.PlayerEvent, eventData);
        }
        internal void SendSystemEvent(SystemEventCode eventCode, Dictionary<byte, object> parameters)
        {
            Dictionary<byte, object> eventData = new Dictionary<byte, object>
            {
                { (byte)SystemEventParameterCode.EventCode, (byte)eventCode },
                { (byte)SystemEventParameterCode.Parameters, parameters }
            };
            SendEvent(UserEventCode.SystemEvent, eventData);
        }

        public void ErrorInform(string title, string message)
        {
            user.UserCommunicationInterface.ErrorInform(title, message);
        }

        internal void SendInformDataEvent(UserInformDataCode informCode, Dictionary<byte, object> parameters)
        {
            Dictionary<byte, object> informDataParameters = new Dictionary<byte, object>
            {
                { (byte)InformDataEventParameterCode.InformCode, (byte)informCode },
                { (byte)InformDataEventParameterCode.Parameters, parameters }
            };
            SendEvent(UserEventCode.InformData, informDataParameters);
        }
    }
}
