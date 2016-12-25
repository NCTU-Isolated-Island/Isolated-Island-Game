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

        protected Dictionary<int, Blueprint> blueprintDictionary;
        public IEnumerable<Blueprint> Blueprints { get { return blueprintDictionary.Values; } }
        public int BlueprintCount { get { return blueprintDictionary.Count; } }

        public delegate void BlueprintChangeEventHandler(Blueprint blueprint, DataChangeType changeType);
        public abstract event BlueprintChangeEventHandler OnBlueprintChange;

        protected BlueprintManager()
        {
            blueprintDictionary = new Dictionary<int, Blueprint>();
        }
        public bool ContainsBlueprint(int blueprintID)
        {
            return blueprintDictionary.ContainsKey(blueprintID);
        }
        public abstract bool FindBlueprint(int blueprintID, out Blueprint blueprint);
        public abstract void AddBlueprint(Blueprint blueprint);
        public abstract bool RemoveBlueprint(int blueprintID);
    }
}
