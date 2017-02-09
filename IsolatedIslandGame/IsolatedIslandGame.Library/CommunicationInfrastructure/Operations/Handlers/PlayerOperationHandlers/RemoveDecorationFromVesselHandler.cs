using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.OperationParameters.Player;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.PlayerOperationHandlers
{
    class RemoveDecorationFromVesselHandler : PlayerOperationHandler
    {
        public RemoveDecorationFromVesselHandler(Player subject) : base(subject, 1)
        {
        }

        internal override bool Handle(PlayerOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(operationCode, parameters))
            {
                int decorationID = (int)parameters[(byte)RemoveDecorationFromVesselParameterCode.DecorationID];

                lock(subject.Vessel)
                {
                    lock(subject.Inventory)
                    {
                        Decoration decoration;
                        if (subject.Vessel.FindDecoration(decorationID, out decoration))
                        {
                            Item material = decoration.Material;
                            if (subject.Inventory.AddItemCheck(material.ItemID, 1))
                            {
                                subject.Inventory.AddItem(material, 1);
                                subject.Vessel.RemoveDecoration(decorationID);
                                DecorationFactory.Instance.DeleteDecoration(decorationID);
                                LogService.InfoFormat("Player: {0}, RemoveDecorationFromVessel, MaterialItemID: {1}", subject.IdentityInformation, material.ItemID);
                                return true;
                            }
                            else
                            {
                                LogService.ErrorFormat("Player: {0}, RemoveDecorationFromVessel AddItem To Inventory Fail, MaterialItemID: {1}", subject.IdentityInformation, material.ItemID);
                                subject.User.EventManager.UserInform("錯誤", "物品欄已滿。");
                                return false;
                            }
                        }
                        else
                        {
                            LogService.ErrorFormat("RemoveDecorationFromVessel error Player: {0}, the decoration is not existed DecorationID: {1}", subject.IdentityInformation, decorationID);
                            subject.User.EventManager.UserInform("錯誤", "無法移除不存在於船上的裝飾。");
                            return false;
                        }
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
