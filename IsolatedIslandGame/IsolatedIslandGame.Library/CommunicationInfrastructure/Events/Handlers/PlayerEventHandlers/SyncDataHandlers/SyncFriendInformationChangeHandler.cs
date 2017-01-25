using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.SyncDataCodes;
using IsolatedIslandGame.Protocol.Communication.SyncDataParameters.Player;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.PlayerEventHandlers.SyncDataHandlers
{
    class SyncFriendInformationChangeHandler : SyncDataHandler<Player, PlayerSyncDataCode>
    {
        public SyncFriendInformationChangeHandler(Player subject) : base(subject, 4)
        {
        }
        internal override bool Handle(PlayerSyncDataCode syncCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(syncCode, parameters))
            {
                try
                {
                    DataChangeType changeType = (DataChangeType)parameters[(byte)SyncFriendInformationChangeParameterCode.DataChangeType];
                    int friendPlayerID = (int)parameters[(byte)SyncFriendInformationChangeParameterCode.FriendPlayerID];
                    bool isInviter = (bool)parameters[(byte)SyncFriendInformationChangeParameterCode.IsInviter];
                    bool isConfirmed = (bool)parameters[(byte)SyncFriendInformationChangeParameterCode.IsConfirmed];

                    switch(changeType)
                    {
                        case DataChangeType.Add:
                        case DataChangeType.Update:
                            subject.AddFriend(new FriendInformation { friendPlayerID = friendPlayerID, isInviter = isInviter, isConfirmed = isConfirmed });
                            break;
                        case DataChangeType.Remove:
                            subject.RemoveFriend(friendPlayerID);
                            break;
                        default:
                            return false;
                    }
                    return true;
                }
                catch (InvalidCastException ex)
                {
                    LogService.Error("SyncPlayerInformation Parameter Cast Error");
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
