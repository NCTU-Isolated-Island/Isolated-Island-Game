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
                            { (byte)FetchAllKnownBlueprintsResponseParameterCode.Requirements, blueprint.Requirements.ToArray() },
                            { (byte)FetchAllKnownBlueprintsResponseParameterCode.Products, blueprint.Products.ToArray() }
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
