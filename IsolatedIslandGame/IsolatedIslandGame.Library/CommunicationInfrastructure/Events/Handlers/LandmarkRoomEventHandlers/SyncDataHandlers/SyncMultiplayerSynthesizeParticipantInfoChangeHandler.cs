using IsolatedIslandGame.Library.Landmarks;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.SyncDataCodes;
using IsolatedIslandGame.Protocol.Communication.SyncDataParameters.LandmarkRoom;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.LandmarkRoomEventHandlers.SyncDataHandlers
{
    class SyncMultiplayerSynthesizeParticipantInfoChangeHandler : SyncDataHandler<LandmarkRoom, LandmarkRoomSyncDataCode>
    {
        public SyncMultiplayerSynthesizeParticipantInfoChangeHandler(LandmarkRoom subject) : base(subject, 4)
        {
        }
        internal override bool Handle(LandmarkRoomSyncDataCode syncCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(syncCode, parameters))
            {
                try
                {
                    DataChangeType changeType = (DataChangeType)parameters[(byte)SyncMultiplayerSynthesizeParticipantInfoChangeParameterCode.DataChangeType];
                    int participantPlayerID = (int)parameters[(byte)SyncMultiplayerSynthesizeParticipantInfoChangeParameterCode.ParticipantPlayerID];
                    int providedItemID = (int)parameters[(byte)SyncMultiplayerSynthesizeParticipantInfoChangeParameterCode.ProvidedItemID];
                    int providedItemCount = (int)parameters[(byte)SyncMultiplayerSynthesizeParticipantInfoChangeParameterCode.ProvidedItemCount];
                    bool isChecked = (bool)parameters[(byte)SyncMultiplayerSynthesizeParticipantInfoChangeParameterCode.IsChecked];

                    MultiplayerSynthesizeParticipantInfo info = new MultiplayerSynthesizeParticipantInfo
                    {
                        participantPlayerID = participantPlayerID,
                        providedItemID = providedItemID,
                        providedItemCount = providedItemCount,
                        isChecked = isChecked
                    };
                    switch (changeType)
                    {
                        case DataChangeType.Add:
                        case DataChangeType.Update:
                            subject.LoadMutiplayerSynthesizeParticipantInfo(info);
                            break;
                        case DataChangeType.Remove:
                            subject.RemoveMutiplayerSynthesizeParticipant(participantPlayerID);
                            break;
                        default:
                            LogService.FatalFormat("SyncMultiplayerSynthesizeParticipantInfoChange undefined DataChangeType: {0}", changeType);
                            return false;
                    }
                    return true;
                }
                catch (InvalidCastException ex)
                {
                    LogService.Error("SyncMultiplayerSynthesizeParticipantInfoChange Parameter Cast Error");
                    LogService.Error(ex.Message);
                    LogService.Error(ex.StackTrace);
                    return false;
                }
                catch (Exception ex)
                {
                    LogService.Error(ex.Message);
                    LogService.Error(ex.StackTrace);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
