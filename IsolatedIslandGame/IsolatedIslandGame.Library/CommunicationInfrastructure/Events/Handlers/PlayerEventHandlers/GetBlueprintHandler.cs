using IsolatedIslandGame.Protocol.Communication.EventCodes;
using IsolatedIslandGame.Protocol.Communication.EventParameters.Player;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Handlers.PlayerEventHandlers
{
    class GetBlueprintHandler : EventHandler<Player, PlayerEventCode>
    {
        public GetBlueprintHandler(Player subject) : base(subject, 5)
        {
        }

        internal override bool Handle(PlayerEventCode eventCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(eventCode, parameters))
            {
                try
                {
                    int blueprintID = (int)parameters[(byte)GetBlueprintParameterCode.BlueprintID];
                    bool isOrderless = (bool)parameters[(byte)GetBlueprintParameterCode.IsOrderless];
                    bool isBlueprintRequired = (bool)parameters[(byte)GetBlueprintParameterCode.IsBlueprintRequired];
                    Blueprint.ElementInfo[] requirements = (Blueprint.ElementInfo[])parameters[(byte)GetBlueprintParameterCode.Requirements];
                    Blueprint.ElementInfo[] products = (Blueprint.ElementInfo[])parameters[(byte)GetBlueprintParameterCode.Products];

                    Blueprint blueprint = new Blueprint(blueprintID, isOrderless, isBlueprintRequired, requirements, products);
                    BlueprintManager.Instance.AddBlueprint(blueprint);
                    subject.GetBlueprint(blueprint);
                    return true;
                }
                catch (InvalidCastException ex)
                {
                    LogService.Error("GetBlueprint Parameter Cast Error");
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
