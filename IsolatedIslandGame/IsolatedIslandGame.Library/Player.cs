using IsolatedIslandGame.Library.CommunicationInfrastructure.Events.Managers;
using IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Managers;
using IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Managers;
using IsolatedIslandGame.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace IsolatedIslandGame.Library
{
    public class Player : IIdentityProvidable
    {
        #region properties
        public User User { get; private set; }
        public int PlayerID { get; private set; }
        public ulong FacebookID { get; private set; }
        public string Nickname { get; private set; }
        public string Signature { get; private set; }
        public GroupType GroupType { get; private set; }
        public IPAddress LastConnectedIPAddress { get; set; }
        public string IdentityInformation { get { return string.Format("Player ID: {0}", PlayerID); } }
        public Inventory Inventory { get; private set; }
        public Vessel Vessel { get; private set; }
        private Dictionary<int, Blueprint> knownBlueprintDictionary;
        public IEnumerable<Blueprint> KnownBlueprints { get { return knownBlueprintDictionary.Values; } }
        private HashSet<int> friendPlayerIDSet;
        public IEnumerable<int> FriendPlayerIDs { get { return friendPlayerIDSet.ToArray(); } }

        public PlayerEventManager EventManager { get; private set; }
        public PlayerOperationManager OperationManager { get; private set; }
        public PlayerResponseManager ResponseManager { get; private set; }
        #endregion

        #region events
        private event Action<Player> onCreateCharacter;
        public event Action<Player> OnCreateCharacter { add { onCreateCharacter += value; } remove { onCreateCharacter -= value; } }

        private event Action<Inventory> onBindInventory;
        public event Action<Inventory> OnBindInventory { add { onBindInventory += value; } remove { onBindInventory -= value; } }

        private event Action<Vessel> onBindVessel;
        public event Action<Vessel> OnBindVessel { add { onBindVessel += value; } remove { onBindVessel -= value; } }

        private event Action<Blueprint> onGetBlueprint;
        public event Action<Blueprint> OnGetBlueprint { add { onGetBlueprint += value; } remove { onGetBlueprint -= value; } }

        #region response events
        public delegate void DrawMaterialEventHandler(Item material, int count);
        private event DrawMaterialEventHandler onDrawMaterial;
        public event DrawMaterialEventHandler OnDrawMaterial { add { onDrawMaterial += value; } remove { onDrawMaterial -= value; } }

        public delegate void SynthesizeMaterialEventHandler(Blueprint.ElementInfo[] requirements, Blueprint.ElementInfo[] products);
        private event SynthesizeMaterialEventHandler onSynthesizeMaterial;
        public event SynthesizeMaterialEventHandler OnSynthesizeMaterial { add { onSynthesizeMaterial += value; } remove { onSynthesizeMaterial -= value; } }

        private event Action<Blueprint> onUseBlueprint;
        public event Action<Blueprint> OnUseBlueprint { add { onUseBlueprint += value; } remove { onUseBlueprint -= value; } }
        #endregion
        #endregion

        public Player(int playerID, ulong facebookID, string nickname, string signature, GroupType groupType, IPAddress lastConnectedIPAddress)
        {
            PlayerID = playerID;
            FacebookID = facebookID;
            Nickname = nickname;
            Signature = signature;
            GroupType = groupType;
            LastConnectedIPAddress = lastConnectedIPAddress;

            EventManager = new PlayerEventManager(this);
            OperationManager = new PlayerOperationManager(this);
            ResponseManager = new PlayerResponseManager(this);

            knownBlueprintDictionary = new Dictionary<int, Blueprint>();
            friendPlayerIDSet = new HashSet<int>();
        }
        public void BindUser(User user)
        {
            User = user;
        }
        public void BindInventory(Inventory inventory)
        {
            Inventory = inventory;
            onBindInventory?.Invoke(Inventory);
        }
        public void BindVessel(Vessel vessel)
        {
            if(vessel.OwnerPlayerID == PlayerID)
            {
                Vessel = vessel;
                onBindVessel?.Invoke(Vessel);
            }
        }
        public void CreateCharacter(string nickname, string signature, GroupType groupType)
        {
            Nickname = nickname;
            Signature = signature;
            GroupType = groupType;
            onCreateCharacter?.Invoke(this);
        }

        public bool IsKnownBlueprint(int blueprintID)
        {
            return knownBlueprintDictionary.ContainsKey(blueprintID);
        }
        public void GetBlueprint(Blueprint blueprint)
        {
            if (!IsKnownBlueprint(blueprint.BlueprintID))
            {
                knownBlueprintDictionary.Add(blueprint.BlueprintID, blueprint);
                onGetBlueprint?.Invoke(blueprint);
            }
        }

        internal void TriggerDrawMaterialEvents(int itemID, int itemCount)
        {
            Item item;
            if(ItemManager.Instance.FindItem(itemID, out item))
            {
                onDrawMaterial?.Invoke(item, itemCount);
            }
        }
        internal void TriggerSynthesizeMaterialEvents(Blueprint.ElementInfo[] requirements, Blueprint.ElementInfo[] products)
        {
            onSynthesizeMaterial?.Invoke(requirements, products);
        }
        internal void TriggerUseBlueprintEvents(Blueprint blueprint)
        {
            onUseBlueprint?.Invoke(blueprint);
        }
    }
}
