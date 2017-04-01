using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataResponseParameters.System;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.SystemOperationHandlers.FetchDataHandlers
{
    class FetchClientFunctionCheckTableHandler : SystemFetchDataHandler
    {
        public FetchClientFunctionCheckTableHandler(SystemManager subject) : base(subject, 0)
        {
        }

        public override bool Handle(CommunicationInterface communicationInterface, SystemFetchDataCode fetchCode, Dictionary<byte, object> parameter)
        {
            if (base.Handle(communicationInterface, fetchCode, parameter))
            {
                try
                {
                    var result = new Dictionary<byte, object>
                    {
                        { (byte)FetchClientFunctionCheckTableResponseParameterCode.CheckTable, ClientFunctionCheckTable.Instance.FunctionCheckTableCopy }
                    };
                    SendResponse(communicationInterface, fetchCode, result);
                    return true;
                }
                catch (InvalidCastException ex)
                {
                    LogService.ErrorFormat("FetchClientFunctionCheckTable Invalid Cast!");
                    LogService.Error(ex.Message);
                    LogService.Error(ex.StackTrace);
                    return false;
                }
                catch (Exception ex)
                {
                    LogService.Error(ex.Message);
                    LogService.Error(ex.StackTrace);
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
