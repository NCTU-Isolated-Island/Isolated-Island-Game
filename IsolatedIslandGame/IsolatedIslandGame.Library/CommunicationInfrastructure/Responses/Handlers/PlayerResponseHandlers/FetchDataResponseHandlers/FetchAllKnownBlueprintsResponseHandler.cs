using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataResponseParameters.Player;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers.PlayerResponseHandlers.FetchDataResponseHandlers
{
    class FetchAllKnownBlueprintsResponseHandler : FetchDataResponseHandler<Player, PlayerFetchDataCode>
    {
        public FetchAllKnownBlueprintsResponseHandler(Player subject) : base(subject)
        {
        }

        public override bool CheckError(Dictionary<byte, object> parameters, ErrorCode returnCode, string debugMessage)
        {
            switch (returnCode)
            {
                case ErrorCode.NoError:
                    {
                        if (parameters.Count != 9)
                        {
                            LogService.ErrorFormat(string.Format("FetchAllKnownBlueprintsResponse Parameter Error, Parameter Count: {0}", parameters.Count));
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                default:
                    {
                        LogService.ErrorFormat("FetchAllKnownBlueprintsResponse Error DebugMessage: {0}", debugMessage);
                        return false;
                    }
            }
        }

        public override bool Handle(PlayerFetchDataCode fetchCode, ErrorCode returnCode, string fetchDebugMessage, Dictionary<byte, object> parameters)
        {
            if (base.Handle(fetchCode, returnCode, fetchDebugMessage, parameters))
            {
                try
                {
                    int blueprintID = (int)parameters[(byte)FetchAllKnownBlueprintsResponseParameterCode.BlueprintID];
                    bool isOrderless = (bool)parameters[(byte)FetchAllKnownBlueprintsResponseParameterCode.IsOrderless];
                    bool isBlueprintRequired = (bool)parameters[(byte)FetchAllKnownBlueprintsResponseParameterCode.IsBlueprintRequired];
                    int[] requirementsItemID_Array = (int[])parameters[(byte)FetchAllKnownBlueprintsResponseParameterCode.RequirementsItemID_Array];
                    int[] requirementsItemCountArray = (int[])parameters[(byte)FetchAllKnownBlueprintsResponseParameterCode.RequirementsItemCountArray];
                    int[] requirementsPositionIndexArray = (int[])parameters[(byte)FetchAllKnownBlueprintsResponseParameterCode.RequirementsPositionIndexArray];
                    int[] productsItemID_Array = (int[])parameters[(byte)FetchAllKnownBlueprintsResponseParameterCode.ProductsItemID_Array];
                    int[] productsItemCountArray = (int[])parameters[(byte)FetchAllKnownBlueprintsResponseParameterCode.ProductsItemCountArray];
                    int[] productsPositionIndexArray = (int[])parameters[(byte)FetchAllKnownBlueprintsResponseParameterCode.ProductsPositionIndexArray];
                    Blueprint.ElementInfo[] requirements = new Blueprint.ElementInfo[requirementsItemID_Array.Length];
                    Blueprint.ElementInfo[] products = new Blueprint.ElementInfo[productsItemID_Array.Length];
                    for (int i = 0; i < requirementsItemID_Array.Length; i++)
                    {
                        requirements[i].itemID = requirementsItemID_Array[i];
                        requirements[i].itemCount = requirementsItemCountArray[i];
                        requirements[i].positionIndex = requirementsPositionIndexArray[i];
                    }
                    for (int i = 0; i < productsItemID_Array.Length; i++)
                    {
                        products[i].itemID = productsItemID_Array[i];
                        products[i].itemCount = productsItemCountArray[i];
                        products[i].positionIndex = productsPositionIndexArray[i];
                    }

                    Blueprint blueprint = new Blueprint(blueprintID, isOrderless, isBlueprintRequired, requirements, products);
                    BlueprintManager.Instance.AddBlueprint(blueprint);
                    subject.GetBlueprint(blueprint);
                    return true;
                }
                catch (InvalidCastException ex)
                {
                    LogService.Error("FetchVesselDecorationsResponse Parameter Cast Error");
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
