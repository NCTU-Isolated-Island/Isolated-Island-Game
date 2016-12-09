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
                    Vessel vessel  = VesselManager.Instance.FindVessel(vesselID);
                    if (vessel != null)
                    {
                        foreach (var decoration in vessel.Decorations)
                        {
                            var result = new Dictionary<byte, object>
                            {
                                { (byte)FetchVesselDecorationsResponseParameterCode.VesselID, vessel.VesselID },
                                { (byte)FetchVesselDecorationsResponseParameterCode.DecorationID, decoration.DecorationID },
                                { (byte)FetchVesselDecorationsResponseParameterCode.MaterialItemID, decoration.Material.ItemID },
                                { (byte)FetchVesselDecorationsResponseParameterCode.PositionX, decoration.Position.x },
                                { (byte)FetchVesselDecorationsResponseParameterCode.PositionY, decoration.Position.y },
                                { (byte)FetchVesselDecorationsResponseParameterCode.PositionZ, decoration.Position.z },
                                { (byte)FetchVesselDecorationsResponseParameterCode.EulerAngleX, decoration.Rotation.eulerAngles.x },
                                { (byte)FetchVesselDecorationsResponseParameterCode.EulerAngleY, decoration.Rotation.eulerAngles.y },
                                { (byte)FetchVesselDecorationsResponseParameterCode.EulerAngleZ, decoration.Rotation.eulerAngles.z }
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
