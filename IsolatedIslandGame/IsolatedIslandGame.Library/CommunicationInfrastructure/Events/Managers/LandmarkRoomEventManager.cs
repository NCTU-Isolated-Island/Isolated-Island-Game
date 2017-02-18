using IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers;
using IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.LandmarkRoomEventHandlers;
using IsolatedIslandGame.Library.Landmarks;
using IsolatedIslandGame.Protocol.Communication.EventCodes;
using IsolatedIslandGame.Protocol.Communication.SyncDataCodes;
using IsolatedIslandGame.Protocol.Communication.SyncDataParameters;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Managers
{
    public class LandmarkRoomEventManager
    {
        private readonly Dictionary<LandmarkRoomEventCode, EventHandler<LandmarkRoom, LandmarkRoomEventCode>> eventTable;
        protected readonly LandmarkRoom landmarkRoom;
        public LandmarkRoomSyncDataResolver SyncDataResolver { get; protected set; }

        internal LandmarkRoomEventManager(LandmarkRoom landmarkRoom)
        {
            this.landmarkRoom = landmarkRoom;
            SyncDataResolver = new LandmarkRoomSyncDataResolver(landmarkRoom);
            eventTable = new Dictionary<LandmarkRoomEventCode, EventHandler<LandmarkRoom, LandmarkRoomEventCode>>
            {
                { LandmarkRoomEventCode.SyncData, SyncDataResolver },
            };
        }

        internal void Operate(LandmarkRoomEventCode eventCode, Dictionary<byte, object> parameters)
        {
            if (eventTable.ContainsKey(eventCode))
            {
                if (!eventTable[eventCode].Handle(eventCode, parameters))
                {
                    LogService.ErrorFormat("LandmarkRoom Event Error: {0} from LandmarkRoomID: {1}", eventCode, landmarkRoom.LandmarkRoomID);
                }
            }
            else
            {
                LogService.ErrorFormat("Unknow LandmarkRoom Event:{0} from LandmarkRoomID: {1}", eventCode, landmarkRoom.LandmarkRoomID);
            }
        }

        internal void SendEvent(LandmarkRoomEventCode eventCode, Dictionary<byte, object> parameters)
        {
            landmarkRoom.Landmark.EventManager.SendLandmarkRoomEvent(landmarkRoom, eventCode, parameters);
        }

        internal void SendSyncDataEvent(LandmarkRoomSyncDataCode syncCode, Dictionary<byte, object> parameters)
        {
            Dictionary<byte, object> syncDataParameters = new Dictionary<byte, object>
            {
                { (byte)SyncDataEventParameterCode.SyncCode, (byte)syncCode },
                { (byte)SyncDataEventParameterCode.Parameters, parameters }
            };
            SendEvent(LandmarkRoomEventCode.SyncData, syncDataParameters);
        }
    }
}
