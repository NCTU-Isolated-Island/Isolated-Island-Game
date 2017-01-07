using System;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Protocol;

namespace IsolatedIslandGame.Client
{
    public class ClientBlueprintManager : BlueprintManager
    {
        private event BlueprintChangeEventHandler onBlueprintChange;
        public override event BlueprintChangeEventHandler OnBlueprintChange { add { onBlueprintChange += value; } remove { onBlueprintChange -= value; } }

        public override void AddBlueprint(Blueprint blueprint)
        {
            if (!ContainsBlueprint(blueprint.BlueprintID))
            {
                blueprintDictionary.Add(blueprint.BlueprintID, blueprint);
                if(onBlueprintChange != null)
                {
                    onBlueprintChange.Invoke(blueprint, DataChangeType.Add);
                }
            }
        }

        public override bool FindBlueprint(int blueprintID, out Blueprint blueprint)
        {
            if(ContainsBlueprint(blueprintID))
            {
                blueprint = blueprintDictionary[blueprintID];
                return true;
            }
            else
            {
                blueprint = null;
                return false;
            }
        }

        public override bool RemoveBlueprint(int blueprintID)
        {
            if(ContainsBlueprint(blueprintID))
            {
                Blueprint blueprint = blueprintDictionary[blueprintID];
                blueprintDictionary.Remove(blueprintID);
                if (onBlueprintChange != null)
                {
                    onBlueprintChange.Invoke(blueprint, DataChangeType.Remove);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
