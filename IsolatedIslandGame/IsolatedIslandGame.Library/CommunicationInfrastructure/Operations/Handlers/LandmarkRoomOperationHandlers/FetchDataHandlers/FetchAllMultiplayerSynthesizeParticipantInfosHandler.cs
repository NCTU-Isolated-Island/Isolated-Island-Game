using IsolatedIslandGame.Library.Landmarks;
using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataResponseParameters.LandmarkRoom;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.LandmarkRoomOperationHandlers.FetchDataHandlers
{
    class FetchAllMultiplayerSynthesizeParticipantInfosHandler : LandmarkRoomFetchDataHandler
    {
        public FetchAllMultiplayerSynthesizeParticipantInfosHandler(LandmarkRoom subject) : base(subject, 0)
        {
        }

        public override bool Handle(CommunicationInterface communicationInterface, LandmarkRoomFetchDataCode fetchCode, Dictionary<byte, object> parameter)
        {
            if (base.Handle(communicationInterface, fetchCode, parameter))
            {
                try
                {
                    foreach (var info in subject.MutiplayerSynthesizeParticipationInfos)
                    {
                        communicationInterface.User.Player.SyncPlayerInformation(info.participantPlayerID);
                        var result = new Dictionary<byte, object>
                        {
                            { (byte)FetchAllMultiplayerSynthesizeParticipantInfosResponseParameterCode.ParticipantPlayerID, info.participantPlayerID },
                            { (byte)FetchAllMultiplayerSynthesizeParticipantInfosResponseParameterCode.ProvidedItemID, info.providedItemID },
                            { (byte)FetchAllMultiplayerSynthesizeParticipantInfosResponseParameterCode.ProvidedItemCount, info.providedItemCount },
                            { (byte)FetchAllMultiplayerSynthesizeParticipantInfosResponseParameterCode.IsChecked, info.isChecked }
                        };
                        SendResponse(communicationInterface, fetchCode, result);
                    }
                    return true;
                }
                catch (InvalidCastException ex)
                {
                    LogService.Error("FetchAllMultiplayerSynthesizeParticipantInfos Invalid Cast!");
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
