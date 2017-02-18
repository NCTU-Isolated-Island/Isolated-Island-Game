using IsolatedIslandGame.Library.Landmarks;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.OperationParameters.LandmarkRoom;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.LandmarkRoomOperationHandlers
{
    class ChangeMultiplayerSynthesizeCheckStatusHandler : LandmarkRoomOperationHandler
    {
        public ChangeMultiplayerSynthesizeCheckStatusHandler(LandmarkRoom landmarkRoom) : base(landmarkRoom, 1)
        {
        }
        internal override bool Handle(CommunicationInterface communicationInterface, LandmarkRoomOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(communicationInterface, operationCode, parameters))
            {
                try
                {
                    bool checkStatus = (bool)parameters[(byte)ChangeMultiplayerSynthesizeCheckStatusParameterCode.CheckStatus];
                    MultiplayerSynthesizeParticipantInfo info;
                    if (subject.FindMutiplayerSynthesizeParticipantInfo(communicationInterface.User.Player.PlayerID, out info))
                    {
                        info.isChecked = checkStatus;
                        subject.LoadMutiplayerSynthesizeParticipantInfo(info);
                        return true;
                    }
                    else
                    {
                        LogService.ErrorFormat("ChangeMultiplayerSynthesizeCheckStatus Error Player ID: {0} Not in LandmarkRoom Identity: {1}", communicationInterface.User.Player.PlayerID, subject.IdentityInformation);
                        return false;
                    }
                }
                catch (InvalidCastException ex)
                {
                    LogService.ErrorFormat("ChangeMultiplayerSynthesizeCheckStatus Parameter Cast Error");
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
