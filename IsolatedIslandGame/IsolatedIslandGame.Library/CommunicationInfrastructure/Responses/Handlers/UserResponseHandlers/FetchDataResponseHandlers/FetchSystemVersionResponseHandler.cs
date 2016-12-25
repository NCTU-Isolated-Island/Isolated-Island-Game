using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataResponseParameters.User;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers.UserResponseHandlers.FetchDataResponseHandlers
{
    class FetchSystemVersionResponseHandler : FetchDataResponseHandler<User, UserFetchDataCode>
    {
        public FetchSystemVersionResponseHandler(User subject) : base(subject)
        {
        }

        public override bool CheckError(Dictionary<byte, object> parameters, ErrorCode returnCode, string debugMessage)
        {
            switch (returnCode)
            {
                case ErrorCode.NoError:
                    {
                        if (parameters.Count != 2)
                        {
                            LogService.ErrorFormat(string.Format("Fetch SystemVersion Response Parameter Error, Parameter Count: {0}", parameters.Count));
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                default:
                    {
                        LogService.ErrorFormat("Fetch SystemVersion Response Error DebugMessage: {0}", debugMessage);
                        return false;
                    }
            }
        }

        public override bool Handle(UserFetchDataCode fetchCode, ErrorCode returnCode, string fetchDebugMessage, Dictionary<byte, object> parameters)
        {
            if (base.Handle(fetchCode, returnCode, fetchDebugMessage, parameters))
            {
                try
                {
                    string currentServerVersion = (string)parameters[(byte)FetchSystemVersionResponseParameterCode.CurrentServerVersion];
                    string currentClientVersion = (string)parameters[(byte)FetchSystemVersionResponseParameterCode.CurrentClientVersion];
                    subject.CommunicationInterface.CheckSystemVersion(currentServerVersion, currentClientVersion);

                    return true;
                }
                catch (InvalidCastException ex)
                {
                    LogService.Error("Fetch SystemVersion Response Parameter Cast Error");
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
