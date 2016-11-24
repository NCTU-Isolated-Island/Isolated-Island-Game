using IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers;
using IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.UserOperationHandlers;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.OperationParameters.User;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Managers
{
    public class UserOperationManager
    {
        private readonly Dictionary<UserOperationCode, OperationHandler<User, UserOperationCode>> operationTable;
        protected readonly User user;
        public UserFetchDataResolver FetchDataResolver { get; protected set; }

        public UserOperationManager(User user)
        {
            this.user = user;
            FetchDataResolver = new UserFetchDataResolver(user);
            operationTable = new Dictionary<UserOperationCode, OperationHandler<User, UserOperationCode>>
            {
                { UserOperationCode.FetchData, FetchDataResolver },
                { UserOperationCode.PlayerOperation, new PlayerOperationResolver(user) },
            };
        }

        public void Operate(UserOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (operationTable.ContainsKey(operationCode))
            {
                if (!operationTable[operationCode].Handle(operationCode, parameters))
                {
                    LogService.ErrorFormat("User Operation Error: {0} from Identity: {1}", operationCode, user.IdentityInformation);
                }
            }
            else
            {
                LogService.ErrorFormat("Unknow User Operation:{0} from Identity: {1}", operationCode, user.IdentityInformation);
            }
        }

        public void SendOperation(UserOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            user.UserCommunicationInterface.SendOperation(operationCode, parameters);
        }
        public void SendAnswerOperation(Player player, PlayerOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            Dictionary<byte, object> operationParameters = new Dictionary<byte, object>
            {
                { (byte)PlayerOperationParameterCode.PlayerID, player.PlayerID },
                { (byte)PlayerOperationParameterCode.OperationCode, operationCode },
                { (byte)PlayerOperationParameterCode.Parameters, parameters }
            };
            SendOperation(UserOperationCode.PlayerOperation, operationParameters);
        }

    }
}
