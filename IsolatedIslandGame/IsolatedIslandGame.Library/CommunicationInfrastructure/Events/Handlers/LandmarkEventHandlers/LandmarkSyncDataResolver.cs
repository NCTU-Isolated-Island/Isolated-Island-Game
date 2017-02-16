using IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.LandmarkEventHandlers.SyncDataHandlers;
using IsolatedIslandGame.Library.Landmarks;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.EventCodes;
using IsolatedIslandGame.Protocol.Communication.SyncDataCodes;
using IsolatedIslandGame.Protocol.Communication.SyncDataParameters.Landmark;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.LandmarkEventHandlers
{
    public class LandmarkSyncDataResolver : SyncDataResolver<Landmark, LandmarkEventCode, LandmarkSyncDataCode>
    {
        internal LandmarkSyncDataResolver(Landmark landmark) : base(landmark)
        {
            syncTable.Add(LandmarkSyncDataCode.LandmarkRoomChange, new SyncLandmarkRoomChangeHandler(subject));
        }

        internal override void SendSyncData(LandmarkSyncDataCode syncCode, Dictionary<byte, object> parameters)
        {
            subject.EventManager.SendSyncDataEvent(syncCode, parameters);
        }

        public void SyncLandmarkRoomChange(DataChangeType changeType, LandmarkRoom room)
        {
            PlayerInformation playerInformation;
            if (PlayerInformationManager.Instance.FindPlayerInformation(room.HostPlayerID, out playerInformation))
            {
                SystemManager.Instance.EventManager.SyncDataResolver.SyncPlayerInformation(playerInformation);
            }
            var parameters = new Dictionary<byte, object>
            {
                { (byte)SyncLandmarkRoomChangeParameterCode.DataChangeType, (byte)changeType },
                { (byte)SyncLandmarkRoomChangeParameterCode.LandmarkRoomID, room.LandmarkRoomID },
                { (byte)SyncLandmarkRoomChangeParameterCode.RoomName, room.RoomName },
                { (byte)SyncLandmarkRoomChangeParameterCode.HostPlayerID, room.HostPlayerID }
            };
            SendSyncData(LandmarkSyncDataCode.LandmarkRoomChange, parameters);
        }
    }
}
