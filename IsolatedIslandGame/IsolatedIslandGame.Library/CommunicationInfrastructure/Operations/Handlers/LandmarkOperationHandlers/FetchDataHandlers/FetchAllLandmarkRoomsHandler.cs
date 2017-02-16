using IsolatedIslandGame.Library.Landmarks;
using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataResponseParameters.Landmark;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.LandmarkOperationHandlers.FetchDataHandlers
{
    class FetchAllLandmarkRoomsHandler : LandmarkFetchDataHandler
    {
        public FetchAllLandmarkRoomsHandler(Landmark subject) : base(subject, 0)
        {
        }

        public override bool Handle(CommunicationInterface communicationInterface, LandmarkFetchDataCode fetchCode, Dictionary<byte, object> parameter)
        {
            if (base.Handle(communicationInterface, fetchCode, parameter))
            {
                try
                {
                    foreach (LandmarkRoom landmarkRoom in landmark.Rooms)
                    {
                        var result = new Dictionary<byte, object>
                        {
                            { (byte)FetchAllLandmarkRoomsResponseParameterCode.LandmarkRoomID, landmarkRoom.LandmarkRoomID },
                            { (byte)FetchAllLandmarkRoomsResponseParameterCode.RoomName, landmarkRoom.RoomName },
                            { (byte)FetchAllLandmarkRoomsResponseParameterCode.HostPlayerID, landmarkRoom.HostPlayerID }
                        };
                        SendResponse(communicationInterface, LandmarkFetchDataCode.AllLandmarkRooms, result);
                    }
                    return true;
                }
                catch (InvalidCastException ex)
                {
                    LogService.Error("FetchAllLandmarkRooms Invalid Cast!");
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
