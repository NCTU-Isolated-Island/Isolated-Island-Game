using IsolatedIslandGame.Protocol;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library
{
    public abstract class BlueprintManager
    {
        public static BlueprintManager Instance { get; private set; }
        public static void Initial(BlueprintManager manager)
        {
            Instance = manager;
        }

        protected Dictionary<int, Blueprint> blueprintDictionary = new Dictionary<int, Blueprint>();
        public IEnumerable<Blueprint> Blueprints { get { return blueprintDictionary.Values; } }
        public int BlueprintCount { get { return blueprintDictionary.Count; } }

        public delegate void BlueprintChangeEventHandler(Blueprint blueprint, DataChangeType changeType);
        private event BlueprintChangeEventHandler onBlueprintChange;
        public event BlueprintChangeEventHandler OnBlueprintChange { add { onBlueprintChange += value; } remove { onBlueprintChange -= value; } }

        protected BlueprintManager()
        {
        }
        public bool ContainsBlueprint(int blueprintID)
        {
            return blueprintDictionary.ContainsKey(blueprintID);
        }
        public void AddBlueprint(Blueprint blueprint)
        {
            if (!ContainsBlueprint(blueprint.BlueprintID))
            {
                blueprintDictionary.Add(blueprint.BlueprintID, blueprint);
                if (onBlueprintChange != null)
                {
                    onBlueprintChange.Invoke(blueprint, DataChangeType.Add);
                }
            }
        }

        public bool FindBlueprint(int blueprintID, out Blueprint blueprint)
        {
            if (ContainsBlueprint(blueprintID))
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

        public bool RemoveBlueprint(int blueprintID)
        {
            if (ContainsBlueprint(blueprintID))
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
