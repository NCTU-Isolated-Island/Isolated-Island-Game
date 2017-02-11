using IsolatedIslandGame.Protocol.Communication.SyncDataCodes;
using IsolatedIslandGame.Protocol.Communication.SyncDataParameters.Player;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.PlayerEventHandlers.SyncDataHandlers
{
    class SyncNextDrawMaterialTimeUpdatedHandler : SyncDataHandler<Player, PlayerSyncDataCode>
    {
        public SyncNextDrawMaterialTimeUpdatedHandler(Player subject) : base(subject, 1)
        {
        }
        internal override bool Handle(PlayerSyncDataCode syncCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(syncCode, parameters))
            {
                try
                {
                    DateTime nextDrawMaterialTime = DateTime.FromBinary((long)parameters[(byte)SyncNextDrawMaterialTimeUpdatedParameterCode.NextDrawMaterialTime]);
                    subject.NextDrawMaterialTime = nextDrawMaterialTime;
                    return true;
                }
                catch (InvalidCastException ex)
                {
                    LogService.Error("SyncNextDrawMaterialTimeUpdated Parameter Cast Error");
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
