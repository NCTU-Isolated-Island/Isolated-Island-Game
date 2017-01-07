using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataResponseParameters;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.UserOperationHandlers
{
    internal abstract class UserFetchDataHandler : FetchDataHandler<User, UserFetchDataCode>
    {
        public UserFetchDataHandler(User subject, int correctParameterCount) : base(subject, correctParameterCount)
        {
        }

        public override void SendError(UserFetchDataCode fetchCode, ErrorCode errorCode, string debugMessage)
        {
            Dictionary<byte, object> eventData = new Dictionary<byte, object>
            {
                { (byte)FetchDataResponseParameterCode.FetchCode, (byte)fetchCode },
                { (byte)FetchDataResponseParameterCode.ReturnCode, (short)errorCode },
                { (byte)FetchDataResponseParameterCode.DebugMessage, debugMessage },
                { (byte)FetchDataResponseParameterCode.Parameters, new Dictionary<byte, object>() }
            };
            LogService.ErrorFormat("Error On {0} Fetch Operation: {1}, ErrorCode:{2}, Debug Message: {3}", subject.GetType(), fetchCode, errorCode, debugMessage);
            subject.ResponseManager.SendResponse(UserOperationCode.FetchData, ErrorCode.NoError, null, eventData);
        }

        public override void SendResponse(UserFetchDataCode fetchCode, Dictionary<byte, object> parameters)
        {
            Dictionary<byte, object> eventData = new Dictionary<byte, object>
            {
                { (byte)FetchDataResponseParameterCode.FetchCode, (byte)fetchCode },
                { (byte)FetchDataResponseParameterCode.ReturnCode, (short)ErrorCode.NoError },
                { (byte)FetchDataResponseParameterCode.DebugMessage, null },
                { (byte)FetchDataResponseParameterCode.Parameters, parameters }
            };
            subject.ResponseManager.SendResponse(UserOperationCode.FetchData, ErrorCode.NoError, null, eventData);
        }
    }
}
