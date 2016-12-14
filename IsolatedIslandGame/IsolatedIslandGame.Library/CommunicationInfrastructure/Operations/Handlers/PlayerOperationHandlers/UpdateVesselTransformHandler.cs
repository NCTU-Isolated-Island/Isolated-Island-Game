using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.OperationParameters.Player;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.PlayerOperationHandlers
{
    class UpdateVesselTransformHandler : PlayerOperationHandler
    {
        public UpdateVesselTransformHandler(Player subject) : base(subject, 3)
        {
        }

        internal override bool Handle(PlayerOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(operationCode, parameters))
            {
                float locationX = (float)parameters[(byte)UpdateVesselTransformParameterCode.LocationX];
                float locationZ = (float)parameters[(byte)UpdateVesselTransformParameterCode.LocatiomZ];
                float eulerAngleY = (float)parameters[(byte)UpdateVesselTransformParameterCode.EulerAngleY];

                if(subject.Vessel != null)
                {
                    subject.Vessel.UpdateTransform(locationX, locationZ, eulerAngleY);
                    return true;
                }
                else
                {
                    LogService.ErrorFormat("UpdateVesselTransform error Player: {0}, doesn't have a vessel", subject.IdentityInformation);
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
