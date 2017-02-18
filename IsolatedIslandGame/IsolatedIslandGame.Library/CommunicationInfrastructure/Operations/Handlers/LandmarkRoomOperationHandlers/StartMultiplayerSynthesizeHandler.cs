using IsolatedIslandGame.Library.Landmarks;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.LandmarkRoomOperationHandlers
{
    class StartMultiplayerSynthesizeHandler : LandmarkRoomOperationHandler
    {
        public StartMultiplayerSynthesizeHandler(LandmarkRoom landmarkRoom) : base(landmarkRoom, 0)
        {
        }
        internal override bool Handle(CommunicationInterface communicationInterface, LandmarkRoomOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(communicationInterface, operationCode, parameters))
            {
                try
                {
                    Player player = communicationInterface.User.Player;
                    if (player.PlayerID == subject.HostPlayerID)
                    {
                        if (subject.CanStartMultiplayerSynthesize)
                        {
                            subject.StartMultiplayerSynthesize();
                            return true;
                        }
                        else
                        {
                            LogService.Error($"StartMultiplayerSynthesize Fail, not all player is checked");
                            return false;
                        }
                    }
                    else
                    {
                        LogService.Error($"StartMultiplayerSynthesize Fail Player ID: {player.IdentityInformation} is not the Host of Room ID: {subject.LandmarkRoomID}");
                        return false;
                    }
                }
                catch (InvalidCastException ex)
                {
                    LogService.ErrorFormat("StartMultiplayerSynthesize Parameter Cast Error");
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
