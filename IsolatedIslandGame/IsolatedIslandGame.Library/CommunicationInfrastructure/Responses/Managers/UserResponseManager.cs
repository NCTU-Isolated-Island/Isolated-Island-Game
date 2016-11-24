using IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers;
using IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers.UserResponseHandlers;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Managers
{
    public class UserResponseManager
    {
        protected readonly Dictionary<UserOperationCode, ResponseHandler<User, UserOperationCode>> operationTable;
        protected readonly User user;

        public UserResponseManager(User user)
        {
            this.user = user;
            operationTable = new Dictionary<UserOperationCode, ResponseHandler<User, UserOperationCode>>
            {
                { UserOperationCode.FetchData, new FetchDataResponseResolver<User, UserOperationCode, UserFetchDataCode>(user) },
                { UserOperationCode.PlayerOperation, new PlayerOperationResponseResolver(user) },
            };
        }

        public void Operate(UserOperationCode operationCode, ErrorCode returnCode, string debugMessage, Dictionary<byte, object> parameters)
        {
            if (operationTable.ContainsKey(operationCode))
            {
                if (!operationTable[operationCode].Handle(operationCode, returnCode, debugMessage, parameters))
                {
                    LogService.ErrorFormat("User Response Error: {0} from Identity: {1}", operationCode, user.IdentityInformation);
                }
            }
            else
            {
                LogService.ErrorFormat("Unknow User Response:{0} from Identity: {1}", operationCode, user.IdentityInformation);
            }
        }

        public void SendResponse(UserOperationCode operationCode, ErrorCode errorCode, string debugMessage, Dictionary<byte, object> parameters)
        {
            user.UserCommunicationInterface.SendResponse(operationCode, errorCode, debugMessage, parameters);
        }
    }
}
