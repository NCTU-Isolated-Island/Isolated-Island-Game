using IsolatedIslandGame.Library.Landmarks;
using IsolatedIslandGame.Protocol.Communication.EventCodes;
using IsolatedIslandGame.Protocol.Communication.SyncDataCodes;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.LandmarkRoomEventHandlers
{
    public class LandmarkRoomSyncDataResolver : SyncDataResolver<LandmarkRoom, LandmarkRoomEventCode, LandmarkRoomSyncDataCode>
    {
        internal LandmarkRoomSyncDataResolver(LandmarkRoom landmarkRoom) : base(landmarkRoom)
        {

        }

        internal override void SendSyncData(LandmarkRoomSyncDataCode syncCode, Dictionary<byte, object> parameters)
        {
            subject.EventManager.SendSyncDataEvent(syncCode, parameters);
        }
    }
}
