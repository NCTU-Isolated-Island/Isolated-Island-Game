using IsolatedIslandGame.Library.Landmarks;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataResponseParameters.System;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers.SystemResponseHandlers.FetchDataResponseHandlers
{
    class FetchAllLandmarksResponseHandler : FetchDataResponseHandler<SystemManager, SystemFetchDataCode>
    {
        public FetchAllLandmarksResponseHandler(SystemManager subject) : base(subject)
        {
        }

        public override bool CheckError(Dictionary<byte, object> parameters, ErrorCode returnCode, string debugMessage)
        {
            switch (returnCode)
            {
                case ErrorCode.NoError:
                    {
                        if (parameters.Count != 3)
                        {
                            LogService.ErrorFormat(string.Format("FetchAllLandmarksResponse Parameter Error, Parameter Count: {0}", parameters.Count));
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                default:
                    {
                        LogService.ErrorFormat("FetchAllLandmarksResponse Error DebugMessage: {0}", debugMessage);
                        return false;
                    }
            }
        }

        public override bool Handle(SystemFetchDataCode fetchCode, ErrorCode returnCode, string fetchDebugMessage, Dictionary<byte, object> parameters)
        {
            if (base.Handle(fetchCode, returnCode, fetchDebugMessage, parameters))
            {
                try
                {
                    int landmarkID = (int)parameters[(byte)FetchAllLandmarksResponseParameterCode.LandmarkID];
                    string landmarkName = (string)parameters[(byte)FetchAllLandmarksResponseParameterCode.LandmarkName];
                    string description = (string)parameters[(byte)FetchAllLandmarksResponseParameterCode.Description];

                    Landmark landmark = new Landmark(landmarkID, landmarkName, description);
                    LandmarkManager.Instance.LoadLandmark(landmark);
                    landmark.OperationManager.FetchDataResolver.FetchAllLandmarkRooms();
                    return true;
                }
                catch (InvalidCastException ex)
                {
                    LogService.Error("FetchAllLandmarksResponse Parameter Cast Error");
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
