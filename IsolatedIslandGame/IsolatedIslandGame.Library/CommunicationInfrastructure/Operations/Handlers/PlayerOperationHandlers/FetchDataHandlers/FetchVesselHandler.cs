using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataResponseParameters.Player;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.PlayerOperationHandlers.FetchDataHandlers
{
    class FetchVesselHandler : PlayerFetchDataHandler
    {
        public FetchVesselHandler(Player subject) : base(subject, 0)
        {
        }

        public override bool Handle(PlayerFetchDataCode fetchCode, Dictionary<byte, object> parameter)
        {
            if (base.Handle(fetchCode, parameter))
            {
                try
                {
                    Vessel vessel = subject.Vessel;
                    var result = new Dictionary<byte, object>
                    {
                        { (byte)FetchVesselResponseParameterCode.VesselID, vessel.VesselID },
                        { (byte)FetchVesselResponseParameterCode.OwnerPlayerID, vessel.OwnerPlayerID },
                        { (byte)FetchVesselResponseParameterCode.Name, vessel.Name },
                        { (byte)FetchVesselResponseParameterCode.LocationX, vessel.LocationX },
                        { (byte)FetchVesselResponseParameterCode.LocationZ, vessel.LocationZ },
                        { (byte)FetchVesselResponseParameterCode.EulerAngleX, vessel.Rotation.eulerAngles.x },
                        { (byte)FetchVesselResponseParameterCode.EulerAngleY, vessel.Rotation.eulerAngles.y },
                        { (byte)FetchVesselResponseParameterCode.EulerAngleZ, vessel.Rotation.eulerAngles.z },
                    };
                    SendResponse(fetchCode, result);
                    return true;
                }
                catch (InvalidCastException ex)
                {
                    LogService.ErrorFormat("FetchVessel Invalid Cast!");
                    LogService.Error(ex.Message);
                    LogService.Error(ex.StackTrace);
                    return false;
                }
                catch (Exception ex)
                {
                    LogService.Error(ex.Message);
                    LogService.Error(ex.StackTrace);
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
