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
                    foreach(Vessel vessel in VesselManager.Instance.Vessels)
                    {
                        var result = new Dictionary<byte, object>
                        {
                            { (byte)FetchAllVesselsResponseParameterCode.VesselID, vessel.VesselID },
                            { (byte)FetchAllVesselsResponseParameterCode.OwnerPlayerID, vessel.OwnerPlayerID },
                            { (byte)FetchAllVesselsResponseParameterCode.Name, vessel.Name },
                            { (byte)FetchAllVesselsResponseParameterCode.LocationX, vessel.LocationX },
                            { (byte)FetchAllVesselsResponseParameterCode.LocationZ, vessel.LocationZ },
                            { (byte)FetchAllVesselsResponseParameterCode.EulerAngleY, vessel.RotationEulerAngleY },
                        };
                        SendResponse(communicationInterface, fetchCode, result);
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
