﻿using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataParameters.System;
using IsolatedIslandGame.Protocol.Communication.FetchDataResponseParameters.System;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.SystemOperationHandlers.FetchDataHandlers
{
    class FetchVesselWithOwnerPlayerIDHandler : SystemFetchDataHandler
    {
        public FetchVesselWithOwnerPlayerIDHandler(SystemManager subject) : base(subject, 1)
        {
        }

        public override bool Handle(CommunicationInterface communicationInterface, SystemFetchDataCode fetchCode, Dictionary<byte, object> parameter)
        {
            if (base.Handle(communicationInterface, fetchCode, parameter))
            {
                try
                {
                    int ownerPlayerID = (int)parameter[(byte)FetchVesselWithOwnerPlayerIDParameterCode.OwnerPlayerID];
                    Vessel vessel = VesselManager.Instance.FindVesselByOwnerPlayerID(ownerPlayerID);
                    if (vessel != null)
                    {
                        var result = new Dictionary<byte, object>
                        {
                            { (byte)FetchVesselWithOwnerPlayerIDResponseParameterCode.VesselID, vessel.VesselID },
                            { (byte)FetchVesselWithOwnerPlayerIDResponseParameterCode.OwnerPlayerID, vessel.OwnerPlayerID },
                            { (byte)FetchVesselWithOwnerPlayerIDResponseParameterCode.Name, vessel.Name },
                            { (byte)FetchVesselWithOwnerPlayerIDResponseParameterCode.LocationX, vessel.LocationX },
                            { (byte)FetchVesselWithOwnerPlayerIDResponseParameterCode.LocationZ, vessel.LocationZ },
                            { (byte)FetchVesselWithOwnerPlayerIDResponseParameterCode.EulerAngleX, vessel.RotationEulerAngleX },
                            { (byte)FetchVesselWithOwnerPlayerIDResponseParameterCode.EulerAngleY, vessel.RotationEulerAngleY },
                            { (byte)FetchVesselWithOwnerPlayerIDResponseParameterCode.EulerAngleZ, vessel.RotationEulerAngleZ }
                        };
                        SendResponse(communicationInterface, fetchCode, result);
                        return true;
                    }
                    else
                    {
                        LogService.ErrorFormat("FetchVesselWithOwnerPlayerID No Vessel OwnerPlayerID: {0}", ownerPlayerID);
                        return false;
                    }
                }
                catch (InvalidCastException ex)
                {
                    LogService.ErrorFormat("FetchVesselWithOwnerPlayerID Invalid Cast!");
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
