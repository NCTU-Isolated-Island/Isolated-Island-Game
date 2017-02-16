using IsolatedIslandGame.Library.Landmarks;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.OperationParameters.Landmark;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.LandmarkOperationHandlers
{
    class EnterLandmarkRoomHandler : LandmarkOperationHandler
    {
        internal EnterLandmarkRoomHandler(Landmark subject) : base(subject, 1)
        {
        }

        internal override bool Handle(CommunicationInterface communicationInterface, LandmarkOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(communicationInterface, operationCode, parameters))
            {
                try
                {
                    int landmarkRoomID = (int)parameters[(byte)EnterLandmarkRoomParameterCode.LandmarkRoomID];
                    LandmarkRoom room;
                    if (subject.FindRoom(landmarkRoomID, out room))
                    {
                        room.LoadMutiplayerSynthesizeParticipantInfo(new MutiplayerSynthesizeParticipantInfo
                        {
                            participantPlayerID = communicationInterface.User.Player.PlayerID,
                            providedItemID = 0,
                            providedItemCount = 0,
                            isChecked = false
                        });
                        return true;
                    }
                    else
                    {
                        LogService.ErrorFormat("EnterLandmarkRoom Error LandmarkRoomID ID: {0} Not in Identity: {1}", landmarkRoomID, subject.IdentityInformation);
                        return false;
                    }
                }
                catch (InvalidCastException ex)
                {
                    LogService.ErrorFormat("EnterLandmarkRoom Parameter Cast Error");
                    LogService.ErrorFormat(ex.Message);
                    LogService.ErrorFormat(ex.StackTrace);
                    return false;
                }
                catch (Exception ex)
                {
                    LogService.ErrorFormat(ex.Message);
                    LogService.ErrorFormat(ex.StackTrace);
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
