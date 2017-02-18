using IsolatedIslandGame.Library.Landmarks;
using IsolatedIslandGame.Protocol.Communication.EventCodes;
using IsolatedIslandGame.Protocol.Communication.EventParameters.Landmark;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.LandmarkEventHandlers
{
    class LandmarkRoomEventResolver : EventHandler<Landmark, LandmarkEventCode>
    {
        public LandmarkRoomEventResolver(Landmark subject) : base(subject, 3) { }
        internal override bool Handle(LandmarkEventCode eventCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(eventCode, parameters))
            {
                try
                {
                    int landmarkRoomID = (int)parameters[(byte)LandmarkRoomEventParameterCode.LandmarkRoomID];
                    LandmarkRoomEventCode resolvedEventCode = (LandmarkRoomEventCode)parameters[(byte)LandmarkRoomEventParameterCode.EventCode];
                    Dictionary<byte, object> resolvedParameters = (Dictionary<byte, object>)parameters[(byte)LandmarkRoomEventParameterCode.Parameters];

                    LandmarkRoom room;
                    if (subject.FindRoom(landmarkRoomID, out room))
                    {
                        room.EventManager.Operate(resolvedEventCode, resolvedParameters);
                        return true;
                    }
                    else
                    {
                        LogService.Error($"LandmarkRoomEvent Error LandmarkRoomID: {landmarkRoomID}");
                        return false;
                    }
                }
                catch (InvalidCastException ex)
                {
                    LogService.ErrorFormat("LandmarkRoomEvent Parameter Cast Error");
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
