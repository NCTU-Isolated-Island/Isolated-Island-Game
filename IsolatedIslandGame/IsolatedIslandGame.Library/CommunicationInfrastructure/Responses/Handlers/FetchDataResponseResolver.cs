using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.FetchDataResponseParameters;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers
{
    public class FetchDataResponseResolver<TSubject, TOperationCode, TFetchDataCode> : ResponseHandler<TSubject, TOperationCode> where TSubject : IIdentityProvidable
    {
        protected readonly Dictionary<TFetchDataCode, FetchDataResponseHandler<TSubject, TFetchDataCode>> fetchResponseTable;

        public FetchDataResponseResolver(TSubject subject) : base(subject)
        {
            fetchResponseTable = new Dictionary<TFetchDataCode, FetchDataResponseHandler<TSubject, TFetchDataCode>>();
        }

        internal override bool CheckError(Dictionary<byte, object> parameters, ErrorCode returnCode, string debugMessage)
        {
            if (returnCode == ErrorCode.NoError)
            {
                if (parameters.Count != 4)
                {
                    LogService.ErrorFormat("{0} Fetch Data Response Parameter Error Parameter Count: {1}", subject.GetType(), parameters.Count);
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                LogService.ErrorFormat("{0} Fetch Data Response Error ErrorCode: {1}, DebugMessage: {2}", subject.GetType(), returnCode, debugMessage);
                return false;
            }
        }

        internal override bool Handle(TOperationCode operationCode, ErrorCode returnCode, string debugMessage, Dictionary<byte, object> parameters)
        {
            if (base.Handle(operationCode, returnCode, debugMessage, parameters))
            {
                TFetchDataCode fetchCode = (TFetchDataCode)parameters[(byte)FetchDataResponseParameterCode.FetchCode];
                ErrorCode resolvedReturnCode = (ErrorCode)parameters[(byte)FetchDataResponseParameterCode.ReturnCode];
                string resolvedDebugMessage = (string)parameters[(byte)FetchDataResponseParameterCode.DebugMessage];
                Dictionary<byte, object> resolvedParameters = (Dictionary<byte, object>)parameters[(byte)FetchDataResponseParameterCode.Parameters];
                if (fetchResponseTable.ContainsKey(fetchCode))
                {
                    return fetchResponseTable[fetchCode].Handle(fetchCode, resolvedReturnCode, resolvedDebugMessage, resolvedParameters);
                }
                else
                {
                    LogService.ErrorFormat("{0} FetchData Response Not Exist Fetch Code: {1}", subject.GetType(), fetchCode);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
