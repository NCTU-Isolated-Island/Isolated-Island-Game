using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataResponseParameters.System;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.SystemOperationHandlers.FetchDataHandlers
{
    class FetchAllVesselsHandler : SystemFetchDataHandler
    {
        public FetchAllVesselsHandler(SystemManager subject) : base(subject, 0)
        {
        }

        public override bool Handle(CommunicationInterface communicationInterface, SystemFetchDataCode fetchCode, Dictionary<byte, object> parameter)
        {
            if (base.Handle(communicationInterface, fetchCode, parameter))
            {
                try
                {
                    foreach (Vessel vessel in VesselManager.Instance.Vessels)
                    {
                        communicationInterface.User.Player.SyncPlayerInformation(vessel.OwnerPlayerID);
                        var result = new Dictionary<byte, object>
                        {
                            { (byte)FetchVesselResponseParameterCode.VesselID, vessel.VesselID },
                            { (byte)FetchVesselResponseParameterCode.OwnerPlayerID, vessel.OwnerPlayerID },
                            { (byte)FetchVesselResponseParameterCode.LocationX, vessel.LocationX },
                            { (byte)FetchVesselResponseParameterCode.LocationZ, vessel.LocationZ },
                            { (byte)FetchVesselResponseParameterCode.EulerAngleY, vessel.RotationEulerAngleY },
                        };
                        SendResponse(communicationInterface, SystemFetchDataCode.Vessel, result);
                    }
                    return true;
                }
                catch (InvalidCastException ex)
                {
                    LogService.ErrorFormat("FetchAllVessels Invalid Cast!");
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
