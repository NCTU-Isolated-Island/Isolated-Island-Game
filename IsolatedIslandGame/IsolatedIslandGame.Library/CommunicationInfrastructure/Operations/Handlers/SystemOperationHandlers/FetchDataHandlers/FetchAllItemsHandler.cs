using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataResponseParameters.System;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.SystemOperationHandlers.FetchDataHandlers
{
    class FetchAllItemsHandler : SystemFetchDataHandler
    {
        public FetchAllItemsHandler(SystemManager subject) : base(subject, 0)
        {
        }

        public override bool Handle(CommunicationInterface communicationInterface, SystemFetchDataCode fetchCode, Dictionary<byte, object> parameter)
        {
            if (base.Handle(communicationInterface, fetchCode, parameter))
            {
                try
                {
                    foreach (Item item in ItemManager.Instance.Items)
                    {
                        if (item is Material)
                        {
                            Material material = item as Material;
                            var result = new Dictionary<byte, object>
                            {
                                { (byte)FetchMaterialResponseParameterCode.ItemID, material.ItemID },
                                { (byte)FetchMaterialResponseParameterCode.ItemName, material.ItemName },
                                { (byte)FetchMaterialResponseParameterCode.Description, material.Description },
                                { (byte)FetchMaterialResponseParameterCode.MaterialID, material.MaterialID },
                                { (byte)FetchMaterialResponseParameterCode.Score, material.Score },
                                { (byte)FetchMaterialResponseParameterCode.GroupType, (byte)material.GroupType }
                            };
                            SendResponse(communicationInterface, SystemFetchDataCode.Item, result);
                        }
                        else
                        {
                            var result = new Dictionary<byte, object>
                            {
                                { (byte)FetchItemResponseParameterCode.ItemID, item.ItemID },
                                { (byte)FetchItemResponseParameterCode.ItemName, item.ItemName },
                                { (byte)FetchItemResponseParameterCode.Description, item.Description },
                            };
                            SendResponse(communicationInterface, SystemFetchDataCode.Item, result);
                        }
                    }
                    return true;
                }
                catch (InvalidCastException ex)
                {
                    LogService.ErrorFormat("FetchAllItems Invalid Cast!");
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
