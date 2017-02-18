using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataResponseParameters.Player;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers.PlayerResponseHandlers.FetchDataResponseHandlers
{
    class FetchAllKnownBlueprintsResponseHandler : FetchDataResponseHandler<Player, PlayerFetchDataCode>
    {
        public FetchAllKnownBlueprintsResponseHandler(Player subject) : base(subject)
        {
        }

        public override bool CheckError(Dictionary<byte, object> parameters, ErrorCode returnCode, string debugMessage)
        {
            switch (returnCode)
            {
                case ErrorCode.NoError:
                    {
                        if (parameters.Count != 5)
                        {
                            LogService.ErrorFormat(string.Format("FetchAllKnownBlueprintsResponse Parameter Error, Parameter Count: {0}", parameters.Count));
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                default:
                    {
                        LogService.ErrorFormat("FetchAllKnownBlueprintsResponse Error DebugMessage: {0}", debugMessage);
                        return false;
                    }
            }
        }

        public override bool Handle(PlayerFetchDataCode fetchCode, ErrorCode returnCode, string fetchDebugMessage, Dictionary<byte, object> parameters)
        {
            if (base.Handle(fetchCode, returnCode, fetchDebugMessage, parameters))
            {
                try
                {
                    int blueprintID = (int)parameters[(byte)FetchAllKnownBlueprintsResponseParameterCode.BlueprintID];
                    bool isOrderless = (bool)parameters[(byte)FetchAllKnownBlueprintsResponseParameterCode.IsOrderless];
                    bool isBlueprintRequired = (bool)parameters[(byte)FetchAllKnownBlueprintsResponseParameterCode.IsBlueprintRequired];
                    Blueprint.ElementInfo[] requirements = SerializationHelper.TypeDeserialize<Blueprint.ElementInfo[]>((byte[])parameters[(byte)FetchAllKnownBlueprintsResponseParameterCode.Requirements]);
                    Blueprint.ElementInfo[] products = SerializationHelper.TypeDeserialize<Blueprint.ElementInfo[]>((byte[])parameters[(byte)FetchAllKnownBlueprintsResponseParameterCode.Products]);

                    Blueprint blueprint = new Blueprint(blueprintID, isOrderless, isBlueprintRequired, requirements, products);
                    BlueprintManager.Instance.AddBlueprint(blueprint);
                    subject.GetBlueprint(blueprint);
                    return true;
                }
                catch (InvalidCastException ex)
                {
                    LogService.Error("FetchVesselDecorationsResponse Parameter Cast Error");
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
