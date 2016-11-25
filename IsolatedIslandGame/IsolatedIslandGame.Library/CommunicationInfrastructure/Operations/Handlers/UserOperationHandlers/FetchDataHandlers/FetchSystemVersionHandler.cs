using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataResponseParameters.User;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.UserOperationHandlers.FetchDataHandlers
{
    class FetchSystemVersionHandler : UserFetchDataHandler
    {
        public FetchSystemVersionHandler(User subject) : base(subject, 0)
        {
        }

        public override bool Handle(UserFetchDataCode fetchCode, Dictionary<byte, object> parameter)
        {
            if (base.Handle(fetchCode, parameter))
            {
                try
                {
                    string serverVersion, clientVersion;
                    subject.UserCommunicationInterface.GetSystemVersion(out serverVersion, out clientVersion);
                    var result = new Dictionary<byte, object>
                    {
                        { (byte)FetchSystemVersionResponseParameterCode.CurrentServerVersion, serverVersion },
                        { (byte)FetchSystemVersionResponseParameterCode.CurrentClientVersion, clientVersion }
                    };
                    SendResponse(fetchCode, result);
                    return true;
                }
                catch (InvalidCastException ex)
                {
                    LogService.ErrorFormat("Fetch System Version Invalid Cast!");
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
