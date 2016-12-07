using IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers;
using IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.SystemOperationHandlers;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Managers
{
    public class SystemOperationManager
    {
        private readonly Dictionary<SystemOperationCode, OperationHandler<SystemManager, SystemOperationCode>> operationTable;
        protected readonly SystemManager systemManager;
        public SystemFetchDataResolver FetchDataResolver { get; protected set; }

        internal SystemOperationManager(SystemManager systemManager)
        {
            this.systemManager = systemManager;
            FetchDataResolver = new SystemFetchDataResolver(systemManager);
            operationTable = new Dictionary<SystemOperationCode, OperationHandler<SystemManager, SystemOperationCode>>
            {
                { SystemOperationCode.FetchData, FetchDataResolver },
            };
        }

        internal void Operate(SystemOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (operationTable.ContainsKey(operationCode))
            {
                if (!operationTable[operationCode].Handle(operationCode, parameters))
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
            systemManager.User.OperationManager.SendSystemOperation(operationCode, parameters);
        }
    }
}
