using IsolatedIslandGame.Protocol.Communication.SyncDataCodes;
using IsolatedIslandGame.Protocol.Communication.SyncDataParameters.System;
using IsolatedIslandGame.Protocol;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.SystemEventHandlers.SyncDataHandlers
{
    class SyncPlayerInformationHandler : SyncDataHandler<SystemManager, SystemSyncDataCode>
    {
        public SyncPlayerInformationHandler(SystemManager subject) : base(subject, 5)
        {
        }
        internal override bool Handle(SystemSyncDataCode syncCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(syncCode, parameters))
            {
                try
                {
                    int playerID = (int)parameters[(byte)SyncPlayerInformationParameterCode.PlayerID];
                    string nickname = (string)parameters[(byte)SyncPlayerInformationParameterCode.Nickname];
                    string signature = (string)parameters[(byte)SyncPlayerInformationParameterCode.Signature];
                    GroupType groupType = (GroupType)parameters[(byte)SyncPlayerInformationParameterCode.GroupType];
                    int vesselID = (int)parameters[(byte)SyncPlayerInformationParameterCode.VesselID];

                    PlayerInformationManager.Instance.AddPlayerInformation(new PlayerInformation
                    {
                        playerID = playerID,
                        nickname = nickname,
                        signature = signature,
                        groupType = groupType,
                        vesselID = vesselID
                    });
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
