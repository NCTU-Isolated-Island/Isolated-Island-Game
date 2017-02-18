using IsolatedIslandGame.Library.Landmarks;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataResponseParameters.Landmark;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers.LandmarkResponseHandlers.FetchDataResponseHandlers
{
    class FetchAllLandmarkRoomsResponseHandler : FetchDataResponseHandler<Landmark, LandmarkFetchDataCode>
    {
        public FetchAllLandmarkRoomsResponseHandler(Landmark subject) : base(subject)
        {
        }

        public override bool CheckError(Dictionary<byte, object> parameters, ErrorCode returnCode, string debugMessage)
        {
            switch (returnCode)
            {
                case ErrorCode.NoError:
                    {
                        if (parameters.Count != 3)
                        {
                            LogService.ErrorFormat(string.Format("FetchAllLandmarkRoomsResponse Parameter Error, Parameter Count: {0}", parameters.Count));
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                default:
                    {
                        LogService.ErrorFormat("FetchAllLandmarkRoomsResponse Error DebugMessage: {0}", debugMessage);
                        return false;
                    }
            }
        }

        public override bool Handle(LandmarkFetchDataCode fetchCode, ErrorCode returnCode, string fetchDebugMessage, Dictionary<byte, object> parameters)
        {
            if (base.Handle(fetchCode, returnCode, fetchDebugMessage, parameters))
            {
                try
                {
                    int landmarkRoomID = (int)parameters[(byte)FetchAllLandmarkRoomsResponseParameterCode.LandmarkRoomID];
                    string roomName = (string)parameters[(byte)FetchAllLandmarkRoomsResponseParameterCode.RoomName];
                    int hostPlayerID = (int)parameters[(byte)FetchAllLandmarkRoomsResponseParameterCode.HostPlayerID];

                    subject.LoadRoom(new LandmarkRoom(landmarkRoomID, roomName, hostPlayerID));
                    return true;
                }
                catch (InvalidCastException ex)
                {
                    LogService.Error("FetchAllLandmarkRoomsResponse Parameter Cast Error");
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
