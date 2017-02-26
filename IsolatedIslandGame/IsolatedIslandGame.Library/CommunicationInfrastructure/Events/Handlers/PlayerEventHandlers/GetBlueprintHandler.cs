using IsolatedIslandGame.Protocol.Communication.EventCodes;
using IsolatedIslandGame.Protocol.Communication.EventParameters.Player;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.PlayerEventHandlers
{
    class GetBlueprintHandler : EventHandler<Player, PlayerEventCode>
    {
        public GetBlueprintHandler(Player subject) : base(subject, 9)
        {
        }

        internal override bool Handle(PlayerEventCode eventCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(eventCode, parameters))
            {
                try
                {
                    int blueprintID = (int)parameters[(byte)GetBlueprintParameterCode.BlueprintID];
                    bool isOrderless = (bool)parameters[(byte)GetBlueprintParameterCode.IsOrderless];
                    bool isBlueprintRequired = (bool)parameters[(byte)GetBlueprintParameterCode.IsBlueprintRequired];
                    int[] requirementsItemID_Array = (int[])parameters[(byte)GetBlueprintParameterCode.RequirementsItemID_Array];
                    int[] requirementsItemCountArray = (int[])parameters[(byte)GetBlueprintParameterCode.RequirementsItemCountArray];
                    int[] requirementsPositionIndexArray = (int[])parameters[(byte)GetBlueprintParameterCode.RequirementsPositionIndexArray];
                    int[] productsItemID_Array = (int[])parameters[(byte)GetBlueprintParameterCode.ProductsItemID_Array];
                    int[] productsItemCountArray = (int[])parameters[(byte)GetBlueprintParameterCode.ProductsItemCountArray];
                    int[] productsPositionIndexArray = (int[])parameters[(byte)GetBlueprintParameterCode.ProductsPositionIndexArray];
                    Blueprint.ElementInfo[] requirements = new Blueprint.ElementInfo[requirementsItemID_Array.Length];
                    Blueprint.ElementInfo[] products = new Blueprint.ElementInfo[productsItemID_Array.Length];
                    for(int i = 0; i < requirementsItemID_Array.Length; i++)
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
                    LogService.Error("GetBlueprint Parameter Cast Error");
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
