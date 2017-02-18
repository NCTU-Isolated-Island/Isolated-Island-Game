using IsolatedIslandGame.Library.Landmarks;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.OperationParameters.Landmark;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.LandmarkOperationHandlers
{
    class CreateLandmarkRoomHandler : LandmarkOperationHandler
    {
        internal CreateLandmarkRoomHandler(Landmark subject) : base(subject, 1)
        {
        }

        internal override bool Handle(CommunicationInterface communicationInterface, LandmarkOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(communicationInterface, operationCode, parameters))
            {
                try
                {
                    string roomName = (string)parameters[(byte)CreateLandmarkRoomParameterCode.RoomName];
                    LandmarkRoom room;
                    if (subject.CreateRoom(communicationInterface.User.Player.PlayerID, roomName, out room))
                    {
                        return true;
                    }
                    else
                    {
                        LogService.ErrorFormat($"CreateLandmarkRoom Fail Player ID: {communicationInterface.User.Player.PlayerID}");
                        return false;
                    }
                }
                catch (InvalidCastException ex)
                {
                    LogService.ErrorFormat("CreateLandmarkRoom Parameter Cast Error");
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
