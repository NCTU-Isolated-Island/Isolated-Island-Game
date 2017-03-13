using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.OperationParameters.Player;
using IsolatedIslandGame.Protocol.Communication.ResponseParameters.Player;
using System.Collections.Generic;
using System.Linq;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.PlayerOperationHandlers
{
    class SynthesizeMaterialHandler : PlayerOperationHandler
    {
        public SynthesizeMaterialHandler(Player subject) : base(subject, 3)
        {
        }

        internal override bool Handle(PlayerOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(operationCode, parameters))
            {
                int[] blueprintElementInfosItemID_Array = (int[])parameters[(byte)SynthesizeMaterialParameterCode.BlueprintElementInfosItemID_Array];
                int[] blueprintElementInfosItemCountArray = (int[])parameters[(byte)SynthesizeMaterialParameterCode.BlueprintElementInfosItemCountArray];
                int[] blueprintElementInfosPositionIndexArray = (int[])parameters[(byte)SynthesizeMaterialParameterCode.BlueprintElementInfosPositionIndexArray];
                List<Blueprint.ElementInfo> elementInfos = new List<Blueprint.ElementInfo>();
                for(int i = 0; i < blueprintElementInfosItemID_Array.Length; i++)
                {
                    elementInfos.Add(new Blueprint.ElementInfo
                    {
                        itemID = blueprintElementInfosItemID_Array[i],
                        itemCount = blueprintElementInfosItemCountArray[i],
                        positionIndex = blueprintElementInfosPositionIndexArray[i]
                    });
                }
                Blueprint resultBlueprint;
                if(subject.SynthesizeMaterial(elementInfos, out resultBlueprint))
                {
                    Dictionary<byte, object> responseParameters = new Dictionary<byte, object>
                    {
                        { (byte)SynthesizeMaterialResponseParameterCode.RequirementsItemID_Array, resultBlueprint.Requirements.Select(x => x.itemID).ToArray() },
                        { (byte)SynthesizeMaterialResponseParameterCode.RequirementsItemCountArray, resultBlueprint.Requirements.Select(x => x.itemCount).ToArray() },
                        { (byte)SynthesizeMaterialResponseParameterCode.RequirementsPositionIndexArray, resultBlueprint.Requirements.Select(x => x.positionIndex).ToArray() },
                        { (byte)SynthesizeMaterialResponseParameterCode.ProductsItemID_Array, resultBlueprint.Products.Select(x => x.itemID).ToArray() },
                        { (byte)SynthesizeMaterialResponseParameterCode.ProductsItemCountArray, resultBlueprint.Products.Select(x => x.itemCount).ToArray() },
                        { (byte)SynthesizeMaterialResponseParameterCode.ProductsPositionIndexArray, resultBlueprint.Products.Select(x => x.positionIndex).ToArray() }
                    };
                    subject.User.EventManager.UserInform("成功", "合成成功。");
                    SendResponse(operationCode, responseParameters);
                    return true;
                }
                else if(resultBlueprint != null)
                {
                    LogService.ErrorFormat("SynthesizeMaterial error Player: {0}, Player doesn't have these materials", subject.IdentityInformation);
                    subject.User.EventManager.UserInform("失敗", "素材不足。");
                    SendError(operationCode, ErrorCode.PermissionDeny, "you don't have the materials");
                    return false;
                }
                else
                {
                    LogService.ErrorFormat("SynthesizeMaterial error Player: {0}, there is no such a blueprint", subject.IdentityInformation);
                    subject.User.EventManager.UserInform("失敗", "合成失敗。(抱歉，現在還有很多藍圖還沒開放，你可能使用了正確的配方但是成品還沒被加入遊戲中)");
                    SendError(operationCode, ErrorCode.InvalidOperation, "there is no such a blueprint");
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
