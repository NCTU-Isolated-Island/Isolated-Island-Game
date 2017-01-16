using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataParameters.System;
using IsolatedIslandGame.Protocol.Communication.FetchDataResponseParameters.System;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.SystemOperationHandlers.FetchDataHandlers
{
    class FetchVesselHandler : SystemFetchDataHandler
    {
        public FetchVesselHandler(SystemManager subject) : base(subject, 1)
        {
        }

        public override bool Handle(CommunicationInterface communicationInterface, SystemFetchDataCode fetchCode, Dictionary<byte, object> parameter)
        {
            if (base.Handle(communicationInterface, fetchCode, parameter))
            {
                try
                {
                    int vesselID = (int)parameter[(byte)FetchVesselParameterCode.VesselID];
                    Vessel vessel;
                    if (VesselManager.Instance.FindVessel(vesselID, out vessel))
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
                        SendResponse(communicationInterface, fetchCode, result);
                        return true;
                    }
                    else
                    {
                        LogService.ErrorFormat("FetchVessel No Vessel VesselID: {0}", vesselID);
                        return false;
                    }
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
