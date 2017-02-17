using IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers;
using IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.LandmarkRoomOperationHandlers;
using IsolatedIslandGame.Library.Landmarks;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataParameters;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.OperationParameters.LandmarkRoom;
using System.Collections.Generic;

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
                { LandmarkRoomOperationCode.ExitRoom, new ExitRoomHandler(landmarkRoom) },
                { LandmarkRoomOperationCode.KickParticipant, new KickParticipantHandler(landmarkRoom) },
                { LandmarkRoomOperationCode.StartMultiplayerSynthesize, new StartMultiplayerSynthesizeHandler(landmarkRoom) },
                { LandmarkRoomOperationCode.ChangeMultiplayerSynthesizeItem, new ChangeMultiplayerSynthesizeItemHandler(landmarkRoom) },
                { LandmarkRoomOperationCode.ChangeMultiplayerSynthesizeCheckStatus, new ChangeMultiplayerSynthesizeCheckStatusHandler(landmarkRoom) },
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

        public void ExitRoom()
        {
            SendOperation(LandmarkRoomOperationCode.ExitRoom, new Dictionary<byte, object> { });
        }
        public void KickParticipant(int playerID)
        {
            Dictionary<byte, object> fetchDataParameters = new Dictionary<byte, object>
            {
                { (byte)KickParticipantParameterCode.ParticipantPlayerID, playerID },
            };
            SendOperation(LandmarkRoomOperationCode.KickParticipant, fetchDataParameters);
        }
        public void StartMultiplayerSynthesize()
        {
            SendOperation(LandmarkRoomOperationCode.StartMultiplayerSynthesize, new Dictionary<byte, object> { });
        }
        public void ChangeMultiplayerSynthesizeItem(int itemID, int itemCount)
        {
            Dictionary<byte, object> fetchDataParameters = new Dictionary<byte, object>
            {
                { (byte)ChangeMultiplayerSynthesizeItemParameterCode.ItemID, itemID },
                { (byte)ChangeMultiplayerSynthesizeItemParameterCode.ItemCount, itemCount }
            };
            SendOperation(LandmarkRoomOperationCode.ChangeMultiplayerSynthesizeItem, fetchDataParameters);
        }
        public void ChangeMultiplayerSynthesizeCheckStatus(int checkStatus)
        {
            Dictionary<byte, object> fetchDataParameters = new Dictionary<byte, object>
            {
                { (byte)ChangeMultiplayerSynthesizeCheckStatusParameterCode.CheckStatus, checkStatus },
            };
            SendOperation(LandmarkRoomOperationCode.ChangeMultiplayerSynthesizeCheckStatus, fetchDataParameters);
        }
    }
}
