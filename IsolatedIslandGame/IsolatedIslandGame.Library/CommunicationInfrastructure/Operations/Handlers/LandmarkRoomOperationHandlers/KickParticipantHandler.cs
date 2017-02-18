using IsolatedIslandGame.Library.Landmarks;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.OperationParameters.LandmarkRoom;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.LandmarkRoomOperationHandlers
{
    class KickParticipantHandler : LandmarkRoomOperationHandler
    {
        public KickParticipantHandler(LandmarkRoom landmarkRoom) : base(landmarkRoom, 1)
        {
        }
        internal override bool Handle(CommunicationInterface communicationInterface, LandmarkRoomOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(communicationInterface, operationCode, parameters))
            {
                try
                {
                    int participantPlayerID = (int)parameters[(byte)KickParticipantParameterCode.ParticipantPlayerID];
                    Player player = communicationInterface.User.Player;
                    if(player.PlayerID == subject.HostPlayerID)
                    {
                        if(subject.ContainsMutiplayerSynthesizeParticipant(participantPlayerID))
                        {
                            subject.RemoveMutiplayerSynthesizeParticipant(participantPlayerID);
                            return true;
                        }
                        else
                        {
                            LogService.Error($"KickParticipant Fail ParticipantPlayerID: {participantPlayerID} is not in the Room ID: {subject.LandmarkRoomID}");
                            return false;
                        }
                    }
                    else
                    {
                        LogService.Error($"KickParticipant Fail Player ID: {player.IdentityInformation} is not the Host of Room ID: {subject.LandmarkRoomID}");
                        return false;
                    }
                }
                catch (InvalidCastException ex)
                {
                    LogService.ErrorFormat("KickParticipant Parameter Cast Error");
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
