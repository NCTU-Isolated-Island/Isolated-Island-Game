using IsolatedIslandGame.Library.Landmarks;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.SyncDataCodes;
using IsolatedIslandGame.Protocol.Communication.SyncDataParameters.Landmark;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.LandmarkEventHandlers.SyncDataHandlers
{
    class SyncLandmarkRoomChangeHandler : SyncDataHandler<Landmark, LandmarkSyncDataCode>
    {
        public SyncLandmarkRoomChangeHandler(Landmark subject) : base(subject, 4)
        {
        }
        internal override bool Handle(LandmarkSyncDataCode syncCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(syncCode, parameters))
            {
                try
                {
                    DataChangeType changeType = (DataChangeType)parameters[(byte)SyncLandmarkRoomChangeParameterCode.DataChangeType];
                    int landmarkRoomID = (int)parameters[(byte)SyncLandmarkRoomChangeParameterCode.LandmarkRoomID];
                    string roomName = (string)parameters[(byte)SyncLandmarkRoomChangeParameterCode.RoomName];
                    int hostPlayerID = (int)parameters[(byte)SyncLandmarkRoomChangeParameterCode.HostPlayerID];

                    LandmarkRoom room = new LandmarkRoom(landmarkRoomID, roomName, hostPlayerID);
                    switch (changeType)
                    {
                        case DataChangeType.Add:
                        case DataChangeType.Update:
                            subject.LoadRoom(room);
                            break;
                        case DataChangeType.Remove:
                            subject.RemoveRoom(landmarkRoomID);
                            break;
                        default:
                            LogService.FatalFormat("SyncLandmarkRoomChange undefined DataChangeType: {0}", changeType);
                            return false;
                    }
                    return true;
                }
                catch (InvalidCastException ex)
                {
                    LogService.Error("SyncLandmarkRoomChange Parameter Cast Error");
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
