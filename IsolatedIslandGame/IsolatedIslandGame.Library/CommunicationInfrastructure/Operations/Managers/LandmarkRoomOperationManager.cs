using IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers;
using IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.LandmarkRoomOperationHandlers;
using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataParameters;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using System.Collections.Generic;
using IsolatedIslandGame.Library.Landmarks;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Managers
{
    public class LandmarkRoomOperationManager
    {
        private readonly Dictionary<LandmarkRoomOperationCode, LandmarkRoomOperationHandler> operationTable;
        protected readonly LandmarkRoom landmarkRoom;
        public LandmarkRoomFetchDataResolver FetchDataResolver { get; protected set; }

        internal LandmarkRoomOperationManager(LandmarkRoom landmarkRoom)
        {
            this.landmarkRoom = landmarkRoom;
            FetchDataResolver = new LandmarkRoomFetchDataResolver(landmarkRoom);
            operationTable = new Dictionary<LandmarkRoomOperationCode, LandmarkRoomOperationHandler>
            {
                { LandmarkRoomOperationCode.FetchData, FetchDataResolver },
            };
        }

        internal void Operate(CommunicationInterface communicationInterface, LandmarkRoomOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (operationTable.ContainsKey(operationCode))
            {
                if (!operationTable[operationCode].Handle(communicationInterface, operationCode, parameters))
                {
                    LogService.ErrorFormat("LandmarkRoom Operation Error: {0} from Identity: {1}", operationCode, landmarkRoom.IdentityInformation);
                }
            }
            else
            {
                LogService.ErrorFormat("Unknow LandmarkRoom Operation:{0} from Identity: {1}", operationCode, landmarkRoom.IdentityInformation);
            }
        }

        internal void SendOperation(LandmarkRoomOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            landmarkRoom.Landmark.OperationManager.SendLandmarkRoomOperation(landmarkRoom, operationCode, parameters);
        }

        internal void SendFetchDataOperation(LandmarkRoomFetchDataCode fetchCode, Dictionary<byte, object> parameters)
        {
            Dictionary<byte, object> fetchDataParameters = new Dictionary<byte, object>
            {
                { (byte)FetchDataParameterCode.FetchDataCode, (byte)fetchCode },
                { (byte)FetchDataParameterCode.Parameters, parameters }
            };
            SendOperation(LandmarkRoomOperationCode.FetchData, fetchDataParameters);
        }
    }
}
