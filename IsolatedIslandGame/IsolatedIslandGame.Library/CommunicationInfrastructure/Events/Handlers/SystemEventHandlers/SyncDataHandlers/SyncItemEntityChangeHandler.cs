using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.SyncDataCodes;
using IsolatedIslandGame.Protocol.Communication.SyncDataParameters.System;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.SystemEventHandlers.SyncDataHandlers
{
    class SyncItemEntityChangeHandler : SyncDataHandler<SystemManager, SystemSyncDataCode>
    {
        public SyncItemEntityChangeHandler(SystemManager subject) : base(subject, 5)
        {
        }
        internal override bool Handle(SystemSyncDataCode syncCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(syncCode, parameters))
            {
                try
                {
                    DataChangeType changeType = (DataChangeType)parameters[(byte)SyncItemEntityChangeParameterCode.DataChangeType];
                    int itemEntityID = (int)parameters[(byte)SyncItemEntityChangeParameterCode.ItemEntityID];
                    int itemID = (int)parameters[(byte)SyncItemEntityChangeParameterCode.ItemID];
                    float positionX = (float)parameters[(byte)SyncItemEntityChangeParameterCode.PositionX];
                    float positionZ = (float)parameters[(byte)SyncItemEntityChangeParameterCode.PositionZ];

                    switch (changeType)
                    {
                        case DataChangeType.Add:
                            ItemEntityManager.Instance.AddItemEntity(new ItemEntity(itemEntityID, itemID, positionX, positionZ));
                            break;
                        case DataChangeType.Remove:
                            ItemEntityManager.Instance.RemoveItemEntity(itemEntityID);
                            break;
                        default:
                            LogService.FatalFormat("SyncItemEntityChange undefined DataChangeType: {0}", changeType);
                            return false;
                    }
                    return true;
                }
                catch (InvalidCastException ex)
                {
                    LogService.Error("SyncItemEntityChange Parameter Cast Error");
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
