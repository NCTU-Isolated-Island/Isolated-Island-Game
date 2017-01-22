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

                lock(subject.Inventory)
                {
                    if (subject.Inventory.RemoveItemCheck(materialItemID, 1))
                    {
                        InventoryItemInfo info;
                        if(subject.Inventory.FindInventoryItemInfoByItemID(materialItemID, out info))
                        {
                            if (info.Item is Material)
                            {
                                subject.Inventory.RemoveItem(materialItemID, 1);
                                Decoration decoration;
                                if(DecorationFactory.Instance.CreateDecoration(subject.Vessel.VesselID, info.Item as Material, positionX, positionY, positionZ, eulerAngleX, eulerAngleY, eulerAngleZ, out decoration))
                                {
                                    subject.Vessel.AddDecoration(decoration);
                                    LogService.InfoFormat("Player: {0}, AddDecorationToVessel, MaterialItemID: {1}", subject.IdentityInformation, materialItemID);
                                    return true;
                                }
                                else
                                {
                                    LogService.ErrorFormat("AddDecorationToVessel Error Player: {0}, item not existed MaterialItemID: {1}", subject.IdentityInformation, materialItemID);
                                    return false;
                                }
                            }
                            else
                            {
                                LogService.ErrorFormat("AddDecorationToVessel Error Player: {0}, the item is not a material MaterialItemID: {1}", subject.IdentityInformation, materialItemID);
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        LogService.ErrorFormat("AddDecorationToVessel error Player: {0}, don't have the material MaterialItemID: {1}", subject.IdentityInformation, materialItemID);
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
