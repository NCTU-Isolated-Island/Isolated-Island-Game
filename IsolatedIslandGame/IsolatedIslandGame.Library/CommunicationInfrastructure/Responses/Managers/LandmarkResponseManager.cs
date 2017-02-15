using IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers;
using IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers.LandmarkResponseHandlers;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using System.Collections.Generic;
using IsolatedIslandGame.Library.Landmarks;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Managers
{
    public class LandmarkResponseManager
    {
        protected readonly Dictionary<LandmarkOperationCode, ResponseHandler<Landmark, LandmarkOperationCode>> operationTable;
        protected readonly Landmark landmark;

        public LandmarkResponseManager(Landmark landmark)
        {
            this.landmark = landmark;
            operationTable = new Dictionary<LandmarkOperationCode, ResponseHandler<Landmark, LandmarkOperationCode>>
            {
                { LandmarkOperationCode.FetchData, new LandmarkFetchDataResponseResolver(landmark) },
            };
        }

        public void Operate(LandmarkOperationCode operationCode, ErrorCode returnCode, string debugMessage, Dictionary<byte, object> parameters)
        {
            if (operationTable.ContainsKey(operationCode))
            {
                if (!operationTable[operationCode].Handle(operationCode, returnCode, debugMessage, parameters))
                {
                    LogService.ErrorFormat("Landmark Response Error: {0} from Identity: {1}", operationCode, landmark.IdentityInformation);
                }
            }
            else
            {
                LogService.ErrorFormat("Unknow Landmark Response:{0} from Identity: {1}", operationCode, landmark.IdentityInformation);
            }
        }

        internal void SendResponse(CommunicationInterface communicationInterface, LandmarkOperationCode operationCode, ErrorCode errorCode, string debugMessage, Dictionary<byte, object> parameters)
        {
            SystemManager.Instance.ResponseManager.SendLandmarkResponse(communicationInterface, landmark, operationCode, errorCode, debugMessage, parameters);
        }
    }
}
