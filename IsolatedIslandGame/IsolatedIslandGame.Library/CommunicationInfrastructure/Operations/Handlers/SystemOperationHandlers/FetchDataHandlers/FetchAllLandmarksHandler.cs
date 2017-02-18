using IsolatedIslandGame.Library.Landmarks;
using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataResponseParameters.System;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.SystemOperationHandlers.FetchDataHandlers
{
    class FetchAllLandmarksHandler : SystemFetchDataHandler
    {
        public FetchAllLandmarksHandler(SystemManager subject) : base(subject, 0)
        {
        }

        public override bool Handle(CommunicationInterface communicationInterface, SystemFetchDataCode fetchCode, Dictionary<byte, object> parameter)
        {
            if (base.Handle(communicationInterface, fetchCode, parameter))
            {
                try
                {
                    foreach (Landmark landmark in LandmarkManager.Instance.Landmarks)
                    {
                        var result = new Dictionary<byte, object>
                        {
                            { (byte)FetchAllLandmarksResponseParameterCode.LandmarkID, landmark.LandmarkID },
                            { (byte)FetchAllLandmarksResponseParameterCode.LandmarkName, landmark.LandmarkName },
                            { (byte)FetchAllLandmarksResponseParameterCode.Description, landmark.Description }
                        };
                        SendResponse(communicationInterface, SystemFetchDataCode.Vessel, result);
                    }
                    return true;
                }
                catch (InvalidCastException ex)
                {
                    LogService.Error("FetchAllLandmarks Invalid Cast!");
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
