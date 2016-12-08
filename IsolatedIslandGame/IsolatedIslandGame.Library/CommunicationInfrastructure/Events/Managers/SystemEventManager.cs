using IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers;
using IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.SystemEventHandlers;
using IsolatedIslandGame.Protocol.Communication.EventCodes;
using IsolatedIslandGame.Protocol.Communication.EventParameters;
using IsolatedIslandGame.Protocol.Communication.SyncDataCodes;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Managers
{
    public class SystemEventManager
    {
        private readonly Dictionary<SystemEventCode, EventHandler<SystemManager, SystemEventCode>> eventTable;
        protected readonly SystemManager systemManager;
        public SystemSyncDataResolver SyncDataResolver { get; protected set; }

        internal SystemEventManager(SystemManager systemManager)
        {
            this.systemManager = systemManager;
            SyncDataResolver = new SystemSyncDataResolver(systemManager);
            eventTable = new Dictionary<SystemEventCode, EventHandler<SystemManager, SystemEventCode>>
            {
                { SystemEventCode.SyncData, SyncDataResolver },
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
            systemManager.User.EventManager.SendSystemEvent(eventCode, parameters);
        }

        public void ErrorInform(string title, string message)
        {
            systemManager.User.EventManager.ErrorInform(title, message);
        }

        internal void SendSyncDataEvent(SystemSyncDataCode syncCode, Dictionary<byte, object> parameters)
        {
            Dictionary<byte, object> syncDataParameters = new Dictionary<byte, object>
            {
                { (byte)SyncDataEventParameterCode.SyncCode, (byte)syncCode },
                { (byte)SyncDataEventParameterCode.Parameters, parameters }
            };
            SendEvent(SystemEventCode.SyncData, syncDataParameters);
        }
    }
}
