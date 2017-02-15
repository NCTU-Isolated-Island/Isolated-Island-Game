using IsolatedIslandGame.Library.Landmarks;
using IsolatedIslandGame.Protocol.Communication.EventCodes;
using IsolatedIslandGame.Protocol.Communication.SyncDataCodes;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.LandmarkEventHandlers
{
    public class LandmarkSyncDataResolver : SyncDataResolver<Landmark, LandmarkEventCode, LandmarkSyncDataCode>
    {
        internal LandmarkSyncDataResolver(Landmark landmark) : base(landmark)
        {

        }

        internal override void SendSyncData(LandmarkSyncDataCode syncCode, Dictionary<byte, object> parameters)
        {
            subject.EventManager.SendSyncDataEvent(syncCode, parameters);
        }
    }
}
