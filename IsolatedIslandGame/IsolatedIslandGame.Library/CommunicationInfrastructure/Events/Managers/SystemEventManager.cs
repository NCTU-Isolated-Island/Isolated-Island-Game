using IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers;
using IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.SystemEventHandlers;
using IsolatedIslandGame.Protocol.Communication.EventCodes;
using IsolatedIslandGame.Protocol.Communication.EventParameters.User;
using IsolatedIslandGame.Protocol.Communication.EventParameters.System;
using IsolatedIslandGame.Protocol.Communication.SyncDataCodes;
using IsolatedIslandGame.Protocol.Communication.SyncDataParameters;
using System.Collections.Generic;
using IsolatedIslandGame.Library.Landmarks;

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
                { SystemEventCode.IslandResetTodayMaterialRanking, new IslandResetTodayMaterialRankingHandler(systemManager) },
                { SystemEventCode.LandmarkEvent, new LandmarkEventResolver(systemManager) },
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
            systemManager.SendAllUserEvent(UserEventCode.SystemEvent, eventData);
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
        internal void SendLandmarkEvent(Landmark landmark, LandmarkEventCode eventCode, Dictionary<byte, object> parameters)
        {
            Dictionary<byte, object> eventData = new Dictionary<byte, object>
            {
                { (byte)LandmarkEventParameterCode.LandmarkID, landmark.LandmarkID },
                { (byte)LandmarkEventParameterCode.EventCode, (byte)eventCode },
                { (byte)LandmarkEventParameterCode.Parameters, parameters }
            };
            SendEvent(SystemEventCode.LandmarkEvent, eventData);
        }

        public void IslandResetTodayMaterialRanking()
        {
            SendEvent(SystemEventCode.IslandResetTodayMaterialRanking, new Dictionary<byte, object>());
        }
    }
}
