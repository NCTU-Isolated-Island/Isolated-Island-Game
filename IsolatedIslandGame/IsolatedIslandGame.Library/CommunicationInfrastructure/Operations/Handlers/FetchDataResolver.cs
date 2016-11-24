using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.FetchDataParameters;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers
{
    public abstract class FetchDataResolver<TSubject, TOperationCode, TFetchDataCode> : OperationHandler<TSubject, TOperationCode> where TSubject : IIdentityProvidable
    {
        protected readonly Dictionary<TFetchDataCode, FetchDataHandler<TSubject, TFetchDataCode>> fetchTable;

        public FetchDataResolver(TSubject subject) : base(subject, 2)
        {
            fetchTable = new Dictionary<TFetchDataCode, FetchDataHandler<TSubject, TFetchDataCode>>();
        }

        internal override bool Handle(TOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(operationCode, parameters))
            {
                string debugMessage;
                TFetchDataCode fetchCode = (TFetchDataCode)parameters[(byte)FetchDataParameterCode.FetchDataCode];
                Dictionary<byte, object> resolvedParameters = (Dictionary<byte, object>)parameters[(byte)FetchDataParameterCode.Parameters];
                if (fetchTable.ContainsKey(fetchCode))
                {
                    return fetchTable[fetchCode].Handle(fetchCode, resolvedParameters);
                }
                else
                {
                    debugMessage = string.Format("{0} Fetch Operation Not Exist Fetch Code: {1}", subject.GetType(), fetchCode);
                    SendError(operationCode, ErrorCode.InvalidOperation, debugMessage);
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
