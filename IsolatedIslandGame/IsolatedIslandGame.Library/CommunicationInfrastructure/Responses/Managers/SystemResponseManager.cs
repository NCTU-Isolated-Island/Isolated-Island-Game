using IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers;
using IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers.SystemResponseHandlers;
using IsolatedIslandGame.Library.Landmarks;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.ResponseParameters.User;
using IsolatedIslandGame.Protocol.Communication.ResponseParameters.System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Managers
{
    public class SystemResponseManager
    {
        protected readonly Dictionary<SystemOperationCode, ResponseHandler<SystemManager, SystemOperationCode>> operationTable;
        protected readonly SystemManager systemManager;

        public SystemResponseManager(SystemManager systemManager)
        {
            this.systemManager = systemManager;
            operationTable = new Dictionary<SystemOperationCode, ResponseHandler<SystemManager, SystemOperationCode>>
            {
                { SystemOperationCode.FetchData, new SystemFetchDataResponseResolver(systemManager) },
                { SystemOperationCode.LandmarkOperation, new LandmarkOperationResponseResolver(systemManager) },
            };
        }

        public void Operate(SystemOperationCode operationCode, ErrorCode returnCode, string debugMessage, Dictionary<byte, object> parameters)
        {
            if (operationTable.ContainsKey(operationCode))
            {
                if (!operationTable[operationCode].Handle(operationCode, returnCode, debugMessage, parameters))
                {
                    LogService.ErrorFormat("System Response Error: {0} from Identity: {1}", operationCode, systemManager.IdentityInformation);
                }
            }
            else
            {
                LogService.ErrorFormat("Unknow System Response:{0} from Identity: {1}", operationCode, systemManager.IdentityInformation);
            }
        }

        internal void SendResponse(CommunicationInterface communicationInterface, SystemOperationCode operationCode, ErrorCode errorCode, string debugMessage, Dictionary<byte, object> parameters)
        {
            Dictionary<byte, object> responseData = new Dictionary<byte, object>
            {
                { (byte)SystemResponseParameterCode.OperationCode, (byte)operationCode },
                { (byte)SystemResponseParameterCode.ReturnCode, (short)errorCode },
                { (byte)SystemResponseParameterCode.DebugMessage, debugMessage },
                { (byte)SystemResponseParameterCode.Parameters, parameters }
            };
            communicationInterface.SendResponse(UserOperationCode.SystemOperation, ErrorCode.NoError, null, responseData);
        }
        public void SendLandmarkResponse(CommunicationInterface communicationInterface, Landmark landmark, LandmarkOperationCode operationCode, ErrorCode errorCode, string debugMessage, Dictionary<byte, object> parameters)
        {
            Dictionary<byte, object> responseData = new Dictionary<byte, object>
            {
                { (byte)LandmarkResponseParameterCode.LandmarkID, landmark.LandmarkID },
                { (byte)LandmarkResponseParameterCode.OperationCode, (byte)operationCode },
                { (byte)LandmarkResponseParameterCode.ReturnCode, (short)errorCode },
                { (byte)LandmarkResponseParameterCode.DebugMessage, debugMessage },
                { (byte)LandmarkResponseParameterCode.Parameters, parameters }
            };
            SendResponse(communicationInterface, SystemOperationCode.LandmarkOperation, ErrorCode.NoError, null, responseData);
        }
    }
}
