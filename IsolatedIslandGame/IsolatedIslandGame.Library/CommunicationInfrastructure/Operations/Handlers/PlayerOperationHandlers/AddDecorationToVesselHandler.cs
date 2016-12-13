using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.OperationParameters.Player;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.PlayerOperationHandlers
{
    class AddDecorationToVesselHandler : PlayerOperationHandler
    {
        public AddDecorationToVesselHandler(Player subject) : base(subject, 7)
        {
        }

        internal override bool Handle(PlayerOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(operationCode, parameters))
            {
                int materialItemID = (int)parameters[(byte)AddDecorationToVesselParameterCode.MaterialItemID];
                float positionX = (float)parameters[(byte)AddDecorationToVesselParameterCode.PositionX];
                float positionY = (float)parameters[(byte)AddDecorationToVesselParameterCode.PositionY];
                float positionZ = (float)parameters[(byte)AddDecorationToVesselParameterCode.PositionZ];
                float eulerAngleX = (float)parameters[(byte)AddDecorationToVesselParameterCode.RotationEulerAngleX];
                float eulerAngleY = (float)parameters[(byte)AddDecorationToVesselParameterCode.RotationEulerAngleY];
                float eulerAngleZ = (float)parameters[(byte)AddDecorationToVesselParameterCode.RotationEulerAngleZ];

                if (subject.Inventory.ItemCount(materialItemID) > 0)
                {
                    Item item = subject.Inventory.FindInventoryItemInfoByItemID(materialItemID).Item;
                    if (item is Material)
                    {
                        subject.Inventory.RemoveItem(materialItemID, 1);
                        Decoration decoration = DecorationFactory.Instance.CreateDecoration(subject.Vessel.VesselID, item as Material, positionX, positionY, positionZ, eulerAngleX, eulerAngleY, eulerAngleZ);
                        subject.Vessel.AddDecoration(decoration);
                        return true;
                    }
                    else
                    {
                        LogService.ErrorFormat("AddDecorationToVessel error Player: {0}, the item is not a material MaterialItemID: {1}", subject.IdentityInformation, materialItemID);
                        return false;
                    }
                }
                else
                {
                    LogService.ErrorFormat("AddDecorationToVessel error Player: {0}, don't have the material MaterialItemID: {1}", subject.IdentityInformation, materialItemID);
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
