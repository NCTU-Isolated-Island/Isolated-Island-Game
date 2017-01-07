using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataParameters.Player;
using IsolatedIslandGame.Protocol.Communication.FetchDataResponseParameters.Player;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.SystemOperationHandlers.FetchDataHandlers
{
    class FetchVesselDecorationsHandler : SystemFetchDataHandler
    {
        public FetchVesselDecorationsHandler(SystemManager subject) : base(subject, 1)
        {
        }

        public override bool Handle(CommunicationInterface communicationInterface, SystemFetchDataCode fetchCode, Dictionary<byte, object> parameter)
        {
            if (base.Handle(communicationInterface, fetchCode, parameter))
            {
                try
                {
                    int vesselID = (int)parameter[(byte)FetchVesselDecorationsParameterCode.VesselID];
                    Vessel vessel;
                    if (VesselManager.Instance.FindVessel(vesselID, out vessel))
                    {
                        foreach (var decoration in vessel.Decorations)
                        {
                            var result = new Dictionary<byte, object>
                            {
                                { (byte)FetchVesselDecorationsResponseParameterCode.VesselID, vessel.VesselID },
                                { (byte)FetchVesselDecorationsResponseParameterCode.DecorationID, decoration.DecorationID },
                                { (byte)FetchVesselDecorationsResponseParameterCode.MaterialItemID, decoration.Material.ItemID },
                                { (byte)FetchVesselDecorationsResponseParameterCode.PositionX, decoration.PositionX },
                                { (byte)FetchVesselDecorationsResponseParameterCode.PositionY, decoration.PositionY },
                                { (byte)FetchVesselDecorationsResponseParameterCode.PositionZ, decoration.PositionZ },
                                { (byte)FetchVesselDecorationsResponseParameterCode.EulerAngleX, decoration.RotationEulerAngleX },
                                { (byte)FetchVesselDecorationsResponseParameterCode.EulerAngleY, decoration.RotationEulerAngleY },
                                { (byte)FetchVesselDecorationsResponseParameterCode.EulerAngleZ, decoration.RotationEulerAngleZ }
                            };
                            SendResponse(communicationInterface, fetchCode, result);
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (InvalidCastException ex)
                {
                    LogService.ErrorFormat("FetchVesselDecorations Invalid Cast!");
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
