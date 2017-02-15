using IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers;
using IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.LandmarkOperationHandlers;
using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataParameters;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using System.Collections.Generic;
using IsolatedIslandGame.Library.Landmarks;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Managers
{
    public class LandmarkOperationManager
    {
        private readonly Dictionary<LandmarkOperationCode, LandmarkOperationHandler> operationTable;
        protected readonly Landmark landmark;
        public LandmarkFetchDataResolver FetchDataResolver { get; protected set; }

        internal LandmarkOperationManager(Landmark landmark)
        {
            this.landmark = landmark;
            FetchDataResolver = new LandmarkFetchDataResolver(landmark);
            operationTable = new Dictionary<LandmarkOperationCode, LandmarkOperationHandler>
            {
                { LandmarkOperationCode.FetchData, FetchDataResolver },
            };
        }

        internal void Operate(CommunicationInterface communicationInterface, LandmarkOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (operationTable.ContainsKey(operationCode))
            {
                if (!operationTable[operationCode].Handle(communicationInterface, operationCode, parameters))
                {
                    LogService.ErrorFormat("Landmark Operation Error: {0} from Identity: {1}", operationCode, landmark.IdentityInformation);
                }
            }
            else
            {
                LogService.ErrorFormat("Unknow Landmark Operation:{0} from Identity: {1}", operationCode, landmark.IdentityInformation);
            }
        }

        internal void SendOperation(LandmarkOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            SystemManager.Instance.OperationManager.SendLandmarkOperation(landmark, operationCode, parameters);
        }

        internal void SendFetchDataOperation(LandmarkFetchDataCode fetchCode, Dictionary<byte, object> parameters)
        {
            Dictionary<byte, object> fetchDataParameters = new Dictionary<byte, object>
            {
                { (byte)FetchDataParameterCode.FetchDataCode, (byte)fetchCode },
                { (byte)FetchDataParameterCode.Parameters, parameters }
            };
            SendOperation(LandmarkOperationCode.FetchData, fetchDataParameters);
        }
    }
}
