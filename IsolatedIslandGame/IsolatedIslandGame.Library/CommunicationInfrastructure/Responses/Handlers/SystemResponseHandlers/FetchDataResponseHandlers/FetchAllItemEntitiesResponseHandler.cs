using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataResponseParameters.System;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers.SystemResponseHandlers.FetchDataResponseHandlers
{
    class FetchAllItemEntitiesResponseHandler : FetchDataResponseHandler<SystemManager, SystemFetchDataCode>
    {
        public FetchAllItemEntitiesResponseHandler(SystemManager subject) : base(subject)
        {
        }

        public override bool CheckError(Dictionary<byte, object> parameters, ErrorCode returnCode, string debugMessage)
        {
            switch (returnCode)
            {
                case ErrorCode.NoError:
                    {
                        if (parameters.Count != 4)
                        {
                            LogService.ErrorFormat(string.Format("FetchAllItemEntitiesResponse Parameter Error, Parameter Count: {0}", parameters.Count));
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                default:
                    {
                        LogService.ErrorFormat("FetchAllItemEntitiesResponse Error DebugMessage: {0}", debugMessage);
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
                    int itemEntityID = (int)parameters[(byte)FetchAllItemEntitiesResponseParameterCode.ItemEntityID];
                    int itemID = (int)parameters[(byte)FetchAllItemEntitiesResponseParameterCode.ItemID];
                    float positionX = (float)parameters[(byte)FetchAllItemEntitiesResponseParameterCode.PositionX];
                    float positionZ = (float)parameters[(byte)FetchAllItemEntitiesResponseParameterCode.PositionZ];

                    ItemEntityManager.Instance.AddItemEntity(new ItemEntity(itemEntityID, itemID, positionX, positionZ));
                    return true;
                }
                catch (InvalidCastException ex)
                {
                    LogService.Error("FetchAllItemEntitiesResponse Parameter Cast Error");
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
