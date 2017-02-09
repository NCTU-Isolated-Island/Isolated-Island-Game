using IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers;
using IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.UserEventHandlers;
using IsolatedIslandGame.Protocol.Communication.EventCodes;
using IsolatedIslandGame.Protocol.Communication.EventParameters.User;
using IsolatedIslandGame.Protocol.Communication.SyncDataCodes;
using IsolatedIslandGame.Protocol.Communication.SyncDataParameters;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Managers
{
    public class UserEventManager
    {
        private readonly Dictionary<UserEventCode, EventHandler<User, UserEventCode>> eventTable;
        protected readonly User user;
        public UserSyncDataResolver SyncDataResolver { get; protected set; }

        internal UserEventManager(User user)
        {
            this.user = user;
            SyncDataResolver = new UserSyncDataResolver(user);
            eventTable = new Dictionary<UserEventCode, EventHandler<User, UserEventCode>>
            {
                { UserEventCode.SyncData, SyncDataResolver },
                { UserEventCode.PlayerEvent, new PlayerEventResolver(user) },
                { UserEventCode.SystemEvent, new SystemEventResolver(user) },
                { UserEventCode.UserInform, new UserInformHandler(user) },
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
            user.CommunicationInterface.SendEvent(eventCode, parameters);
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

        internal void SendSyncDataEvent(UserSyncDataCode syncCode, Dictionary<byte, object> parameters)
        {
            Dictionary<byte, object> syncDataParameters = new Dictionary<byte, object>
            {
                { (byte)SyncDataEventParameterCode.SyncCode, (byte)syncCode },
                { (byte)SyncDataEventParameterCode.Parameters, parameters }
            };
            SendEvent(UserEventCode.SyncData, syncDataParameters);
        }
        public void UserInform(string title, string content)
        {
            Dictionary<byte, object> eventParameters = new Dictionary<byte, object>
            {
                { (byte)UserInformParameterCode.Title, title },
                { (byte)UserInformParameterCode.Content, content }
            };
            SendEvent(UserEventCode.UserInform, eventParameters);
        }
    }
}
