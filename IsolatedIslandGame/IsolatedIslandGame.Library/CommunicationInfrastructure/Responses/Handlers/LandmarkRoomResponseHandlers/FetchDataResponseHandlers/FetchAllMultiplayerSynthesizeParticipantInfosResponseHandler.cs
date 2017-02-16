using IsolatedIslandGame.Library.Landmarks;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataResponseParameters.LandmarkRoom;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers.LandmarkRoomResponseHandlers.FetchDataResponseHandlers
{
    class FetchAllMultiplayerSynthesizeParticipantInfosResponseHandler : FetchDataResponseHandler<LandmarkRoom, LandmarkRoomFetchDataCode>
    {
        public FetchAllMultiplayerSynthesizeParticipantInfosResponseHandler(LandmarkRoom subject) : base(subject)
        {
        }

        public override bool CheckError(Dictionary<byte, object> parameters, ErrorCode returnCode, string debugMessage)
        {
            switch (returnCode)
            {
                case ErrorCode.NoError:
                    {
                        if (parameters.Count != 4)
                        {
                            LogService.ErrorFormat(string.Format("FetchAllMultiplayerSynthesizeParticipantInfosResponse Parameter Error, Parameter Count: {0}", parameters.Count));
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                default:
                    {
                        LogService.ErrorFormat("FetchAllMultiplayerSynthesizeParticipantInfosResponse Error DebugMessage: {0}", debugMessage);
                        return false;
                    }
            }
        }

        public override bool Handle(LandmarkRoomFetchDataCode fetchCode, ErrorCode returnCode, string fetchDebugMessage, Dictionary<byte, object> parameters)
        {
            if (base.Handle(fetchCode, returnCode, fetchDebugMessage, parameters))
            {
                try
                {
                    int participantPlayerID = (int)parameters[(byte)FetchAllMultiplayerSynthesizeParticipantInfosResponseParameterCode.ParticipantPlayerID];
                    int providedItemID = (int)parameters[(byte)FetchAllMultiplayerSynthesizeParticipantInfosResponseParameterCode.ProvidedItemID];
                    int providedItemCount = (int)parameters[(byte)FetchAllMultiplayerSynthesizeParticipantInfosResponseParameterCode.ProvidedItemCount];
                    bool isChecked = (bool)parameters[(byte)FetchAllMultiplayerSynthesizeParticipantInfosResponseParameterCode.IsChecked];

                    subject.LoadMutiplayerSynthesizeParticipantInfo(new MutiplayerSynthesizeParticipantInfo
                    {
                        participantPlayerID = participantPlayerID,
                        providedItemID = providedItemID,
                        providedItemCount = providedItemCount,
                        isChecked = isChecked
                    });
                    return true;
                }
                catch (InvalidCastException ex)
                {
                    LogService.Error("FetchAllMultiplayerSynthesizeParticipantInfosResponse Parameter Cast Error");
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
