using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.OperationParameters.Player;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.PlayerOperationHandlers
{
    class UpdateDecorationOnVesselHandler : PlayerOperationHandler
    {
        public UpdateDecorationOnVesselHandler(Player subject) : base(subject, 7)
        {
        }

        internal override bool Handle(PlayerOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(operationCode, parameters))
            {
                int decorationID = (int)parameters[(byte)UpdateDecorationOnVesselParameterCode.DecorationID];
                float positionX = (float)parameters[(byte)UpdateDecorationOnVesselParameterCode.PositionX];
                float positionY = (float)parameters[(byte)UpdateDecorationOnVesselParameterCode.PositionY];
                float positionZ = (float)parameters[(byte)UpdateDecorationOnVesselParameterCode.PositionZ];
                float eulerAngleX = (float)parameters[(byte)UpdateDecorationOnVesselParameterCode.RotationEulerAngleX];
                float eulerAngleY = (float)parameters[(byte)UpdateDecorationOnVesselParameterCode.RotationEulerAngleY];
                float eulerAngleZ = (float)parameters[(byte)UpdateDecorationOnVesselParameterCode.RotationEulerAngleZ];

                lock(subject.Vessel)
                {
                    Decoration decoration;
                    if (subject.Vessel.FindDecoration(decorationID, out decoration))
                    {
                        decoration.UpdateDecoration(positionX, positionY, positionZ, eulerAngleX, eulerAngleY, eulerAngleZ);
                        LogService.InfoFormat("Player: {0}, UpdateDecorationOnVessel, DecorationID: {1}", subject.IdentityInformation, decorationID);
                        return true;
                    }
                    else
                    {
                        LogService.ErrorFormat("UpdateDecorationOnVessel error Player: {0}, don't have the decoration Decoration: {1}", subject.IdentityInformation, decorationID);
                        subject.User.EventManager.UserInform("錯誤", "更新船的裝飾錯誤，這艘船上並沒有此裝飾。");
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
