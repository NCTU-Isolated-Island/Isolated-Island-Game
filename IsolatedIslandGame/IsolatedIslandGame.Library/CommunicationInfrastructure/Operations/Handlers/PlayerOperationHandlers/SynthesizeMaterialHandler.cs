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
        public SynthesizeMaterialHandler(Player subject) : base(subject, 1)
        {
        }

        internal override bool Handle(PlayerOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(operationCode, parameters))
            {
                List<Blueprint.ElementInfo>  elementInfos = (SerializationHelper.BlueprintElementInfoArrayDeserialize((byte[])parameters[(byte)SynthesizeMaterialParameterCode.BlueprintElementInfosDataByteArray])).ToList();

                lock (subject.Inventory)
                {
                    Blueprint blueprint = BlueprintManager.Instance.Blueprints.FirstOrDefault(x => !x.IsBlueprintRequired && x.IsSufficientRequirements(elementInfos));
                    if (blueprint != null)
                    {
                        bool inventoryCheck = true;

                        foreach (var requirement in blueprint.Requirements)
                        {
                            if (!subject.Inventory.RemoveItemCheck(requirement.itemID, requirement.itemCount))
                            {
                                inventoryCheck = false;
                                break;
                            }
                        }
                        foreach (var product in blueprint.Products)
                        {
                            if (!subject.Inventory.AddItemCheck(product.itemID, product.itemCount))
                            {
                                inventoryCheck = false;
                                break;
                            }
                        }
                        if (inventoryCheck)
                        {
                            if (!subject.IsKnownBlueprint(blueprint.BlueprintID))
                            {
                                subject.GetBlueprint(blueprint);
                            }
                            foreach (var requirement in blueprint.Requirements)
                            {
                                subject.Inventory.RemoveItem(requirement.itemID, requirement.itemCount);
                            }
                            foreach (var product in blueprint.Products)
                            {
                                Item item;
                                if (ItemManager.Instance.FindItem(product.itemID, out item))
                                {
                                    subject.Inventory.AddItem(item, product.itemCount);
                                }
                            }
                            Dictionary<byte, object> responseParameters = new Dictionary<byte, object>
                            {
                                { (byte)SynthesizeMaterialResponseParameterCode.Requirements, SerializationHelper.BlueprintElementInfoArraySerialize(blueprint.Requirements.ToArray()) },
                                { (byte)SynthesizeMaterialResponseParameterCode.Products, SerializationHelper.BlueprintElementInfoArraySerialize(blueprint.Products.ToArray()) }
                            };
                            subject.User.EventManager.UserInform("成功", "合成成功。");
                            SendResponse(operationCode, responseParameters);
                            return true;
                        }
                        else
                        {
                            LogService.ErrorFormat("SynthesizeMaterial error Player: {0}, Player doesn't have these materials", subject.IdentityInformation);
                            subject.User.EventManager.UserInform("失敗", "素材不足。");
                            SendError(operationCode, ErrorCode.PermissionDeny, "you don't have the materials");
                            return false;
                        }
                    }
                    else
                    {
                        LogService.ErrorFormat("SynthesizeMaterial error Player: {0}, there is no such a blueprint", subject.IdentityInformation);
                        subject.User.EventManager.UserInform("失敗", "合成失敗。");
                        SendError(operationCode, ErrorCode.InvalidOperation, "there is no such a blueprint");
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }
    }
}
