using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.OperationParameters.Player;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.PlayerOperationHandlers
{
    class UpdateVesselTransformHandler : PlayerOperationHandler
    {
        public UpdateVesselTransformHandler(Player subject) : base(subject, 4)
        {
        }

        internal override bool Handle(PlayerOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(operationCode, parameters))
            {
                float locationX = (float)parameters[(byte)UpdateVesselTransformParameterCode.LocationX];
                float locationZ = (float)parameters[(byte)UpdateVesselTransformParameterCode.LocatiomZ];
                float eulerAngleY = (float)parameters[(byte)UpdateVesselTransformParameterCode.EulerAngleY];
                OceanType locatedOceanType = (OceanType)parameters[(byte)UpdateVesselTransformParameterCode.LocatedOceanType];

                if (subject.Vessel != null)
                {
                    subject.Vessel.UpdateTransform(locationX, locationZ, eulerAngleY, locatedOceanType);
                    return true;
                }
                else
                {
                    LogService.ErrorFormat("UpdateVesselTransform error Player: {0}, doesn't have a vessel", subject.IdentityInformation);
                    subject.User.EventManager.UserInform("錯誤", "更新船的位置錯誤，你並沒有船。");
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
