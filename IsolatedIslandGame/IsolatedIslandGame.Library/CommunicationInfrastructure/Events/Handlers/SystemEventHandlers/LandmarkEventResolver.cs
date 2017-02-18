using IsolatedIslandGame.Library.Landmarks;
using IsolatedIslandGame.Protocol.Communication.EventCodes;
using IsolatedIslandGame.Protocol.Communication.EventParameters.System;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.SystemEventHandlers
{
    class LandmarkEventResolver : EventHandler<SystemManager, SystemEventCode>
    {
        public LandmarkEventResolver(SystemManager subject) : base(subject, 3) { }
        internal override bool Handle(SystemEventCode eventCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(eventCode, parameters))
            {
                try
                {
                    int landmarkID = (int)parameters[(byte)LandmarkEventParameterCode.LandmarkID];
                    LandmarkEventCode resolvedEventCode = (LandmarkEventCode)parameters[(byte)LandmarkEventParameterCode.EventCode];
                    Dictionary<byte, object> resolvedParameters = (Dictionary<byte, object>)parameters[(byte)LandmarkEventParameterCode.Parameters];

                    Landmark landmark;
                    if (LandmarkManager.Instance.FindLandmark(landmarkID, out landmark))
                    {
                        landmark.EventManager.Operate(resolvedEventCode, resolvedParameters);
                        return true;
                    }
                    else
                    {
                        LogService.Error($"LandmarkEvent Error LandmarkID: {landmarkID}");
                        return false;
                    }
                }
                catch (InvalidCastException ex)
                {
                    LogService.ErrorFormat("LandmarkEvent Parameter Cast Error");
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
