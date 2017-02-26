using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.ResponseParameters.Player;
using System;
using System.Collections.Generic;
namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers.PlayerResponseHandlers
{
    class SynthesizeMaterialResponseHandler : ResponseHandler<Player, PlayerOperationCode>
    {
        public SynthesizeMaterialResponseHandler(Player subject) : base(subject)
        {
        }

        internal override bool CheckError(Dictionary<byte, object> parameters, ErrorCode returnCode, string debugMessage)
        {
            switch (returnCode)
            {
                case ErrorCode.NoError:
                    {
                        if (parameters.Count != 6)
                        {
                            LogService.ErrorFormat(string.Format("SynthesizeMaterial Parameter Error, Parameter Count: {0}", parameters.Count));
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                default:
                    {
                        LogService.ErrorFormat("SynthesizeMaterial Error DebugMessage: {0}", debugMessage);
                        subject.ResponseManager.SynthesizeMaterialResponse(returnCode, null, null);
                        return false;
                    }
            }
        }
        internal override bool Handle(PlayerOperationCode operationCode, ErrorCode returnCode, string debugMessage, Dictionary<byte, object> parameters)
        {
            if (base.Handle(operationCode, returnCode, debugMessage, parameters))
            {
                try
                {
                    int[] requirementsItemID_Array = (int[])parameters[(byte)SynthesizeMaterialResponseParameterCode.RequirementsItemID_Array];
                    int[] requirementsItemCountArray = (int[])parameters[(byte)SynthesizeMaterialResponseParameterCode.RequirementsItemCountArray];
                    int[] requirementsPositionIndexArray = (int[])parameters[(byte)SynthesizeMaterialResponseParameterCode.RequirementsPositionIndexArray];
                    int[] productsItemID_Array = (int[])parameters[(byte)SynthesizeMaterialResponseParameterCode.ProductsItemID_Array];
                    int[] productsItemCountArray = (int[])parameters[(byte)SynthesizeMaterialResponseParameterCode.ProductsItemCountArray];
                    int[] productsPositionIndexArray = (int[])parameters[(byte)SynthesizeMaterialResponseParameterCode.ProductsPositionIndexArray];
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

                    subject.ResponseManager.SynthesizeMaterialResponse(returnCode, requirements, products);
                    return true;
                }
                catch (InvalidCastException ex)
                {
                    LogService.Error("SynthesizeMaterial Parameter Cast Error");
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
