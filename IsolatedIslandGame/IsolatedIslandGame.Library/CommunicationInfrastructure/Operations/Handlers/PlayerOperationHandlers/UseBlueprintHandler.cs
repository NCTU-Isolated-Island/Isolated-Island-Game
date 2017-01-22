using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.OperationParameters.Player;
using IsolatedIslandGame.Protocol.Communication.ResponseParameters.Player;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.PlayerOperationHandlers
{
    class UseBlueprintHandler : PlayerOperationHandler
    {
        public UseBlueprintHandler(Player subject) : base(subject, 1)
        {
        }

        internal override bool Handle(PlayerOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(operationCode, parameters))
            {
                int blueprintID = (int)parameters[(byte)UseBlueprintParameterCode.BlueprintID];

                if(!BlueprintManager.Instance.ContainsBlueprint(blueprintID))
                {
                    SendError(operationCode, ErrorCode.ParameterError, "Blueprint is not existed");
                    LogService.ErrorFormat("UseBlueprint error Player: {0}, the blueprint is not existed BlueprintID: {1}", subject.IdentityInformation, blueprintID);
                    return false;
                }
                else if (!subject.IsKnownBlueprint(blueprintID))
                {
                    SendError(operationCode, ErrorCode.ParameterError, "You don't know the blueprint");
                    LogService.ErrorFormat("UseBlueprint error Player: {0}, the blueprint is not known by the player, BlueprintID: {1}", subject.IdentityInformation, blueprintID);
                    return false;
                }
                else
                {
                    Blueprint blueprint;
                    if (BlueprintManager.Instance.FindBlueprint(blueprintID, out blueprint))
                    {
                        lock (subject.Inventory)
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
                                foreach (var requirement in blueprint.Requirements)
                                {
                                    subject.Inventory.RemoveItem(requirement.itemID, requirement.itemCount);
                                }
                                foreach (var product in blueprint.Products)
                                {
                                    Item item;
                                    if(ItemManager.Instance.FindItem(product.itemID, out item))
                                    {
                                        subject.Inventory.AddItem(item, product.itemCount);
                                    }
                                }
                                Dictionary<byte, object> responseParameters = new Dictionary<byte, object>
                            {
                                { (byte)UseBlueprintResponseParameterCode.BlueprintID, blueprintID }
                            };
                                SendResponse(operationCode, responseParameters);
                                return true;
                            }
                            else
                            {
                                SendError(operationCode, ErrorCode.Fail, "You don't have sufficient materials");
                                LogService.ErrorFormat("UseBlueprint error Player: {0}, Player doesn't have enough materials, BlueprintID: {1}", subject.IdentityInformation, blueprintID);
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
            else
            {
                return false;
            }
        }
    }
}
