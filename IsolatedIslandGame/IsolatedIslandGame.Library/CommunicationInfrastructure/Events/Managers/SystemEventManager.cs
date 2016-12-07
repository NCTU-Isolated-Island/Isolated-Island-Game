using IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers;
using IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.SystemEventHandlers;
using IsolatedIslandGame.Protocol.Communication.EventCodes;
using IsolatedIslandGame.Protocol.Communication.EventParameters.User;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Managers
{
    public class SystemEventManager
    {
        private readonly Dictionary<SystemEventCode, EventHandler<SystemManager, SystemEventCode>> eventTable;
        protected readonly SystemManager systemManager;
        public SystemInformDataResolver InformDataResolver { get; protected set; }

        internal SystemEventManager(SystemManager systemManager)
        {
            this.systemManager = systemManager;
            InformDataResolver = new SystemInformDataResolver(systemManager);
            eventTable = new Dictionary<SystemEventCode, EventHandler<SystemManager, SystemEventCode>>
            {
                { SystemEventCode.InformData, InformDataResolver },
            };
        }

        internal void Operate(SystemEventCode eventCode, Dictionary<byte, object> parameters)
        {
            if (eventTable.ContainsKey(eventCode))
            {
                if (!eventTable[eventCode].Handle(eventCode, parameters))
                {
                    LogService.ErrorFormat("System Event Error: {0} from {1}", eventCode, systemManager.IdentityInformation);
                }
            }
            else
            {
                LogService.ErrorFormat("Unknow System Event:{0} from {1}", eventCode, systemManager.IdentityInformation);
            }
        }

        internal void SendEvent(SystemEventCode eventCode, Dictionary<byte, object> parameters)
        {
            Dictionary<byte, object> eventData = new Dictionary<byte, object>
            {
                { (byte)SystemEventParameterCode.EventCode, (byte)eventCode },
                { (byte)SystemEventParameterCode.Parameters, parameters }
            };
            systemManager.User.EventManager.SendEvent(UserEventCode.SystemEvent, eventData);
        }

        public void ErrorInform(string title, string message)
        {
            systemManager.User.EventManager.ErrorInform(title, message);
        }
    }
}
