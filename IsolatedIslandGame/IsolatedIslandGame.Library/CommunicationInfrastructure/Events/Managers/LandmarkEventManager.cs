using IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers;
using IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.LandmarkEventHandlers;
using IsolatedIslandGame.Library.Landmarks;
using IsolatedIslandGame.Protocol.Communication.EventCodes;
using IsolatedIslandGame.Protocol.Communication.SyncDataCodes;
using IsolatedIslandGame.Protocol.Communication.SyncDataParameters;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Managers
{
    public class LandmarkEventManager
    {
        private readonly Dictionary<LandmarkEventCode, EventHandler<Landmark, LandmarkEventCode>> eventTable;
        protected readonly Landmark landmark;
        public LandmarkSyncDataResolver SyncDataResolver { get; protected set; }

        internal LandmarkEventManager(Landmark landmark)
        {
            this.landmark = landmark;
            SyncDataResolver = new LandmarkSyncDataResolver(landmark);
            eventTable = new Dictionary<LandmarkEventCode, EventHandler<Landmark, LandmarkEventCode>>
            {
                { LandmarkEventCode.SyncData, SyncDataResolver },
            };
        }

        internal void Operate(LandmarkEventCode eventCode, Dictionary<byte, object> parameters)
        {
            if (eventTable.ContainsKey(eventCode))
            {
                if (!eventTable[eventCode].Handle(eventCode, parameters))
                {
                    LogService.ErrorFormat("Landmark Event Error: {0} from LandmarkID: {1}", eventCode, landmark.LandmarkID);
                }
            }
            else
            {
                LogService.ErrorFormat("Unknow Landmark Event:{0} from LandmarkID: {1}", eventCode, landmark.LandmarkID);
            }
        }

        internal void SendEvent(LandmarkEventCode eventCode, Dictionary<byte, object> parameters)
        {
            SystemManager.Instance.EventManager.SendLandmarkEvent(landmark, eventCode, parameters);
        }

        internal void SendSyncDataEvent(LandmarkSyncDataCode syncCode, Dictionary<byte, object> parameters)
        {
            Dictionary<byte, object> syncDataParameters = new Dictionary<byte, object>
            {
                { (byte)SyncDataEventParameterCode.SyncCode, (byte)syncCode },
                { (byte)SyncDataEventParameterCode.Parameters, parameters }
            };
            SendEvent(LandmarkEventCode.SyncData, syncDataParameters);
        }
    }
}
