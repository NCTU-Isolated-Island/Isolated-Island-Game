using IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.LandmarkRoomEventHandlers.SyncDataHandlers;
using IsolatedIslandGame.Library.Landmarks;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.EventCodes;
using IsolatedIslandGame.Protocol.Communication.SyncDataCodes;
using IsolatedIslandGame.Protocol.Communication.SyncDataParameters.LandmarkRoom;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.LandmarkRoomEventHandlers
{
    public class LandmarkRoomSyncDataResolver : SyncDataResolver<LandmarkRoom, LandmarkRoomEventCode, LandmarkRoomSyncDataCode>
    {
        internal LandmarkRoomSyncDataResolver(LandmarkRoom subject) : base(subject)
        {
            syncTable.Add(LandmarkRoomSyncDataCode.MultiplayerSynthesizeParticipantInfoChange, new SyncMultiplayerSynthesizeParticipantInfoChangeHandler(subject));
        }

        internal override void SendSyncData(LandmarkRoomSyncDataCode syncCode, Dictionary<byte, object> parameters)
        {
            subject.EventManager.SendSyncDataEvent(syncCode, parameters);
        }

        public void SyncMultiplayerSynthesizeParticipantInfoChange(DataChangeType changeType, MultiplayerSynthesizeParticipantInfo info)
        {
            PlayerInformation playerInformation;
            if (PlayerInformationManager.Instance.FindPlayerInformation(info.participantPlayerID, out playerInformation))
            {
                SystemManager.Instance.EventManager.SyncDataResolver.SyncPlayerInformation(playerInformation);
            }
            var parameters = new Dictionary<byte, object>
            {
                { (byte)SyncMultiplayerSynthesizeParticipantInfoChangeParameterCode.DataChangeType, (byte)changeType },
                { (byte)SyncMultiplayerSynthesizeParticipantInfoChangeParameterCode.ParticipantPlayerID, info.participantPlayerID },
                { (byte)SyncMultiplayerSynthesizeParticipantInfoChangeParameterCode.ProvidedItemID, info.providedItemID },
                { (byte)SyncMultiplayerSynthesizeParticipantInfoChangeParameterCode.ProvidedItemCount, info.providedItemID },
                { (byte)SyncMultiplayerSynthesizeParticipantInfoChangeParameterCode.IsChecked, info.isChecked }
            };
            SendSyncData(LandmarkRoomSyncDataCode.MultiplayerSynthesizeParticipantInfoChange, parameters);
        }
    }
}
