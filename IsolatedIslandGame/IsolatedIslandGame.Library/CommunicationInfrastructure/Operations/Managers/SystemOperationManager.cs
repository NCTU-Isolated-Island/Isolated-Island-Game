using IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers;
using IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.SystemOperationHandlers;
using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataParameters;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
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
    }
}
