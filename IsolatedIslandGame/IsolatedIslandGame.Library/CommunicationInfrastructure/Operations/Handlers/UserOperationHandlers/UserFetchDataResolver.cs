using IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.UserOperationHandlers.FetchDataHandlers;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataParameters;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.UserOperationHandlers
{
    public class UserFetchDataResolver : FetchDataResolver<User, UserOperationCode, UserFetchDataCode>
    {
        public UserFetchDataResolver(User subject) : base(subject)
        {
            fetchTable.Add(UserFetchDataCode.SystemVersion, new FetchSystemVersionHandler(subject));
        }

        internal override void SendResponse(UserOperationCode operationCode, Dictionary<byte, object> parameter)
        {
            subject.ResponseManager.SendResponse(operationCode, ErrorCode.NoError, null, parameter);
        }
        internal override void SendError(UserOperationCode operationCode, ErrorCode errorCode, string debugMessage)
        {
            base.SendError(operationCode, errorCode, debugMessage);
            Dictionary<byte, object> parameters = new Dictionary<byte, object>();
            subject.ResponseManager.SendResponse(operationCode, errorCode, debugMessage, parameters);
        }
        internal void SendOperation(UserFetchDataCode fetchCode, Dictionary<byte, object> parameters)
        {
            subject.OperationManager.SendFetchDataOperation(fetchCode, parameters);
        }

        public void FetchSystemVersion()
        {
            SendOperation(UserFetchDataCode.SystemVersion, new Dictionary<byte, object>());
        }
    }
}
