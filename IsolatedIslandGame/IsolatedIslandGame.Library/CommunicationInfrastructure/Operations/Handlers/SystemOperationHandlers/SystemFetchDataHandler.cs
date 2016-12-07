using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataResponseParameters;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.SystemOperationHandlers
{
    internal abstract class SystemFetchDataHandler : FetchDataHandler<SystemManager, SystemFetchDataCode>
    {
        public SystemFetchDataHandler(SystemManager subject, int correctParameterCount) : base(subject, correctParameterCount)
        {
        }

        public override void SendError(SystemFetchDataCode fetchCode, ErrorCode errorCode, string debugMessage)
        {
            Dictionary<byte, object> eventData = new Dictionary<byte, object>
            {
                { (byte)FetchDataResponseParameterCode.FetchCode, (byte)fetchCode },
                { (byte)FetchDataResponseParameterCode.ReturnCode, (short)errorCode },
                { (byte)FetchDataResponseParameterCode.DebugMessage, debugMessage },
                { (byte)FetchDataResponseParameterCode.Parameters, new Dictionary<byte, object>() }
            };
            LogService.ErrorFormat("Error On {0} Fetch Operation: {1}, ErrorCode:{2}, Debug Message: {3}", subject.GetType(), fetchCode, errorCode, debugMessage);
            subject.ResponseManager.SendResponse(SystemOperationCode.FetchData, ErrorCode.NoError, null, eventData);
        }

        public override void SendResponse(SystemFetchDataCode fetchCode, Dictionary<byte, object> parameters)
        {
            Dictionary<byte, object> eventData = new Dictionary<byte, object>
            {
                { (byte)FetchDataResponseParameterCode.FetchCode, (byte)fetchCode },
                { (byte)FetchDataResponseParameterCode.ReturnCode, (short)ErrorCode.NoError },
                { (byte)FetchDataResponseParameterCode.DebugMessage, null },
                { (byte)FetchDataResponseParameterCode.Parameters, parameters }
            };
            subject.ResponseManager.SendResponse(SystemOperationCode.FetchData, ErrorCode.NoError, null, eventData);
        }
    }
}
