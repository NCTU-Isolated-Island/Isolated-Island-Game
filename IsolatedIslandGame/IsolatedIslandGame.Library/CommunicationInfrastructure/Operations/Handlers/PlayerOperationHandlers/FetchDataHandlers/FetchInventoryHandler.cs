using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataResponseParameters.Player;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.PlayerOperationHandlers.FetchDataHandlers
{
    class FetchInventoryHandler : PlayerFetchDataHandler
    {
        public FetchInventoryHandler(Player subject) : base(subject, 0)
        {
        }

        public override bool Handle(PlayerFetchDataCode fetchCode, Dictionary<byte, object> parameter)
        {
            if (base.Handle(fetchCode, parameter))
            {
                try
                {
                    Inventory inventory = subject.Inventory;
                    var result = new Dictionary<byte, object>
                    {
                        { (byte)FetchInventoryResponseParameterCode.InventoryID, inventory.InventoryID },
                        { (byte)FetchInventoryResponseParameterCode.Capacity, inventory.Capacity },
                    };
                    SendResponse(fetchCode, result);
                    return true;
                }
                catch (InvalidCastException ex)
                {
                    LogService.ErrorFormat("Fetch System Version Invalid Cast!");
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
