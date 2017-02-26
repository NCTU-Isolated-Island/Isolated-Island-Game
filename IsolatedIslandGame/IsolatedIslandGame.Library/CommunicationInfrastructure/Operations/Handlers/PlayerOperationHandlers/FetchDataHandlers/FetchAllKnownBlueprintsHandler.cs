using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataResponseParameters.Player;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.PlayerOperationHandlers.FetchDataHandlers
{
    class FetchAllKnownBlueprintsHandler : PlayerFetchDataHandler
    {
        public FetchAllKnownBlueprintsHandler(Player subject) : base(subject, 0)
        {
        }

        public override bool Handle(PlayerFetchDataCode fetchCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(fetchCode, parameters))
            {
                try
                {
                    foreach (var blueprint in subject.KnownBlueprints)
                    {
                        var result = new Dictionary<byte, object>
                        {
                            { (byte)FetchAllKnownBlueprintsResponseParameterCode.BlueprintID, blueprint.BlueprintID },
                            { (byte)FetchAllKnownBlueprintsResponseParameterCode.IsOrderless, blueprint.IsOrderless },
                            { (byte)FetchAllKnownBlueprintsResponseParameterCode.IsBlueprintRequired, blueprint.IsBlueprintRequired },
                            { (byte)FetchAllKnownBlueprintsResponseParameterCode.RequirementsItemID_Array, blueprint.Requirements.Select(x => x.itemID).ToArray() },
                            { (byte)FetchAllKnownBlueprintsResponseParameterCode.RequirementsItemCountArray, blueprint.Requirements.Select(x => x.itemCount).ToArray() },
                            { (byte)FetchAllKnownBlueprintsResponseParameterCode.RequirementsPositionIndexArray, blueprint.Requirements.Select(x => x.positionIndex).ToArray() },
                            { (byte)FetchAllKnownBlueprintsResponseParameterCode.ProductsItemID_Array, blueprint.Products.Select(x => x.itemID).ToArray() },
                            { (byte)FetchAllKnownBlueprintsResponseParameterCode.ProductsItemCountArray, blueprint.Products.Select(x => x.itemCount).ToArray() },
                            { (byte)FetchAllKnownBlueprintsResponseParameterCode.ProductsPositionIndexArray, blueprint.Products.Select(x => x.positionIndex).ToArray() }
                        };
                        SendResponse(fetchCode, result);
                    }
                    return true;
                }
                catch (InvalidCastException ex)
                {
                    LogService.ErrorFormat("FetchAllKnownBlueprints Invalid Cast!");
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
