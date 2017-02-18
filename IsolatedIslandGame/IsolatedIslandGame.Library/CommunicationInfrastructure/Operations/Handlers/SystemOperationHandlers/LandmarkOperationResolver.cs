using IsolatedIslandGame.Library.Landmarks;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.OperationParameters.System;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.SystemOperationHandlers
{
    class LandmarkOperationResolver : SystemOperationHandler
    {
        internal LandmarkOperationResolver(SystemManager subject) : base(subject, 3)
        {
        }

        internal override bool Handle(CommunicationInterface communicationInterface, SystemOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(communicationInterface, operationCode, parameters))
            {
                try
                {
                    int landmarkID = (int)parameters[(byte)LandmarkOperationParameterCode.LandmarkID];
                    LandmarkOperationCode resolvedOperationCode = (LandmarkOperationCode)parameters[(byte)LandmarkOperationParameterCode.OperationCode];
                    Dictionary<byte, object> resolvedParameters = (Dictionary<byte, object>)parameters[(byte)LandmarkOperationParameterCode.Parameters];

                    Landmark landmark;
                    if(LandmarkManager.Instance.FindLandmark(landmarkID, out landmark))
                    {
                        landmark.OperationManager.Operate(communicationInterface, resolvedOperationCode, resolvedParameters);
                        return true;
                    }
                    else
                    {
                        LogService.ErrorFormat("LandmarkOperation Error Landmark ID: {0} Not in Identity: {1}", landmarkID, subject.IdentityInformation);
                        return false;
                    }
                }
                catch (InvalidCastException ex)
                {
                    LogService.ErrorFormat("LandmarkOperation Parameter Cast Error");
                    LogService.ErrorFormat(ex.Message);
                    LogService.ErrorFormat(ex.StackTrace);
                    return false;
                }
                catch (Exception ex)
                {
                    LogService.ErrorFormat(ex.Message);
                    LogService.ErrorFormat(ex.StackTrace);
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
