using IsolatedIslandGame.Library.Landmarks;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.LandmarkRoomOperationHandlers
{
    class ExitRoomHandler : LandmarkRoomOperationHandler
    {
        public ExitRoomHandler(LandmarkRoom landmarkRoom) : base(landmarkRoom, 0)
        {

        }
        internal override bool Handle(CommunicationInterface communicationInterface, LandmarkRoomOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(communicationInterface, operationCode, parameters))
            {
                try
                {
                    Player player = communicationInterface.User.Player;
                    if (subject.ContainsMutiplayerSynthesizeParticipant(player.PlayerID))
                    {
                        subject.RemoveMutiplayerSynthesizeParticipant(player.PlayerID);
                        return true;
                    }
                    else
                    {
                        LogService.ErrorFormat("ExitRoom Error Player ID: {0} Not in LandmarkRoom Identity: {1}", player.PlayerID, subject.IdentityInformation);
                        return false;
                    }
                }
                catch (InvalidCastException ex)
                {
                    LogService.ErrorFormat("ExitRoom Parameter Cast Error");
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
