using IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers;
using IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers.LandmarkRoomResponseHandlers;
using IsolatedIslandGame.Library.Landmarks;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Managers
{
    public class LandmarkRoomResponseManager
    {
        protected readonly Dictionary<LandmarkRoomOperationCode, ResponseHandler<LandmarkRoom, LandmarkRoomOperationCode>> operationTable;
        protected readonly LandmarkRoom landmarkRoom;

        public LandmarkRoomResponseManager(LandmarkRoom landmarkRoom)
        {
            this.landmarkRoom = landmarkRoom;
            operationTable = new Dictionary<LandmarkRoomOperationCode, ResponseHandler<LandmarkRoom, LandmarkRoomOperationCode>>
            {
                { LandmarkRoomOperationCode.FetchData, new LandmarkRoomFetchDataResponseResolver(landmarkRoom) },
            };
        }

        public void Operate(LandmarkRoomOperationCode operationCode, ErrorCode returnCode, string debugMessage, Dictionary<byte, object> parameters)
        {
            if (operationTable.ContainsKey(operationCode))
            {
                if (!operationTable[operationCode].Handle(operationCode, returnCode, debugMessage, parameters))
                {
                    LogService.ErrorFormat("LandmarkRoom Response Error: {0} from Identity: {1}", operationCode, landmarkRoom.IdentityInformation);
                }
            }
            else
            {
                LogService.ErrorFormat("Unknow LandmarkRoom Response:{0} from Identity: {1}", operationCode, landmarkRoom.IdentityInformation);
            }
        }

        internal void SendResponse(CommunicationInterface communicationInterface, LandmarkRoomOperationCode operationCode, ErrorCode errorCode, string debugMessage, Dictionary<byte, object> parameters)
        {
            landmarkRoom.Landmark.ResponseManager.SendLandmarkRoomResponse(communicationInterface, landmarkRoom, operationCode, errorCode, debugMessage, parameters);
        }
    }
}
