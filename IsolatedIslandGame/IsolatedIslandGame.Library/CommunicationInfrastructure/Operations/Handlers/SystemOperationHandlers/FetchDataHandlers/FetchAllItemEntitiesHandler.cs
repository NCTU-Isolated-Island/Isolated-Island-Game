using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataResponseParameters.System;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.SystemOperationHandlers.FetchDataHandlers
{
    class FetchAllItemEntitiesHandler : SystemFetchDataHandler
    {
        public FetchAllItemEntitiesHandler(SystemManager subject) : base(subject, 0)
        {
        }

        public override bool Handle(CommunicationInterface communicationInterface, SystemFetchDataCode fetchCode, Dictionary<byte, object> parameter)
        {
            if (base.Handle(communicationInterface, fetchCode, parameter))
            {
                try
                {
                    foreach (var itemEntity in ItemEntityManager.Instance.ItemEntities)
                    {
                        var result = new Dictionary<byte, object>
                        {
                            { (byte)FetchAllItemEntitiesResponseParameterCode.ItemEntityID, itemEntity.ItemEntityID },
                            { (byte)FetchAllItemEntitiesResponseParameterCode.ItemID, itemEntity.ItemID },
                            { (byte)FetchAllItemEntitiesResponseParameterCode.PositionX, itemEntity.PositionX },
                            { (byte)FetchAllItemEntitiesResponseParameterCode.PositionZ, itemEntity.PositionZ }
                        };
                        SendResponse(communicationInterface, fetchCode, result);
                    }
                    return true;
                }
                catch (InvalidCastException ex)
                {
                    LogService.ErrorFormat("FetchAllItemEntities Invalid Cast!");
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
