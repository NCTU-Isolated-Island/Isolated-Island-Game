using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataResponseParameters.System;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers.SystemResponseHandlers.FetchDataResponseHandlers
{
    class FetchItemResponseHandler : FetchDataResponseHandler<SystemManager, SystemFetchDataCode>
    {
        public FetchItemResponseHandler(SystemManager subject) : base(subject)
        {
        }

        public override bool CheckError(Dictionary<byte, object> parameters, ErrorCode returnCode, string debugMessage)
        {
            switch (returnCode)
            {
                case ErrorCode.NoError:
                    {
                        if (parameters.Count != 3 && parameters.Count != 5)
                        {
                            LogService.ErrorFormat(string.Format("FetchItemResponse Parameter Error, Parameter Count: {0}", parameters.Count));
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                default:
                    {
                        LogService.ErrorFormat("FetchItemResponse Error DebugMessage: {0}", debugMessage);
                        return false;
                    }
            }
        }

        public override bool Handle(SystemFetchDataCode fetchCode, ErrorCode returnCode, string fetchDebugMessage, Dictionary<byte, object> parameters)
        {
            if (base.Handle(fetchCode, returnCode, fetchDebugMessage, parameters))
            {
                try
                {
                    switch(parameters.Count)
                    {
                        case 3:
                            {
                                int itemID = (int)parameters[(byte)FetchItemResponseParameterCode.ItemID];
                                string itemName = (string)parameters[(byte)FetchItemResponseParameterCode.ItemName];
                                string description = (string)parameters[(byte)FetchItemResponseParameterCode.Description];
                                ItemManager.Instance.AddItem(new Item(itemID, itemName, description));
                            }
                            return true;
                        case 6:
                            {
                                int itemID = (int)parameters[(byte)FetchMaterialResponseParameterCode.ItemID];
                                string itemName = (string)parameters[(byte)FetchMaterialResponseParameterCode.ItemName];
                                string description = (string)parameters[(byte)FetchMaterialResponseParameterCode.Description];
                                int materialID = (int)parameters[(byte)FetchMaterialResponseParameterCode.MaterialID];
                                int score = (int)parameters[(byte)FetchMaterialResponseParameterCode.Score];
                                GroupType groupType = (GroupType)parameters[(byte)FetchMaterialResponseParameterCode.GroupType];

                                ItemManager.Instance.AddItem(new Material(itemID, itemName, description, materialID, score, groupType));
                            }
                            return true;
                        default:
                            return false;
                    }
                }
                catch (InvalidCastException ex)
                {
                    LogService.Error("FetchItemResponse Parameter Cast Error");
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
