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
                List<Blueprint.ElementInfo>  elementInfos = ((Blueprint.ElementInfo[])parameters[(byte)SynthesizeMaterialParameterCode.BlueprintElementInfos]).ToList();

                lock(subject.Inventory)
                {
                    bool isReallyHaveTheseMaterials = true;
                    foreach (var info in elementInfos)
                    {
                        if (subject.Inventory.ItemCount(info.itemID) < info.itemCount)
                        {
                            isReallyHaveTheseMaterials = false;
                            break;
                        }
                    }
                    if (isReallyHaveTheseMaterials)
                    {
                        if (BlueprintManager.Instance.Blueprints.Any(x => x.IsSufficientRequirements(elementInfos)))
                        {
                            Blueprint blueprint = BlueprintManager.Instance.Blueprints.First(x => x.IsSufficientRequirements(elementInfos));
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
                                subject.Inventory.AddItem(ItemManager.Instance.FindItem(product.itemID), product.itemCount);
                            }
                            Dictionary<byte, object> responseParameters = new Dictionary<byte, object>
                            {
                                { (byte)SynthesizeMaterialResponseParameterCode.Requirements, blueprint.Requirements.ToArray() },
                                { (byte)SynthesizeMaterialResponseParameterCode.Products, blueprint.Products.ToArray() }
                            };
                            SendResponse(operationCode, responseParameters);
                            return true;
                        }
                        else
                        {
                            LogService.ErrorFormat("SynthesizeMaterial error Player: {0}, there is no such a blueprint", subject.IdentityInformation);
                            SendError(operationCode, ErrorCode.InvalidOperation, "there is no such a blueprint");
                            return false;
                        }
                    }
                    else
                    {
                        LogService.ErrorFormat("SynthesizeMaterial error Player: {0}, Player doesn't have these materials", subject.IdentityInformation);
                        SendError(operationCode, ErrorCode.PermissionDeny, "you don't have the materials");
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
