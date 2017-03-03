using IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers;
using IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.SystemOperationHandlers;
using IsolatedIslandGame.Library.Landmarks;
using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataParameters;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.OperationParameters.System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Managers
{
    public class SystemOperationManager
    {
        private readonly Dictionary<SystemOperationCode, SystemOperationHandler> operationTable;
        protected readonly SystemManager systemManager;
        public SystemFetchDataResolver FetchDataResolver { get; protected set; }

        internal SystemOperationManager(SystemManager systemManager)
        {
            this.systemManager = systemManager;
            FetchDataResolver = new SystemFetchDataResolver(systemManager);
            operationTable = new Dictionary<SystemOperationCode, SystemOperationHandler>
            {
                { SystemOperationCode.FetchData, FetchDataResolver },
                { SystemOperationCode.LandmarkOperation, new LandmarkOperationResolver(systemManager) },
                { SystemOperationCode.AssignQuestToAllPlayer, new AssignQuestToAllPlayerHandler(systemManager) },
            };
        }

        internal void Operate(CommunicationInterface communicationInterface, SystemOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (operationTable.ContainsKey(operationCode))
            {
                if (!operationTable[operationCode].Handle(communicationInterface, operationCode, parameters))
                {
                    LogService.ErrorFormat("System Operation Error: {0} from Identity: {1}", operationCode, systemManager.IdentityInformation);
                }
            }
            else
            {
                LogService.ErrorFormat("Unknow System Operation:{0} from Identity: {1}", operationCode, systemManager.IdentityInformation);
            }
        }

        internal void SendOperation(SystemOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            UserManager.Instance.User.OperationManager.SendSystemOperation(operationCode, parameters);
        }

        internal void SendFetchDataOperation(SystemFetchDataCode fetchCode, Dictionary<byte, object> parameters)
        {
            Dictionary<byte, object> fetchDataParameters = new Dictionary<byte, object>
            {
                { (byte)FetchDataParameterCode.FetchDataCode, (byte)fetchCode },
                { (byte)FetchDataParameterCode.Parameters, parameters }
            };
            SendOperation(SystemOperationCode.FetchData, fetchDataParameters);
        }
        public void SendLandmarkOperation(Landmark landmark, LandmarkOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            Dictionary<byte, object> operationParameters = new Dictionary<byte, object>
            {
                { (byte)LandmarkOperationParameterCode.LandmarkID, landmark.LandmarkID },
                { (byte)LandmarkOperationParameterCode.OperationCode, operationCode },
                { (byte)LandmarkOperationParameterCode.Parameters, parameters }
            };
            SendOperation(SystemOperationCode.LandmarkOperation, operationParameters);
        }
        public void AssignQuestToAllPlayer(int questID, string administratorPassword)
        {
            Dictionary<byte, object> operationParameters = new Dictionary<byte, object>
            {
                { (byte)AssignQuestToAllPlayerParameterCode.QuestID, questID },
                { (byte)AssignQuestToAllPlayerParameterCode.AdministratorPassword, administratorPassword }
            };
            SendOperation(SystemOperationCode.AssignQuestToAllPlayer, operationParameters);
        }
    }
}
